using TodoGenieLib.Functions;
using TodoGenieLib.Models;
using TodoGenieLib.Utils;
/*
    param (
        [Parameter(Position=1,
            HelpMessage = "Directory with the .git folder")]
        [Alias('d','dir')]
        [string] $gitDirectory = $PWD
        ,
        [Parameter(ParameterSetName = 'TestMode',
            HelpMessage = 'Enables test mode, which runs through all subcommands in the specified -TestDirectory')]
        [Alias('t','test')]
        [switch] $testMode
        ,
        [Parameter(ParameterSetName = 'TestMode',
            HelpMessage = 'Test directory to run -TestMode in. Defaults to test/')]
        [Alias('td', 'testdir')]
        [string] $testDirectory = "test/"
        ,
        [Parameter(
            HelpMessage = 'Avoids commiting the changes to the repo automatically')]
        [Alias('n')]
        [switch] $noAutoCommit
        ,
        [Parameter(
            HelpMessage = 'Shows the syntax/help message')]
        [Alias('h')]
        [switch] $help
        ,
        [Parameter(
            HelpMessage = 'Used to update ApiKey')]
        [string] $newApikey = ""
        ,
        [Alias('X')]
        [Parameter(
            HelpMessage = 'Excluded Directories (name only), comma seperated [ex: folder1,folder2,folder3]')]
        [string[]] $excludedDirs
    )
*/
public static class Utils {
    public static void PrintUsage() {
        Console.WriteLine("USAGE: Invoke-Genie (list, prune, create) [--GitDirectory [string]] [--TestMode] [--TestDirectory [string]] [--NoAutoCommit] [--Unreported]");
    }
    public static void PrintHelp() {
        PrintUsage();
        Console.WriteLine("\nARGUMENTS");
        Console.WriteLine("\n---- Global ----\n");
        Console.WriteLine("TestMode - Enables testing mode which runs through all subcommands in the specified -TestDirectory");
        Console.WriteLine("TestDirectory - Directory to run tests in");
        Console.WriteLine("GitDirectory - Directory to base TodoGenie in. Needs to have a .git folder");
        Console.WriteLine("\n---- List ----\n");
        Console.WriteLine("Unreported - Shows only Todos that are unreported to Github");
        Console.WriteLine("\n---- Create ----\n");
        Console.WriteLine("NoAutoCommit - Doesn't commit updated Todo to Github automatically.");
        Console.WriteLine("\n---- Prune ----\n");
        Console.WriteLine("NOT IMPLEMENTED");
        Console.WriteLine("\n---- Config ----\n");
        Console.WriteLine("ApiKey - Github Api token for creating issues. This will overwrite any existing token");


    }
    public static ConfigModel ParseArgs(string[] args) {
        ConfigModel config = new();

        config = ConfigFunctions.SetupConfigDir(config);

        int argCount = args.Length;
        if (argCount < 1) {
            return config;
        }

        var tempCommand = args[0].ToLower();
        if(new List<string>{"--help", "-h", "?"}.Contains(tempCommand)) {
            PrintHelp();
            Environment.Exit(0);
        }
        if(!new List<string>{"list", "prune", "create", "config"}.Contains(tempCommand)) {
            PrintUsage();
            Error.Critical("Expected one of the following commands: list, prune, create, config");
        }
        config.Command = tempCommand;
        for(int i = 0; i < argCount; i++) {
            try {
                switch(args[i].ToLower()) {
                    case "--apikey":
                        config.GithubApiKey = args[i+1];
                        break;
                    case "--gitdirectory":
                        config.RootDirectory = args[i+1];
                        break;
                    case "--unreported":
                        config.ShowUnreportedOnly = true;
                        break;
                    case "--noautocommit":
                        //TODO: implement this argument
                    case "--testmode":
                        //TODO: implement this argument
                    case "--testdirectory":
                        //TODO: implement this argument
                        break;
                    case "--exclude":
                        foreach(var dir in args[i+1].Split(',')) {
                            config.ExcludedDirs.Add(dir);
                        }
                        break;
                    default:
                        break;
                }
            } catch (Exception e) when (e is IndexOutOfRangeException)  {
                Error.Critical($"No valid argument specified for {args[i]}");
            } catch (Exception e) {
                Error.Critical($"Could not parse args {e.Message}");
            }
        }
        return config;
    }    
}