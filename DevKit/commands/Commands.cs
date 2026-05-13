namespace DevKit;

public class Commands
{
    private readonly CommandService _commandService;
    private readonly InstallService _installService;
    private readonly SetupService _setupService;

    public Commands(CommandService commandService, InstallService installService, SetupService setupService)
    {
        _commandService = commandService;
        _installService = installService;
        _setupService = setupService;
    }

    public void Menu(string[] args)
    {
        // Se um comando foi passado diretamente como argumento, executa e sai
        if (args.Length >= 1)
        {
            ExecuteCommand(args);
            return;
        }

        // Loop interativo
        Console.WriteLine(@"
██████╗ ███████╗██╗   ██╗    ██╗  ██╗██╗████████╗
██╔══██╗██╔════╝██║   ██║    ██║ ██╔╝██║╚══██╔══╝
██║  ██║█████╗  ██║   ██║    █████╔╝ ██║   ██║   
██║  ██║██╔══╝  ╚██╗ ██╔╝    ██╔═██╗ ██║   ██║   
██████╔╝███████╗ ╚████╔╝     ██║  ██╗██║   ██║   
╚═════╝ ╚══════╝  ╚═══╝      ╚═╝  ╚═╝╚═╝   ╚═╝   
");
        Console.WriteLine(new string('─', 50));
        Console.WriteLine("Digite 'help' para ver os comandos disponíveis.");
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                continue;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (ExecuteCommand(parts) == false)
                break;
        }
    }

    private bool ExecuteCommand(string[] parts)
    {
        string command = parts[0].ToLower();
        string[] cmdArgs = parts.Skip(1).ToArray();

        switch (command)
        {
            case "help":
                _commandService.ShowHelp();
                break;
            case "list":
                _installService.ListTools();
                break;
            case "install":
                // --profile é tratado pelo wizard interativo
                if (cmdArgs.Length >= 2 && cmdArgs[0] is "--profile" or "-p")
                    _setupService.RunProfileWizard(cmdArgs[1]);
                else
                    _installService.HandleInstall(cmdArgs);
                break;
            case "setup":
                _setupService.RunWizard();
                break;
            case "exit":
                Console.WriteLine("Encerrando o DevKit...");
                return false;
            default:
                Console.WriteLine($"Comando '{command}' não reconhecido. Use 'help' para obter ajuda.");
                break;
        }
        return true;
    }
}
