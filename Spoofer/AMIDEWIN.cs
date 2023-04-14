using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Functions;

namespace SpooferApp
{
    public class AMIDEWIN
    {
        /// <summary>
        /// Creates all the comments for AMIDEWIN, then sends it to RunAMIDEWIN so the commands get run.
        /// </summary>
        public static void SpoofAMIDEWIN()
        {
            var spoofCommands = new Dictionary<string, string>
            {
                { "/IVN", RandomString(16) },
                { "/IV", $"{RandomNumber(2)}.{RandomNumber(2)}" },
                { "/ID", $"{RandomNumber(2)}/{RandomNumber(2)}/{RandomNumber(4)}" },
                { "/SM", RandomString(16) },
                { "/SP", RandomString(16) },
                { "/SV", $"{RandomNumber(2)}.{RandomNumber(2)}" },
                { "/SS", RandomString(16) },
                { "/SU", "auto" },
                { "/SK", RandomString(16) },
                { "/SF", RandomString(16) },
                { "/BM", RandomString(16) },
                { "/BP", RandomString(16) },
                { "/BV", $"{RandomNumber(2)}.{RandomNumber(2)}" },
                { "/BS", $"{RandomString(8)}_{RandomString(8)}" },
                { "/BT", RandomString(16) },
                { "/BLC", RandomString(16) },
                { "/CM", RandomString(16) },
                { "/CT", RandomString(3, "0123456789") },
                { "/CV", $"{RandomNumber(2)}.{RandomNumber(2)}" },
                { "/CS", RandomString(16) },
                { "/CA", RandomString(16) },
                { "/CO", RandomString(16) },
                { "/CH", RandomString(16) },
                { "/CPC", RandomString(16) },
                { "/CSK", RandomString(16) },
                { "/PSN", RandomString(16) },
                { "/PAT", RandomString(16) },
                { "/PPN", RandomString(16) }
            };
            var oldValues = new Dictionary<string, string>();

            // First, run the commands without the values to get the old values
            foreach (var command in spoofCommands.Keys)
            {
                string oldValue = RunAMIDEWIN(command, null);
                oldValues[command] = oldValue;
            }

            // Then, run the commands with the new values
            foreach (var (command, value) in spoofCommands)
            {
                RunAMIDEWIN(command, value, oldValues[command]);
            }
        }

        private static string RunAMIDEWIN(string command, string value, string oldValue = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "AMIDEWINx64.EXE",
                    Arguments = value != null ? $"{command} {value}" : command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                Regex regex = new Regex(@"Done\s+""([^""]+)""");
                Match match = regex.Match(output);
                string currentValue = match.Groups[1].Value;

                if (GlobalSettings.DebugOutput)
                {
                    if (value != null && oldValue != null)
                    {
                        Console.WriteLine($"Command: {command}\nOld Value: {oldValue}\nNew Value: {value}\nOutput:\n{output}");
                    }
                }

                return currentValue;
            }
        }
    }
}