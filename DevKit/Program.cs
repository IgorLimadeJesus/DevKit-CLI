using DevKit;

public class Program
{
    public static void Main(string[] args)
    {
        CommandService commandService = new CommandService();
        InstallService installService = new InstallService();
        SetupService setupService = new SetupService(installService);
        Commands commands = new Commands(commandService, installService, setupService);
        commands.Menu(args);

    }
}