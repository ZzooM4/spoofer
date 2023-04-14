using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using static Functions;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct IP_ADAPTER_INFO
{
    public IntPtr Next;
    public int ComboIndex;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string AdapterName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
    public string Description;
    public uint AddressLength;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] Address;
    public uint Index;
    public uint Type;
    public uint DhcpEnabled;
    public IntPtr CurrentIpAddress;
    public IP_ADDR_STRING IpAddressList;
    public IP_ADDR_STRING GatewayList;
    public IP_ADDR_STRING DhcpServer;
    public bool HaveWins;
    public IP_ADDR_STRING PrimaryWinsServer;
    public IP_ADDR_STRING SecondaryWinsServer;
    public int LeaseObtained;
    public int LeaseExpires;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct IP_ADDR_STRING
{
    public IntPtr Next;
    public IP_ADDRESS_STRING IpAddress;
    public IP_ADDRESS_STRING IpMask;
    public uint Context;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct IP_ADDRESS_STRING
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string Address;
}

namespace SpooferApp
{
    public class MacAddress
    {
        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        private static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref uint pOutBufLen);

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int GetAdaptersInfo(byte[] pAdapterInfo, ref uint pOutBufLen);

        /// <summary>
        /// Spoofs the MAC address by updating the registry values for NetworkAddress, NetworkAddresses, and OriginalNetworkAddress.
        /// This method also updates the PnPCapabilities and resets the NIC adapters to apply the changes.
        /// </summary>

        public static void SpoofMacAddress()
        {
            var registrySubkeys = new Dictionary<string, List<string>>
        {
            {
                @"SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}",
                new List<string> { "NetworkAddress", "NetworkAddresses", "OriginalNetworkAddress" }
            },
            {
                @"SYSTEM\ControlSet001\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}",
                new List<string> { "NetworkAddress", "NetworkAddresses", "OriginalNetworkAddress" }
            }
        };

            Dictionary<string, string> adapters = GetAdapters();
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                foreach (var subkey in registrySubkeys)
                {
                    using (RegistryKey key = baseKey.OpenSubKey(subkey.Key, true))
                    {
                        if (key != null)
                        {
                            foreach (var valueName in subkey.Value)
                            {
                                foreach (var adapter in adapters)
                                {
                                    string oldValue = adapter.Value;

                                    if (oldValue != null && oldValue.GetType() == typeof(string))
                                    {
                                        RegistrySearch(RegistryHive.LocalMachine, subkey.Key, valueName, GenerateIncrementedMacAddress(oldValue));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            RegistrySearch(
                RegistryHive.LocalMachine,
                @"SYSTEM\CurrentControlSet\Control\Class\{4D36E972-E325-11CE-BFC1-08002bE10318}",
                "PnPCapabilities",
                24
            );

            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c \"wmic nic where (netconnectionid like '%%') get netconnectionid,netconnectionstatus /format:csv\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line.StartsWith(","))
                {
                    string netConnectionId = line.Split(',')[1];
                    ToggleNICAdapter(netConnectionId, "disable");
                    ToggleNICAdapter(netConnectionId, "enable");
                }
            }
        }

        public static Dictionary<string, string> GetAdapters()
        {
            var result = new Dictionary<string, string>();

            uint outBufLen = 0;
            GetAdaptersInfo(IntPtr.Zero, ref outBufLen);

            if (outBufLen != 0)
            {
                IntPtr buffer = Marshal.AllocHGlobal((int)outBufLen);
                int errorCode = GetAdaptersInfo(buffer, ref outBufLen);

                if (errorCode == 0)
                {
                    IntPtr current = buffer;

                    while (current != IntPtr.Zero)
                    {
                        IP_ADAPTER_INFO adapterInfo = (IP_ADAPTER_INFO)Marshal.PtrToStructure(current, typeof(IP_ADAPTER_INFO));
                        byte[] addressBytes = new byte[adapterInfo.AddressLength];
                        for (int i = 0; i < addressBytes.Length; i++)
                        {
                            addressBytes[i] = Marshal.ReadByte(adapterInfo.Address, i);
                        }
                        string macAddress = string.Join("-", addressBytes.Select(b => b.ToString("X2")));

                        result.Add(adapterInfo.Description, macAddress);
                        current = adapterInfo.Next;
                    }
                }
                else
                {
                    Console.WriteLine($"[!] GetAdaptersInfo failed with error: {errorCode}");
                }

                Marshal.FreeHGlobal(buffer);
            }

            return result;
        }
        /// <summary>
        /// Toggles the NIC Adapters
        /// </summary>
        private static void ToggleNICAdapter(string netConnectionId, string action)
        {
            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "netsh.exe",
                    Arguments = $"interface set interface name=\"{netConnectionId}\" {action}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Takes the old Mac value, then increments it by its position times 2.
        /// </summary>
        private static string GenerateIncrementedMacAddress(string oldMac)
        {
            string macPattern = "^([0-9A-Fa-f]{2}[-]){5}([0-9A-Fa-f]{2})$";
            if (!Regex.IsMatch(oldMac, macPattern))
            {
                Console.WriteLine($"Invalid MAC address format: {oldMac}. Generating a random MAC address.");
                return GenerateRandomMacAddress();
            }

            string[] oldMacParts = oldMac.Split('-');
            byte[] incrementedMacBytes = new byte[6];

            for (int i = 0; i < oldMacParts.Length; i++)
            {
                byte macPart = Convert.ToByte(oldMacParts[i], 16);
                macPart = (byte)((macPart + (i + 1) * 2) % 256);
                incrementedMacBytes[i] = macPart;
            }

            return string.Join("-", incrementedMacBytes.Select(b => b.ToString("X2")));
        }

        /// <summary>
        /// If GenerateIncrementedMacAddress sees that the Mac Address isn't in valid format, it'll generate a new one.
        /// </summary>
        private static string GenerateRandomMacAddress()
        {
            Random random = new Random();
            byte[] randomMacBytes = new byte[6];
            random.NextBytes(randomMacBytes);
            return string.Join("-", randomMacBytes.Select(b => b.ToString("X2")));
        }
    }
}
