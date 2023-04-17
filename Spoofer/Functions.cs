using Microsoft.Win32;
using SpooferApp;
using System.Xml.Linq;

public class Functions
{
    /// <summary>
    /// Updates Registry Value, optionally takes newValue; if not defined, it'll go to IncrementRegistryValue.
    /// </summary>
    public static void UpdateRegistryValue(RegistryHive hive, string subkey, string valueName, object newValue = null)
    {
        if (string.IsNullOrEmpty(subkey))
        {
            throw new ArgumentException("Subkey cannot be null or empty.", nameof(subkey));
        }
        if (string.IsNullOrEmpty(valueName))
        {
            throw new ArgumentException("Value name cannot be null or empty.", nameof(valueName));
        }

        using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
        using (RegistryKey key = baseKey.OpenSubKey(subkey, true))
        {
            if (key == null)
            {
                // Key does not exist
                return;
            }

            object value = key.GetValue(valueName);
            if (value == null)
            {
                // Value does not exist
                return;
            }

            // Automatically read RegistryValueKind
            RegistryValueKind valueKind;
            try
            {
                valueKind = key.GetValueKind(valueName);
            }
            catch (IOException)
            {
                // Unable to determine value kind
                return;
            }

            if (newValue == null)
            {
                switch (valueKind)
                {
                    case RegistryValueKind.String:
                        IncrementRegistryValue(key, valueName, (string)value, valueKind);
                        break;
                    case RegistryValueKind.Binary:
                        IncrementRegistryValue(key, valueName, (byte[])value, valueKind);
                        break;
                    case RegistryValueKind.DWord:
                        IncrementRegistryValue(key, valueName, (int)value, valueKind);
                        break;
                    case RegistryValueKind.QWord:
                        IncrementRegistryValue(key, valueName, (long)value, valueKind);
                        break;
                    case RegistryValueKind.MultiString:
                        IncrementRegistryValue(key, valueName, (string[])value, valueKind);
                        break;
                    case RegistryValueKind.ExpandString:
                        IncrementRegistryValue(key, valueName, (string)value, valueKind);
                        break;
                    case RegistryValueKind.Unknown:
                    default:
                        throw new NotSupportedException(
                            $"The registry value kind '{valueKind}' is not supported."
                        );
                }
            }
            else
            {
                string oldValue = value.ToString();

                switch (valueKind)
                {
                    case RegistryValueKind.String:
                    case RegistryValueKind.ExpandString:
                        newValue = newValue.ToString();
                        key.SetValue(valueName, newValue, valueKind);
                        DebugPrint(key, valueName, oldValue, newValue.ToString());
                        break;
                    case RegistryValueKind.Binary:
                        oldValue = BitConverter.ToString((byte[])value).Replace("-", " ");
                        key.SetValue(valueName, (byte[])newValue, valueKind);
                        string newValueString = BitConverter.ToString((byte[])newValue).Replace("-", " ");
                        DebugPrint(key, valueName, oldValue, newValueString);
                        break;
                    case RegistryValueKind.DWord:
                        key.SetValue(valueName, Convert.ToInt32(newValue), valueKind);
                        DebugPrint(key, valueName, oldValue, newValue.ToString());
                        break;
                    case RegistryValueKind.QWord:
                        key.SetValue(valueName, Convert.ToInt64(newValue), valueKind);
                        DebugPrint(key, valueName, oldValue, newValue.ToString());
                        break;
                    case RegistryValueKind.MultiString:
                        oldValue = string.Join(", ", (string[])value);
                        key.SetValue(valueName, (string[])newValue, valueKind);
                        DebugPrint(key, valueName, oldValue, string.Join(", ", (string[])newValue));
                        break;
                    case RegistryValueKind.Unknown:
                    default:
                        throw new NotSupportedException(
                            $"The registry value kind '{valueKind}' is not supported."
                        );
                }
            }
        }
    }

    /// <summary>
    /// Increments a Registry Value by its position times 2. Sends it to it's own Function for each RegistryValueKind for better readability.
    /// </summary>
    public static void IncrementRegistryValue(
        RegistryKey key,
        string valueName,
        object value,
        RegistryValueKind valueKind
    )
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (valueName == null)
        {
            throw new ArgumentNullException(nameof(valueName));
        }

