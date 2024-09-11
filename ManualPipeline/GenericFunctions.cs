using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace ManualPipeline;

public static class GenericFunctions {
    
    /* Command Line Tools */
    public static class Cli {
        
        private const ConsoleKey answerYes = ConsoleKey.Y;
        private const ConsoleKey answerNo = ConsoleKey.N;

        // Read Input
        public static Boolean ConfirmResponse(String info, ConsoleKey confirm, ConsoleKey deny) {
            Console.Write(info);
            ConsoleKey keyInfo = Console.ReadKey(true).Key;
            while (true) {
                if (confirm == keyInfo) {
                    return true;
                }
                if (deny == keyInfo) {
                    return false;
                }
                Console.WriteLine("Please enter a valid response.");
                keyInfo = Console.ReadKey(true).Key;
            }
        }
        public static ConsoleKey CheckResponse(String info, ConsoleKey[] responses) {
            Console.Write(info);
            ConsoleKey keyInfo = Console.ReadKey(true).Key;
            while (true) {
                if (responses.Contains(keyInfo)) {
                    return keyInfo;
                }
                Console.WriteLine("Please enter a valid response.");
                keyInfo = Console.ReadKey(true).Key;
            }
        }
    }

    /* CMD, PowerShell and Bash Processes */
    public static class Shell {
        
        // CMD
        
        /// <summary>
        /// Execute a CMD command, the window will close on completion.
        /// </summary>
        /// <param name="command">String: CMD Command</param>
        public static void AutoClose(String command) {
            using Process p = new();
            ProcessStartInfo psi = new() {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = false,
                UseShellExecute = true,
                CreateNoWindow = false
            };
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
        /// <summary>
        /// Execute a CMD command and return the output
        /// </summary>
        /// <param name="command">String: CMD Command</param>
        /// <returns>String: output | null: empty</returns>
        public static String? GetOutput(String command) {
            using Process p = new();
            ProcessStartInfo psi = new() {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            p.StartInfo = psi;
            p.Start();
            String output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (String.IsNullOrEmpty(output)) {
                return null;
            }
            return output;
        }
        /// <summary>
        /// Execute a CMD command and remain open until closed manually.
        /// <para>Synchronous code execution will be halted until the window is closed.</para>
        /// </summary>
        /// <param name="command">String: CMD Command</param>
        public static void KeepOpen(String command) {
            using Process p = new();
            ProcessStartInfo psi = new() {
                FileName = "cmd.exe",
                Arguments = $"/K {command}",
                RedirectStandardOutput = false,
                UseShellExecute = true,
                CreateNoWindow = false
            };
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
        }
    }
    
    /* Environment Variables */
    public static class Env {

        /// <summary>
        /// Checks system and user paths if it exists via regular expression
        /// </summary>
        /// <param name="path">String: Path to check</param>
        /// <param name="exact"> Boolean: Defaults to false, set to true to use regular expression matching.</param>
        /// <returns>Boolean</returns>
        public static Boolean PathExists(String path, Boolean exact = false) {
            IDictionary envs = Environment.GetEnvironmentVariables();
            if (exact) {
                foreach (DictionaryEntry entry in envs) {
                    String? key = entry.Key.ToString();
                    String? value = entry.Value?.ToString();
                    if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) {
                        continue;
                    }

                    if (!key.Contains("PATH", StringComparison.OrdinalIgnoreCase)) {
                        continue;
                    }

                    if (path == value) {
                        return true;
                    }
                }
            }
            else {
                Regex re = new($@"{path}", RegexOptions.IgnoreCase);
                foreach (DictionaryEntry entry in envs) {
                    String? key = entry.Key.ToString();
                    String? value = entry.Value?.ToString();
                    if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) {
                        continue;
                    }

                    if (!key.Contains("PATH", StringComparison.OrdinalIgnoreCase)) {
                        continue;
                    }

                    if (re.IsMatch(value)) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks system and user paths for the regex match
        /// </summary>
        /// <param name="re">Regex: Path to check</param>
        /// <returns>Boolean</returns>
        public static Boolean PathExistsRegex(Regex re) {
            IDictionary envs = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry entry in envs) {
                String? key = entry.Key.ToString();
                String? value = entry.Value?.ToString();
                if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) {
                    continue;
                }

                if (!key.Contains("PATH", StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }

                if (re.IsMatch(value)) {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Retrieve the full path of a String or Regex
        /// </summary>
        /// <param name="path">String: Path to check</param>
        /// <param name="reg"> Boolean: Defaults to false, set to true to use regular expression matching.</param>
        /// <returns></returns>
        public static String? GetPath(String path, Boolean reg = false) {
            IDictionary envs = Environment.GetEnvironmentVariables();
            if (!reg) {
                foreach (DictionaryEntry entry in envs) {
                    String? key = entry.Key.ToString();
                    String? value = entry.Value?.ToString();
                    if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) {
                        continue;
                    }
                    if (!key.Contains("PATH", StringComparison.OrdinalIgnoreCase)) {
                        continue;
                    }

                    if (path == value) {
                        return value;
                    }
                }
            }
            else {
                Regex re = new($@"{path}", RegexOptions.IgnoreCase);
                foreach (DictionaryEntry entry in envs) {
                    String? key = entry.Key.ToString();
                    String? value = entry.Value?.ToString();
                    if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) {
                        continue;
                    }

                    if (!key.Contains("PATH", StringComparison.OrdinalIgnoreCase)) {
                        continue;
                    }

                    if (re.IsMatch(value)) {
                        return value;
                    }
                }
            }
            return null;
        }
    }

    /* File and Console Logging */
    public static class Log {
        
        public static void Info(String message) {
            String? file = FileHandler();

        }
        public static void Debug(String message) {
            
        }
        public static void Error(String message) {
            
        }
        
        private static String? FileHandler() {
            String currentTime = DateTime.Now.ToString("yyyyMMdd-HHmm");
            Console.WriteLine(currentTime);
            
            // File.Exists
            
            return null;
        }
    }
}