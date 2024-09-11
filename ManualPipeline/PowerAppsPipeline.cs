using System.Text.RegularExpressions;
namespace ManualPipeline;

public abstract partial class PowerAppsPipeline {
    private static readonly Regex re_PathPowerAppsCLI = new (@".*\\AppData\\Local\\Microsoft\\PowerAppsCLI.*", RegexOptions.IgnoreCase);
    private static readonly Regex re_PathGit = new (@".*\\Git\\cmd.*", RegexOptions.IgnoreCase);

    /* Installation */
    private const String confirmText = "PowerApps CLI is not installed. Install now? (y/n)\n";
    private static readonly ConsoleKey[] confirmKeys = [ConsoleKey.Y, ConsoleKey.N]; // y, n
    public static void InstallAll(ref String? error) {
        if (!GenericFunctions.Cli.ConfirmResponse(confirmText, confirmKeys)) {
            error = "Exiting...";
            return;
        }
        
        // CLI
        Console.WriteLine("\nInstalling the PowerApps CLI...");
        if ((error = InstallCLI()) is not null) {
            return;
        }
        Console.WriteLine("Installed the PowerApps CLI.");
        Console.WriteLine("\nInstallation completed successfully.");
    }
    
    private static String? InstallCLI() {
        GenericFunctions.Shell.AutoClose("winget install Microsoft.PowerAppsCLI");
        return null;
    }
    
    /* Startup */
    public static Boolean Installed() {
        if (!GenericFunctions.Env.PathExistsRegex(re_PathPowerAppsCLI)) {
            Console.WriteLine("PowerApps CLI is not installed.");
            return false;
        }
        if (!GenericFunctions.Env.PathExistsRegex(re_PathGit)) {
            Console.WriteLine("Git is not installed.");
            return false;
        }
        return true;
    }
    public static String? UpdateCLI() {
        GenericFunctions.Shell.AutoClose("pac install latest");
        return null;
    }
    
    /* Environment */
    private const String envText = "\nEnvironment\nDev: 1) MITOUAT 2) MITO UAT\nProd: 3) idk\n";
    private static readonly ConsoleKey[] envKeys = [ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3]; // 1, 2, 3
    public static String SelectEnvironment() {
        ConsoleKey response = GenericFunctions.Cli.CheckResponse(envText, envKeys);
        String currentEnvironment = String.Empty;
        if (response == ConsoleKey.D1) {
            currentEnvironment = "MITOUAT";
        }
        else if (response == ConsoleKey.D2) {
            currentEnvironment = "MITO UAT";
        }
        else if (response == ConsoleKey.D3) {
            currentEnvironment = "MYMITONZ";
        }
        else {
            Console.WriteLine("No valid response selected.");
        }
        return currentEnvironment;
    }
    
    /* Authentication */
    public static String? SelectAuth(String envName, ref String? error) {
        Console.WriteLine($"Using environment: {envName}");
        String? output = GenericFunctions.Shell.GetOutput($"pac auth select --name {envName}");
        if (String.IsNullOrEmpty(output)) {
            return "Unexpected error when retrieving output, command returned no output.\nExiting...";
        }
        Console.WriteLine("\nFailed to select an authentication profile for this environment.\nCreating one...");
        return CreateAuth(envName);
    }
    private static String? CreateAuth(String envName) {
        GenericFunctions.Shell.AutoClose($"pac auth create --environment {envName} --name {envName}");
        return null;
    }
}