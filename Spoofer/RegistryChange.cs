using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static Functions;

namespace SpooferApp
{
    public class RegistryChange
    {
        /// <summary>
        /// Reads the Keys/Values from Config.json and deletes/edits/... them.
        /// </summary>
        public static void SpoofGUIDAddresses()
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
        }
    }
}
