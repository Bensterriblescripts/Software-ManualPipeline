﻿using System.Collections;
using System.Text.RegularExpressions;
using GenericFunctions;

namespace ManualPipeline;

public abstract partial class PowerAppsPipeline {
    private static readonly Regex re_PowerAppsCLIPath = MyRegex();

    /* Installation */
    public static void InstallAll(ref String? error) {
        if (!Cli.CheckResponse_YN("PowerApps CLI is not installed. Install now? (y/n)")) {
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
        if (!Shell.AutoClose("winget install Microsoft.PowerAppsCLI")) {
            return "\nPowerAppsCLI did not install correctly.\n\nCheck if winget is installed or has been disabled and check that the path has been added.\nInstall it manually from the Microsoft website here: https://aka.ms/PowerAppsCLI";
        }
        return null;
    }
    // private static String? InstallPRT() {
    //     if (!Shell.KeepOpen("pac tool prt")) {
    //         return "\nPowerApps Plugin Registration Tool did not install correctly.\n\nInstall it manually by running pac tool prt.";
    //     }
    //     return null;
    // }
    // private static String? CreateAuth(String envName) {
    //     if (!Shell.KeepOpen($"pac auth create --environment {envName} --name {envName}")) {
    //         return $"\nFailed to create an authentication profile for {envName}.\n\nAdd one manually with: pac auth create --environment {envName} --name {envName}.";
    //     }
    //     return null;
    // }
    
    /* Startup */
    public static Boolean Installed() {
        IDictionary envs = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in envs) {
            String? key = entry.Key.ToString();
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
        if (!Shell.AutoClose("pac install latest")) {
            return "\nPowerAppsCLI update failed with an error.";
        }
        return null;
    }
    
    /* Connecting */
    public static String SelectEnvironment() {
        Console.WriteLine("\nEnvironment\nDev: 1) MITOUAT 2) MITO UAT\nProd: 3) idk");
        Int16 response = Cli.CheckResponse_Numbers(new []{ ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4 });
        String currentEnvironment = String.Empty;
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
    public static String SelectAuth(String envName) {
        String? output = Shell.ReturnOutput($"pac auth select --name {envName}");
        if (String.IsNullOrEmpty(output)) {
            return "Unexpected error when retrieving output, command returned no output.\nExiting...";
        }
        return "\nFailed to select an authentication profile for this environment.\nCreating one...";
        return null;
    }

    [GeneratedRegex(@".*\\AppData\\Local\\Microsoft\\PowerAppsCLI\\.*", RegexOptions.IgnoreCase, "en-NZ")]
    private static partial Regex MyRegex();
}