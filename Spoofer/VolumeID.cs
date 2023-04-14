using System.Diagnostics;
using System.IO;

namespace SpooferApp
{
    internal static class VolumeID
    {
        /// <summary>
        /// Spoofs with VolumeID (limited effect).
        /// </summary>
        public static void SpoofVolumeID()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "Volumeid.exe",
                        Arguments = $"{drive.Name} {string.Join("-", Enumerable.Range(0, 2).Select(i => Guid.NewGuid().ToString().Substring(0, 4).ToUpper()))}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    }
                };

                process.Start();
                Console.WriteLine(process.StandardOutput.ReadToEnd());


                if (process.ExitCode != 0)
                {
                    System.Console.Error.WriteLine($"Error spoofing volume ID for {drive.Name}: Volumeid.exe exited with error code {process.ExitCode}.");
                }
            }
        }
    }
}
