using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
namespace ManualPipeline;

public static class Generic {
    
    private const ConsoleKey answerYes = ConsoleKey.Y;
    private const ConsoleKey answerNo = ConsoleKey.N;
    private const ConsoleKey answer1 = ConsoleKey.D1;
    private const ConsoleKey answer2 = ConsoleKey.D2;
    private const ConsoleKey answer3 = ConsoleKey.D3;
    private const ConsoleKey answer4 = ConsoleKey.D4;

    /* Strings */
    public static Boolean CheckResponseBool(String info) {
        Console.WriteLine(info);
        ConsoleKey keyInfo = Console.ReadKey(true).Key;
        if (keyInfo == answerYes) {
            return true;
        }
        if (keyInfo == answerNo) {
            return false;
        }
        while (true) {
            Console.WriteLine("Please enter a valid response. (y/n)");
            keyInfo = Console.ReadKey(true).Key;
            if (keyInfo == answerYes) {
                return true;
            }
            if (keyInfo == answerNo) {
                return false;
            }
        }
    }
    public static Int16 CheckResponseNumbered(ConsoleKey option1, ConsoleKey option2, ConsoleKey option3) {
        ConsoleKey keyInfo = Console.ReadKey(true).Key;
        while (true) {
            if (keyInfo == answer1) {
                return 1;
            }
            if (keyInfo == answer2) {
                return 2;
            }
            if (keyInfo == answer3) {
                return 3;
            }
            if (keyInfo == answer4) {
                return 4;
            }
            Console.WriteLine("Please enter a valid response. (1, 2, 3, 4)");
            keyInfo = Console.ReadKey(true).Key;
        }
    }

    /* CLI */
    public static Boolean CMDCommandAutoClose(String command) {
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
    public static String CMDCommandReturnOutput(String command) {
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
        String output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        
        return output.Trim();
    }
    public static Boolean CMDCommandKeepOpen(String command) {
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