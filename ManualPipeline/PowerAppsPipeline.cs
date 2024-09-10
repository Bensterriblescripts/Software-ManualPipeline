using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
namespace ManualPipeline;

public abstract class PowerAppsPipeline() {
    private static readonly Regex re_PowerAppsCLIPath = new(@".*\\AppData\\Local\\Microsoft\\PowerAppsCLI\\.*", RegexOptions.IgnoreCase);

    /* Installation */
    public static void InstallAll(ref String? error) {
        if (!Generic.CheckResponseBool("PowerApps CLI is not installed. Install now? (y/n)")) {
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
        if (!Generic.CMDCommandAutoClose("winget install Microsoft.PowerAppsCLI")) {
            return "\nPowerAppsCLI did not install correctly.\n\nCheck if winget is installed or has been disabled and check that the path has been added.\nInstall it manually from the Microsoft website here: https://aka.ms/PowerAppsCLI";
        }
        return null;
    }
    private static String? InstallPRT() {
        if (!Generic.CMDCommandKeepOpen("pac tool prt")) {
            return "\nPowerApps Plugin Registration Tool did not install correctly.\n\nInstall it manually by running pac tool prt.";
        }
        return null;
    }
    private static String? CreateAuth(String envName) {
        if (!Generic.CMDCommandKeepOpen($"pac auth create --environment {envName} --name {envName}")) {
            return $"\nFailed to create an authentication profile for {envName}.\n\nAdd one manually with: pac auth create --environment {envName} --name {envName}.";
        }
        return null;
    }
    
    /* Startup */
    public static Boolean Installed() {
        IDictionary envs = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in envs) {
            String? key = entry.Key?.ToString();
            String? value = entry.Value?.ToString();
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) {
                continue;
            }
            if (key.Contains("PATH", StringComparison.OrdinalIgnoreCase)) {
                if (re_PowerAppsCLIPath.IsMatch(value)) {
                    return true;
                }
            }
        }

        return false;
    }
    public static String? UpdateCLI() {
        if (!Generic.CMDCommandAutoClose("pac install latest")) {
            return "\nPowerAppsCLI update failed with an error.";
        }
        return null;
    }
    
    /* Connecting */
    public static String SelectEnvironment() {
        Console.WriteLine("\nEnvironment\nDev: 1) MITOUAT 2) MITO UAT\nProd: 3) idk");
        Int16 response = Generic.CheckResponseNumbered(ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3);
        String? currentEnvironment = String.Empty;
        if (response == 1) {
            currentEnvironment = "MITOUAT";
        }
        else if (response == 2) {
            currentEnvironment = "MITO UAT";
        }
        else if (response == 3) {
            currentEnvironment = "MYMITONZ";
        }
        return currentEnvironment;
    }
    public static String? SelectAuth(String envName) {
        if (!Generic.CMDCommandReturnOutput($"pac auth select --name {envName}")) {
            return "\nFailed to select an authentication profile for this environment.\nCreating one...";
        }
        return null;
    }
}