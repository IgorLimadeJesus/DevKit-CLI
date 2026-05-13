namespace DevKit;

public class SetupService(InstallService installService)
{
    private readonly InstallService _installService = installService;

    // ── Ponto de entrada: comando `setup` ─────────────────────────────────────
    public void RunWizard()
    {
        PrintHeader("DevKit – Wizard de configuração");

        var tools = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        AskBackend(tools);
        AskFrontend(tools);
        AskDatabase(tools);
        ConfirmAndInstall(tools, "Setup concluído!");
    }

    // ── Ponto de entrada: comando `install --profile <perfil>` ────────────────
    public void RunProfileWizard(string profile)
    {
        switch (profile.ToLower())
        {
            case "fullstack":
                PrintHeader("Perfil: Fullstack");
                var fsTools = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                AskBackend(fsTools);
                AskFrontend(fsTools);
                AskDatabase(fsTools);
                ConfirmAndInstall(fsTools, $"Perfil '{profile}' instalado!");
                break;

            case "backend":
                PrintHeader("Perfil: Backend");
                var beTools = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                AskBackend(beTools);
                AskDatabase(beTools);
                ConfirmAndInstall(beTools, $"Perfil '{profile}' instalado!");
                break;

            case "frontend":
                PrintHeader("Perfil: Frontend");
                var feTools = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                AskFrontend(feTools);
                ConfirmAndInstall(feTools, $"Perfil '{profile}' instalado!");
                break;

            case "mobile":
            case "devops":
                // Perfis fixos — sem perguntas de stack
                if (!ToolRegistry.Profiles.TryGetValue(profile.ToLower(), out var fixed_))
                {
                    Console.WriteLine($"Perfil '{profile}' não encontrado.");
                    return;
                }
                PrintHeader($"Perfil: {profile}");
                var fixedTools = new HashSet<string>(fixed_, StringComparer.OrdinalIgnoreCase);
                ConfirmAndInstall(fixedTools, $"Perfil '{profile}' instalado!");
                break;

            default:
                var available = string.Join(", ", ToolRegistry.Profiles.Keys.Append("fullstack"));
                Console.WriteLine($"Perfil '{profile}' não encontrado. Perfis disponíveis: {available}");
                break;
        }
    }

    // ── Blocos de perguntas ───────────────────────────────────────────────────

    private static void AskBackend(HashSet<string> tools)
    {
        int idx = AskChoice(
            "Qual é sua stack de backend?",
            ToolRegistry.BackendStacks.Select(s => s.Label).ToArray(),
            allowSkip: false
        );
        foreach (var t in ToolRegistry.BackendStacks[idx].Tools)
            tools.Add(t);
    }

    private static void AskFrontend(HashSet<string> tools)
    {
        int idx = AskChoice(
            "Qual é sua stack de frontend?",
            ToolRegistry.FrontendStacks.Select(s => s.Label).ToArray(),
            allowSkip: false
        );
        // índice da última opção ("Nenhum") não adiciona nada
        foreach (var t in ToolRegistry.FrontendStacks[idx].Tools)
            tools.Add(t);
    }

    private static void AskDatabase(HashSet<string> tools)
    {
        if (!AskYesNo("Vai usar banco de dados?"))
            return;

        int idx = AskChoice(
            "Qual banco de dados você vai usar?",
            ToolRegistry.Databases.Select(d => d.Label).ToArray(),
            allowSkip: false
        );

        var db = ToolRegistry.Databases[idx];

        if (AskYesNo($"Deseja instalar o {db.Key} localmente nesta máquina?"))
        {
            tools.Add(db.ToolName);
            tools.Add("dbeaver"); // GUI para gerenciar o banco
        }
        else
        {
            Console.WriteLine("  -> OK, o banco não será instalado localmente.");
        }
    }

    private void ConfirmAndInstall(HashSet<string> tools, string doneMessage)
    {
        if (tools.Count == 0)
        {
            Console.WriteLine("Nenhuma ferramenta selecionada.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Ferramentas que serão instaladas:");
        Console.WriteLine(new string('─', 50));
        foreach (var name in tools)
            Console.WriteLine($"  • {ToolRegistry.Find(name)?.DisplayName ?? name}");
        Console.WriteLine(new string('─', 50));

        if (!AskYesNo("Confirmar e instalar agora?"))
        {
            Console.WriteLine("Instalação cancelada.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Iniciando instalação...");
        Console.WriteLine(new string('─', 50));

        foreach (var name in tools)
            _installService.InstallTool(name);

        Console.WriteLine(new string('─', 50));
        Console.WriteLine(doneMessage);
        Console.WriteLine("Reinicie o terminal para aplicar as mudanças.");
        Console.WriteLine();
    }

    // ── Helpers de I/O ───────────────────────────────────────────────────────

    private static void PrintHeader(string title)
    {
        int width = 50;
        string line = $"║  {title.PadRight(width - 4)}║";
        Console.WriteLine();
        Console.WriteLine("╔" + new string('═', width - 2) + "╗");
        Console.WriteLine(line);
        Console.WriteLine("╚" + new string('═', width - 2) + "╝");
        Console.WriteLine();
    }

    private static int AskChoice(string question, string[] options, bool allowSkip)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine(question);
            for (int i = 0; i < options.Length; i++)
                Console.WriteLine($"  {i + 1}) {options[i]}");
            if (allowSkip)
                Console.WriteLine("  0) Pular");

            Console.Write("> ");
            string? input = Console.ReadLine()?.Trim();

            if (allowSkip && input == "0") return -1;

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Length)
                return choice - 1;

            Console.WriteLine("Opção inválida. Digite o número correspondente.");
        }
    }

    private static bool AskYesNo(string question)
    {
        while (true)
        {
            Console.Write($"\n{question} (s/n) > ");
            string? input = Console.ReadLine()?.Trim().ToLower();
            if (input is "s" or "sim" or "y" or "yes") return true;
            if (input is "n" or "nao" or "não" or "no") return false;
            Console.WriteLine("Responda com 's' ou 'n'.");
        }
    }
}
