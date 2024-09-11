namespace ManualPipeline;

public static class PowerAppsPages {
    
    /* Setup */
    private const String confirmRepo = "Local repository has not been created\nWould you like to do this now? The download will take a few minutes to complete. (Y/N)";
    public static Boolean Installed(String envName) {
        String? path;
        if ((path = GenericFunctions.Env.GetPath($@"{envName}\custom-portal\")) != null)  {
            Console.WriteLine("Environment variable not found.");
            return false;
        }
        if (!Directory.Exists(path)) {
            Console.WriteLine("Directory doesn't exist.");
            return false;
        }
        return true;
    }
    public static String? CreateRepo(String envName) {
        String path = $@"C:\Repositories\MyMITOPortal\{envName}\";
        if (!GenericFunctions.Cli.ConfirmResponse(confirmRepo, ConsoleKey.Y, ConsoleKey.N)) {
            return "Exiting...";
        }
        String? existingPath = GenericFunctions.Env.GetPath(path);
        if (existingPath == null) {
            Console.WriteLine($@"Repository will be created in C:\Repositories\MyMITOPortal\{envName}");
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Machine);
        }
        if (!Directory.Exists(existingPath)) {
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Machine);
        }

        return null;
    }
    
    /* Git */
}