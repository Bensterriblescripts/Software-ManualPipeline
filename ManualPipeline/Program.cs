namespace ManualPipeline;

public static class Program {
    public static void Main(String[] args) {
        String? error = null;

        /* PowerApps Portal CLI*/
        if (!PowerAppsCLI.Installed()) {
            PowerAppsCLI.InstallAll(ref error);
            if (error is not null) {
                Console.WriteLine(error);
                return;
            }
            Console.WriteLine("This terminal window will need to be restarted for the environment variables to be available.\nPress any key to exit...");
            _ = Console.ReadKey(true).Key;
            return;
        }
        Console.WriteLine("Found PowerApps CLI.");
        if ((error = PowerAppsCLI.UpdateCLI()) is not null) {
            Console.WriteLine(error);
            return;
        }
        (String envName, String envGUID) = PowerAppsCLI.SelectEnvironment();
        if ((error = PowerAppsCLI.SelectAuth(envName, ref error)) is not null) {
            Console.WriteLine(error);
            return;
        }
        Console.WriteLine($"Environment set to {envName}.");
        
        /* PowerApps Pages */
        if (!PowerAppsPages.Installed(envName)) {
            if ((error = PowerAppsPages.CreateRepo(envName)) != null) {
                Console.WriteLine(error);
                return;
            }
        }
        
        
        

        if (error is not null) {
            Console.WriteLine($"Uncaught error:\n{error}");
        }
        Console.WriteLine("Ended successfully.");
    }
}