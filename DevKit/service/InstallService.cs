namespace DevKit;

public class InstallService
{
    public void HandleInstall(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Uso: install <ferramenta> | install --profile <perfil>");
            Console.WriteLine("     Use 'list' para ver as ferramentas e perfis disponíveis.");
            return;
        }

        if (args[0] is "--profile" or "-p")
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Informe o perfil. Ex: install --profile fullstack");
                return;
            }
            InstallProfile(args[1]);
        }
        else
        {
            InstallTool(args[0]);
        }
    }

    public void ListTools()
    {
        Console.WriteLine();
        Console.WriteLine("Ferramentas disponíveis:");
        Console.WriteLine($"  {"Nome",-12} {"Descrição",-30}");
        Console.WriteLine("  " + new string('─', 42));
        foreach (var tool in ToolRegistry.Tools)
            Console.WriteLine($"  {tool.Name,-12} {tool.DisplayName}");

        Console.WriteLine();
        Console.WriteLine("Perfis disponíveis:");
        Console.WriteLine($"  {"Perfil",-12} {"Ferramentas"}");
        Console.WriteLine("  " + new string('─', 42));
        foreach (var (profile, tools) in ToolRegistry.Profiles)
            Console.WriteLine($"  {profile,-12} {string.Join(", ", tools)}");

        Console.WriteLine();
    }

    public void InstallTool(string name)
    {
        var tool = ToolRegistry.Find(name);
        if (tool is null)
        {
            Console.WriteLine($"Ferramenta '{name}' não encontrada. Use 'list' para ver as disponíveis.");
            return;
        }

        // Verifica se há suporte na plataforma atual
        if (PackageManagerService.GetPackageId(tool) is null)
        {
            Console.WriteLine($"[{tool.Name}] {tool.DisplayName} – não disponível no {PackageManagerService.PlatformName}, pulando.");
            return;
        }

        Console.Write($"[{tool.Name}] Verificando {tool.DisplayName}... ");

        if (PackageManagerService.IsInstalled(tool))
        {
            Console.WriteLine("já instalado, pulando.");
            return;
        }

        Console.WriteLine("não encontrado, instalando...");
        bool ok = PackageManagerService.Install(tool);
        Console.WriteLine(ok ? "  -> Instalado com sucesso." : "  -> Falha na instalação.");
    }

    private void InstallProfile(string profileName)
    {
        if (!ToolRegistry.Profiles.TryGetValue(profileName.ToLower(), out var tools))
        {
            var available = string.Join(", ", ToolRegistry.Profiles.Keys);
            Console.WriteLine($"Perfil '{profileName}' não encontrado. Perfis disponíveis: {available}");
            return;
        }

        Console.WriteLine($"Instalando perfil '{profileName}' ({tools.Length} ferramentas)...");
        Console.WriteLine(new string('─', 50));

        foreach (var toolName in tools)
            InstallTool(toolName);

        Console.WriteLine(new string('─', 50));
        Console.WriteLine($"Perfil '{profileName}' instalado.");
    }
}
