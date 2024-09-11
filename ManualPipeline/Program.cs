namespace ManualPipeline;

public static class Program {
    public static void Main(string[] args) {
        String? error = null;

        /* PowerApps Portal CLI*/
        if (!PowerAppsPipeline.Installed()) {
            PowerAppsPipeline.InstallAll(ref error);
            if (error is not null) {
                Console.WriteLine(error);
                return;
            }
            Console.WriteLine("This terminal window will need to be restarted for the environment variables to be available.\nPress any key to exit...");
            _ = Console.ReadKey(true).Key;
            return;
        }
        Console.WriteLine("Found PowerApps CLI.");
        if ((error = PowerAppsPipeline.UpdateCLI()) is not null) {
            Console.WriteLine(error);
            return;
        }
        String env = PowerAppsPipeline.SelectEnvironment();
        if ((error = PowerAppsPipeline.SelectAuth(env, ref error)) is not null) {
            Console.WriteLine(error);
            return;
        }
        Console.WriteLine($"Environment set to {env}.");
        
        

        if (error is not null) {
            Console.WriteLine($"Uncaught error:\n{error}");
        }
        Console.WriteLine("Ended successfully.");
    }
}