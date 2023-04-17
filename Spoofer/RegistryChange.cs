using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static Functions;

namespace SpooferApp
{
    public class RegistryChange
    {
        /// <summary>
        /// Reads the Keys/Values from Config.json and deletes/edits/... them.
        /// </summary>
        /// 

        public static void SpoofRegistry()
        {


            string configFilePath = "Config.json";
            string json = File.ReadAllText(configFilePath);
            dynamic config = JsonConvert.DeserializeObject(json);

            foreach (dynamic entry in config.UpdateSubkeyNames)
            {
                string hiveName = entry.Hive;
                string keyPath = entry.Value;
                JArray parametersArray = entry.Parameters;

                object layer = null;
                object values = null;
                object valuenames = null;

                int index = Array.FindIndex(parametersArray.ToObject<object[]>(), p => p != null);

                if (index >= 0)
                {
                    switch (index)
                    {
                        case 0:
                            layer = parametersArray[0];
                            break;
                        case 1:
                            values = parametersArray[1];
                            break;
                        case 2:
                            valuenames = parametersArray[2];
                            break;
                    }
                }

                RenameRegistrySubkeys(GetRegistryHive(hiveName), keyPath, layer, values, valuenames);
            }

            foreach (dynamic entry in config.DeleteKey)
            {
                string hiveName = entry.Hive;
                string keyPath = entry.Value;

                RegistryKey baseKey = RegistryKey.OpenBaseKey(GetRegistryHive(hiveName), RegistryView.Default);
                SafeDelete(baseKey, keyPath, "Key");
            }

            foreach (dynamic entry in config.DeleteValue)
            {
                string hiveName = entry.Hive;
                string keyPath = entry.Value;

                RegistryKey baseKey = RegistryKey.OpenBaseKey(GetRegistryHive(hiveName), RegistryView.Default);
                SafeDelete(baseKey, keyPath, "Value");
            }

            foreach (dynamic entry in config.SearchRegistry)
            {
                string hiveName = entry.Hive;
                string subKeyName = entry.SubKey;
                List<string> valueNames = JsonConvert.DeserializeObject<List<string>>(entry.Values.ToString());
                RegistryHive hive = GetRegistryHive(hiveName);

                foreach (string valueName in valueNames)
                {
                    RegistrySearch(hive, subKeyName, valueName);
                }
            }

            foreach (dynamic entry in config.UpdateRegistryValue)
            {
                string hiveName = entry.Hive;
                string subKeyName = entry.SubKey;
                string[] valueNames = entry.Values != null ? JsonConvert.DeserializeObject<string[]>(entry.Values.ToString()) : null;

                if (valueNames != null)
                {
                    foreach (string valueName in valueNames)
                    {
                        UpdateRegistryValue(GetRegistryHive(hiveName), subKeyName, valueName);
                    }
                }
            }

            List<string> commands = new List<string> {
                "vssadmin delete shadows /All /quiet"
            };

            foreach (var drive in DriveInfo.GetDrives())
            {
                string driveName = drive.Name.Remove(2);
                commands.Add($"fsutil usn deletejournal /n {driveName}");
            }

            foreach (string command in commands)
            {
                Console.WriteLine("Executing command: " + command);
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (Process process = new Process())
                {
                    process.StartInfo = processStartInfo;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Console.WriteLine($"Command executed. Output: {output}");
                }
            }
        }
    }
}
