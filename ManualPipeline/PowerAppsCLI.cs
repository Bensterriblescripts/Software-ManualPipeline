using System.Text.RegularExpressions;
using GenericFunctions;
namespace ManualPipeline;

public abstract class PowerAppsCLI {
    private static readonly Regex re_PathPowerAppsCLI = new (@".*\\AppData\\Local\\Microsoft\\PowerAppsCLI.*", RegexOptions.IgnoreCase);
    private static readonly Regex re_PathGit = new (@".*\\Git\\cmd.*", RegexOptions.IgnoreCase);

    /* Installation */
    private const String confirmText = "PowerApps CLI is not installed. Install now? (Y/N)\n";
    public static void InstallAll(ref String? error) {
        if (!Cli.ConfirmResponse(confirmText, ConsoleKey.Y, ConsoleKey.N)) {
            error = "Exiting...";
            return;
        }
        Console.WriteLine("\nInstalling the PowerApps CLI...");
        if ((error = InstallCLI()) is not null) {
            return;
        }
        Console.WriteLine("Installed the PowerApps CLI.");
        Console.WriteLine("\nInstallation completed successfully.");
    }
    
    private static String? InstallCLI() {
        Shell.AutoClose("winget install Microsoft.PowerAppsCLI");
        return null;
    }
    
    /* Startup */
    public static Boolean Installed() {
        if (!Env.PathExistsRegex(re_PathPowerAppsCLI)) {
            Console.WriteLine("PowerApps CLI is not installed.");
            return false;
        }
        if (!Env.PathExistsRegex(re_PathGit)) {
            Console.WriteLine("Git is not installed.");
            return false;
        }
        return true;
    }
    public static String? UpdateCLI() {
        Shell.AutoClose("pac install latest");
        return null;
    }
    
    /* Environment */
    private const String envText = "\nEnvironment\nDev: 1) MITOUAT 2) MITO UAT\nProd: 3) idk\n";
    private static readonly ConsoleKey[] envKeys = [ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3]; // 1, 2, 3
    
    public static (String, String) SelectEnvironment() {
        ConsoleKey response = Cli.CheckResponse(envText, envKeys);
        String curEnvName;
        String curEnvGUID;
        
        if (response == ConsoleKey.D1) {
            curEnvName = "MITOUAT";
            curEnvGUID = "9add4700-fdfc-42c0-9369-77a90ca8a9e2";
        }
        else if (response == ConsoleKey.D2) {
            curEnvName = "MITO UAT";
            curEnvGUID = String.Empty;
        }
        else if (response == ConsoleKey.D3) {
            curEnvName = "MYMITONZ";
            curEnvGUID = String.Empty;
        }
        else {
            Console.WriteLine("Error: User input options contain an invalid option.");
            return (String.Empty, String.Empty);
        }
        return (curEnvName, curEnvGUID);
    }
    
    /* Authentication */
    public static String? SelectAuth(String envName, ref String? error) {
        Console.WriteLine($"Using environment: {envName}");
        String? output = Shell.GetOutput($"pac auth select --name {envName}");
        if (output == null) {
            return "Unexpected error when retrieving output, command returned no output.\nExiting...";
        }
        if (output.Contains(envName)) {
            return null;
        }
        Console.WriteLine("\nFailed to select an authentication profile for this environment.\nCreating one...");
        return CreateAuth(envName);
    }
    private static String? CreateAuth(String envName) {
        Shell.AutoClose($"pac auth create --environment {envName} --name {envName}");
        return null;
    }
}