using Microsoft.Win32;
using Newtonsoft.Json;
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

            string[] commands = {
            "fsutil usn deletejournal /n c:",
            "fsutil usn deletejournal /n D:",
            "fsutil usn deletejournal /n E:",
            "fsutil usn deletejournal /n F:",
            "vssadmin delete shadows /All"
        };

            foreach (string command in commands)
            {
                Console.WriteLine("Executing command: " + command);
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                Console.WriteLine("Command executed.");
            }

            Console.WriteLine("All commands executed. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
