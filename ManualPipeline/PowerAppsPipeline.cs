using System.Collections;
using System.Text.RegularExpressions;

namespace ManualPipeline;

public abstract partial class PowerAppsPipeline {
    private static readonly Regex re_PowerAppsCLIPath = MyRegex();

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
        if (!GenericFunctions.Shell.AutoClose("winget install Microsoft.PowerAppsCLI")) {
            return "\nPowerAppsCLI did not install correctly.\n\nCheck if winget is installed or has been disabled and check that the path has been added.\nInstall it manually from the Microsoft website here: https://aka.ms/PowerAppsCLI";
        }
        return null;
    }
    
    /* Startup */
    public static Boolean Installed() {
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
            if (re_PowerAppsCLIPath.IsMatch(value)) {
                return true;
            }
        }

        return false;
    }
    public static String? UpdateCLI() {
        if (!GenericFunctions.Shell.AutoClose("pac install latest")) {
            return "\nPowerAppsCLI update failed with an error.";
        }
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
        if (!GenericFunctions.Shell.AutoClose($"pac auth create --environment {envName} --name {envName}")) {
            return $"\nFailed to create an authentication profile for {envName}.\n\nAdd one manually with: pac auth create --environment {envName} --name {envName}.";
        }
        return null;
    }

    
    
    [GeneratedRegex(@".*\\AppData\\Local\\Microsoft\\PowerAppsCLI\\.*", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex MyRegex();
}