        if (!key.GetValueNames().Contains(valueName))
        {
            throw new ArgumentException($"The value \"{valueName}\" does not exist in the registry key.", nameof(valueName));
        }

        object oldValue = key.GetValue(valueName);
        object newValue;

        switch (valueKind)
        {
            case RegistryValueKind.String:
            case RegistryValueKind.ExpandString:
                newValue = IncrementStringValue((string)value);
                break;
            case RegistryValueKind.Binary:
                newValue = IncrementBinaryValue((byte[])value);
                break;
            case RegistryValueKind.DWord:
                newValue = IncrementIntValue((int)value, valueName);
                break;
            case RegistryValueKind.QWord:
                newValue = IncrementLongValue((long)value, valueName);
                break;
            case RegistryValueKind.MultiString:
                newValue = IncrementMultiStringValue((string[])value);
                break;
            case RegistryValueKind.None:
                // Do nothing, as there is no specific value type for this case.
                return;
            default:
                throw new ArgumentException($"Invalid value kind: {nameof(valueKind)}");
        }

        key.SetValue(valueName, newValue, valueKind);
        DebugPrint(key, valueName, oldValue, newValue);
    }

    private static string IncrementStringValue(string value)
    {
        char[] chars = value.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (Char.IsLetter(chars[i]))
            {
                int diff = Char.IsUpper(chars[i]) ? 'A' : 'a';
                int charValue = chars[i] - diff;
                charValue += ((i + 1) * 2);
                if (charValue > 25)
                {
                    charValue = charValue % 26;
                }
                chars[i] = (char)(diff + charValue);
            }
            else if (Char.IsDigit(chars[i]))
            {
                int numericValue = chars[i] - '0';
                numericValue += ((i + 1) * 2);
                if (numericValue > 9)
                {
                    numericValue = numericValue % 10;
                }
                chars[i] = (char)('0' + numericValue);
            }
        }
        return new string(chars);
    }

    private static byte[] IncrementBinaryValue(byte[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = (byte)((value[i] + i * 2) % 256);
        }
        return value;
    }

    private static int IncrementIntValue(int value, string valueName)
    {
        value += ((int)(Char.ToLower(valueName[valueName.Length - 1])) - '0' + 1) * 2;
        return value;
    }

    private static long IncrementLongValue(long value, string valueName)
    {
        value += ((int)(Char.ToLower(valueName[valueName.Length - 1])) - '0' + 1) * 2;
        return value;
    }

    private static string[] IncrementMultiStringValue(string[] value)
    {
        for (int j = 0; j < value.Length; j++)
        {
            value[j] = IncrementStringValue(value[j]);
        }
        return value;
    }

    /// <summary>
    /// Isn't really needed as the Console.WriteLine can still be done if the Console doesn't currently exist, but it's nice to have.
    /// </summary>
    private static void DebugPrint(RegistryKey key, string valueName, object oldValue, object newValue, object output = null)
    {
        if (!GlobalSettings.DebugOutput) return;

        string oldStrValue, newStrValue;

        switch (oldValue)
        {
            case byte[] oldValueBytes:
                oldStrValue = BitConverter.ToString(oldValueBytes).Replace("-", " ");
                newStrValue = BitConverter.ToString((byte[])newValue).Replace("-", " ");
                break;
            case string[] oldValueStrings:
                oldStrValue = string.Join(", ", oldValueStrings);
                newStrValue = string.Join(", ", (string[])newValue);
                break;
            default:
                oldStrValue = oldValue.ToString();
                newStrValue = newValue.ToString();
                break;
        }

        Console.WriteLine($"\nKey: {key.Name}\nValue Name: {valueName}\nOld Value: {oldStrValue}\nNew Value: {newStrValue}\n");

        if (output != null)
        {
            Console.WriteLine($"Output:\n{output}\n");
        }
    }

    /// <summary>
    /// Takes a Registry Key & Value as input, then searches the key itself and ever sub(sub)key for that Value.
    /// Only stops after it has found every single value with the name.
    /// </summary>
    public static void RegistrySearch(RegistryHive hive, string subkey, string valueName, object newValue = null)
    {
        RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
        RegistryKey key = null;
        try
        {
            key = baseKey.OpenSubKey(subkey);
            if (key == null)
            {
                // Key does not exist
                return;
            }

            // Check if the value exists in the current key
            object keyValue = key.GetValue(
                valueName,
                null,
                RegistryValueOptions.DoNotExpandEnvironmentNames
            );
            if (keyValue != null)
            {
                if (newValue == null)
                {
                    // Value found, update it
                    UpdateRegistryValue(hive, subkey, valueName);
                    // MessageBox.Show("Value found & updated in registry: " + subkey);
                }
                else
                {
                    UpdateRegistryValue(hive, subkey, valueName, newValue);
                }
            }

            // Recursively search in all subkeys
            if (newValue == null)
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    RegistrySearch(hive, subkey + "\\" + subkeyName, valueName);
                }
            }
            else
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    RegistrySearch(hive, subkey + "\\" + subkeyName, valueName, newValue);
                }

            }
        }
        catch (Exception)
        {
            // Handle the exception by skipping this subkey and continuing with the next one
            return;
        }
    }

    /// <summary>
    /// Renames every subkey
    /// </summary>
    public static void RenameRegistrySubkeys(RegistryHive hive, string oldParentKey, object layer = null, object values = null, object valuenames = null)
    {
        RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
        RegistryKey parentKey = baseKey.OpenSubKey(oldParentKey, true);
        if (parentKey == null)
        {
            Console.WriteLine("Error: Unable to open parent key.");
            return;
        }
        string[] subKeyNames = parentKey.GetSubKeyNames();

        foreach (string subKeyName in subKeyNames)
        {
            RegistryKey subKey = null;

            try
            {
                subKey = parentKey.OpenSubKey(subKeyName, true);
                if (subKey == null)
                {
                    Console.WriteLine($"Error: Unable to open subkey '{subKeyName}'.");
                    continue;
                }
                string newSubKeyName = IncrementStringValue(subKeyName);
                RegistryKey newSubKey = parentKey.CreateSubKey(newSubKeyName);
                if (newSubKey == null)
                {
                    Console.WriteLine($"Error: Unable to create subkey '{newSubKeyName}'.");
                    subKey.Close();
                    continue;
                }

                foreach (string valueName in subKey.GetValueNames())
                {
                    if (values != null)
                    {
                        Console.WriteLine($"Value Name: {valueName}");
                    }
                    object value = subKey.GetValue(valueName);
                    if (valuenames != null) { newSubKey.SetValue(IncrementStringValue(valueName), value); }
                    else { newSubKey.SetValue(valueName, value); }
                }

                if (layer == null)
                {
                    // only rename the first layer of subkeys
                    foreach (string subSubKeyName in subKey.GetSubKeyNames())
                    {
                        RegistryKey subSubKey = null;
                        RegistryKey newSubSubKey = null;

                        try
                        {
                            subSubKey = subKey.OpenSubKey(subSubKeyName);
                            if (subSubKey == null)
                            {
                                Console.WriteLine($"Error: Unable to open subkey '{subSubKeyName}'.");
                                continue;
                            }
                            newSubSubKey = newSubKey.CreateSubKey(subSubKeyName);
                            if (newSubSubKey == null)
                            {
                                Console.WriteLine($"Error: Unable to create subkey '{subSubKeyName}'.");
                                subSubKey.Close();
                                continue;
                            }

                            foreach (string valueName in subSubKey.GetValueNames())
                            {
                                if (values != null)
                                {
                                    Console.WriteLine($"Value Name: {valueName}");
                                }
                                object value = subSubKey.GetValue(valueName);
                                if (valuenames != null) { newSubSubKey.SetValue(IncrementStringValue(valueName), value); }
                                else { newSubSubKey.SetValue(valueName, value); }
                            }

                            newSubSubKey.Close();
                            subSubKey.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while renaming subkey '{subSubKeyName}': {ex.Message}");
                        }
                    }
                }
                else
                {
                    // recursively rename all subkeys and values
                    RenameRegistrySubkeysRecursive(subKey, newSubKey, values, valuenames);
                }

                subKey.Close();
                newSubKey.Close();
                try
                {
                    if (parentKey.SubKeyCount > 0 && parentKey.OpenSubKey(subKeyName) != null)
                    {
                        parentKey.DeleteSubKeyTree(subKeyName);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
                Console.WriteLine($"\nRenamed: {parentKey}\nFrom: {subKeyName}\nTo: {newSubKeyName}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while renaming subkey '{subKeyName}': {ex.Message}");
            }
            finally
            {
                if (subKey != null)
                {
                    subKey.Close();
                }
            }
        }
    }

    private static void RenameRegistrySubkeysRecursive(RegistryKey sourceKey, RegistryKey destKey, object values = null, object valuenames = null)
    {
        try
        {
            foreach (string subKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = null;
                RegistryKey destSubKey = null;

                try
                {
                    sourceSubKey = sourceKey.OpenSubKey(subKeyName);
                    string newSubKeyName = IncrementStringValue(subKeyName);
                    destSubKey = destKey.CreateSubKey(newSubKeyName);

                    foreach (string valueName in sourceSubKey.GetValueNames())
                    {
                        if (values != null)
                        {
                            Console.WriteLine($"Value Name: {valueName}");
                        }
                        object value = sourceSubKey.GetValue(valueName);
                        if (valuenames != null) { destSubKey.SetValue(IncrementStringValue(valueName), value); }
                        else { destSubKey.SetValue(valueName, value); }
                    }

                    RenameRegistrySubkeysRecursive(sourceSubKey, destSubKey, values);

                    if (sourceKey.GetSubKeyNames().Contains(subKeyName))
                    {
                        try {
                        sourceKey.DeleteSubKeyTree(subKeyName);
                        }
                        catch (Exception ex) { Console.WriteLine(ex); }
                    }
                    else
                    {
                        Console.WriteLine($"Subkey '{subKeyName}' does not exist, skipping deletion.");
                    }

                    Console.WriteLine($"\nRenamed: {sourceKey}\nFrom: {subKeyName}\nTo: {newSubKeyName}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while renaming subkey: {subKeyName}");
                    Console.WriteLine($"Error message: {ex.Message}");
                }
                finally
                {
                    if (sourceSubKey != null)
                    {
                        sourceSubKey.Close();
                    }
                    if (destSubKey != null)
                    {
                        destSubKey.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting subkeys: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Registry Value or Key so RegistryChange.cs doesn't have to always be in a try. Better readability.
    /// </summary>
    public static void SafeDelete(RegistryKey key, string name, string type)
    {
        if (type.ToLower() == "value")
        {
            try
            {
                key.DeleteValue(name);
                Console.WriteLine($"\nDeleted Registry Value: {name}\n");
            }
            catch (ArgumentException)
            {
                // Value not found, skip
            }
        }
        if (type.ToLower() == "key")
        {
            try
            {
                key.DeleteSubKeyTree(name);
                Console.WriteLine($"\nDeleted Registry Subkey Tree: {name}\n");
            }
            catch (ArgumentException)
            {
                // Value not found, skip
            }
        }
    }

    private static readonly Random _random = new Random();

    public static string RandomString(
        int length,
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive.");
        }

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public static string RandomNumber(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive.");
        }

        int maxValue = (int)Math.Pow(10, length);
        int randomNumber = _random.Next(maxValue / 10, maxValue);

        return randomNumber.ToString();
    }

    public static RegistryHive GetRegistryHive(string hiveName)
    {
        if (hiveName.EndsWith("ClassesRoot", StringComparison.OrdinalIgnoreCase))
        {
            return RegistryHive.ClassesRoot;
        }
        else if (hiveName.EndsWith("CurrentConfig", StringComparison.OrdinalIgnoreCase))
        {
            return RegistryHive.CurrentConfig;
        }
        else if (hiveName.EndsWith("CurrentUser", StringComparison.OrdinalIgnoreCase))
        {
            return RegistryHive.CurrentUser;
        }
        else if (hiveName.EndsWith("LocalMachine", StringComparison.OrdinalIgnoreCase))
        {
            return RegistryHive.LocalMachine;
        }
        else if (hiveName.EndsWith("Users", StringComparison.OrdinalIgnoreCase))
        {
            return RegistryHive.Users;
        }
        else
        {
            throw new ArgumentException("Invalid registry hive name: " + hiveName);
        }
    }
}
