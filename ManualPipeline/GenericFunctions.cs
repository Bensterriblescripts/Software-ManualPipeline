using System.Diagnostics;
namespace ManualPipeline;

public static class GenericFunctions {
    
    // Command Line Tools
    public static class Cli {
        
        private const ConsoleKey answerYes = ConsoleKey.Y;
        private const ConsoleKey answerNo = ConsoleKey.N;

        /* Read Input */
        public static Boolean ConfirmResponse(String info, ConsoleKey[] confirm) {
            Console.Write(info);
            ConsoleKey keyInfo = Console.ReadKey(true).Key;
            while (true) {
                if (confirm[0] == keyInfo) {
                    return true;
                }
                if (confirm[1] == keyInfo) {
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

    // CMD, PowerShell and Bash Processes
    public static class Shell {
        
        /* CMD */
        public static Boolean AutoClose(String command) {
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

            return true;
        }
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

            if (output.Trim() == String.Empty) {
                return null;
            }
            return output.Trim();
        }
        public static Boolean KeepOpen(String command) {
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

            return true;
        }
    }

    public static class Log {
        
        public static void Info(String message) {
            String? file = fileHandler();

        }
        public static void Debug(String message) {
            
        }
        public static void Error(String message) {
            
        }
        
        private static String? fileHandler() {
            String currentTime = DateTime.Now.ToString("yyyyMMdd-HHmm");
            Console.WriteLine(currentTime);
            
            // File.Exists
            
            return null;
        }
        
    }
}