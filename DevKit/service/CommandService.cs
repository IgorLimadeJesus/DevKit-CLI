namespace DevKit;

public class CommandService
{
    public void ShowHelp()
    {
        Console.WriteLine();
        Console.WriteLine("Comandos disponíveis:");
        Console.WriteLine($"  {"help",-32} Exibe esta mensagem de ajuda.");
        Console.WriteLine($"  {"list",-32} Lista ferramentas e perfis disponíveis.");
        Console.WriteLine($"  {"setup",-32} Wizard interativo de configuração do ambiente.");
        Console.WriteLine($"  {"install <ferramenta>",-32} Instala uma ferramenta. Ex: install git");
        Console.WriteLine($"  {"install --profile <perfil>",-32} Instala um perfil. Ex: install --profile backend");
        Console.WriteLine($"  {"exit",-32} Encerra o DevKit.");
        Console.WriteLine();
    }
}
