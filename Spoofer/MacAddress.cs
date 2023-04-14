using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Functions;

namespace SpooferApp
{
    public class MacAddress
    {
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
                                object oldValue = key.GetValue(valueName);

                                if (oldValue != null && oldValue.GetType() == typeof(string))
                                {
                                    RegistrySearch(RegistryHive.LocalMachine, subkey.Key, valueName, GenerateIncrementedMacAddress(oldValue.ToString()));
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
