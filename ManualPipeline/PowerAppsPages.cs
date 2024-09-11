namespace ManualPipeline;

public static class PowerAppsPages {
    
    /* Setup */
    private const String confirmRepo = "Local repository has not been created\nWould you like to do this now? The download will take a few minutes to complete.\nIf a personal access token has not been created for Azure Devops you will need to do this before continuing. (Y/N)";
    public static Boolean Installed(String path) {
        if (!Directory.Exists(path)) {
            Console.WriteLine("Directory doesn't exist.");
            return false;
        }
        return true;
    }
    public static String? CreateDir(String path) {
        try {
            Directory.CreateDirectory(path);
            return null;
        }
        catch (Exception e) {
            return $"Error while creating the repository directory\nException: {e.Message}";
        }
    }
    
    /* Git */
    public static String? PullRepo(String path) {
        try {
            GenericFunctions.Shell.AutoClose(
                $"cd {path}\ngit clone https://mitodevcode@dev.azure.com/mitodevcode/mitosource/_git/MyMITOPortal");
        }
        catch (Exception e) {
            return $"Error while pulling the repository repository\nException: {e.Message}";
        }
        return null;
    }
}   