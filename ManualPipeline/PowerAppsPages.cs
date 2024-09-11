namespace ManualPipeline;

public static class PowerAppsPages {
    
    /* Setup */
    private const String confirmRepo = "Local repository has not been created\nWould you like to do this now? The download will take a few minutes to complete. (Y/N)";
    public static Boolean Installed(String path) {
        if (!Directory.Exists(path)) {
            Console.WriteLine("Directory doesn't exist.");
            return false;
        }
        return true;
    }
    public static String? CreateRepo(String path) {

        return null;
    }
    
    /* Git */
}