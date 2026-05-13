namespace DevKit;

public static class ToolRegistry
{
    // new(name, displayName, wingetId, brewId, aptId, tags)
    // brewId: prefixo "cask:" para GUI apps no macOS
    // aptId:  prefixo "snap:" para pacotes via snap no Linux
    public static readonly List<Tool> Tools =
    [
        // Controle de versão
        new("git",        "Git",                   "Git.Git",                              "git",                       "git",                  ["vcs"]),

        // Editor
        new("vscode",     "Visual Studio Code",    "Microsoft.VisualStudioCode",           "cask:visual-studio-code",   "snap:code",            ["editor"]),

        // Terminal
        new("wt",         "Windows Terminal",      "Microsoft.WindowsTerminal",             null,                        null,                   ["terminal"]),

        // API clients
        new("postman",    "Postman",               "Postman.Postman",                      "cask:postman",              "snap:postman",         ["api"]),
        new("insomnia",   "Insomnia",              "Insomnia.Insomnia",                    "cask:insomnia",             "snap:insomnia",        ["api"]),

        // Containers
        new("docker",     "Docker Desktop",        "Docker.DockerDesktop",                 "cask:docker",               "docker.io",            ["devops"]),

        // Database GUI
        new("dbeaver",    "DBeaver",               "dbeaver.dbeaver",                      "cask:dbeaver-community",    "snap:dbeaver-ce",      ["database"]),

        // Backend runtimes
        new("node",       "Node.js LTS",           "OpenJS.NodeJS.LTS",                    "node",                      "nodejs",               ["backend", "frontend"]),
        new("dotnet",     ".NET SDK",              "Microsoft.DotNet.SDK.9",               "dotnet",                    "dotnet-sdk-9.0",       ["backend"]),
        new("java",       "Java JDK 21 (Temurin)", "EclipseAdoptium.Temurin.21.JDK",       "cask:temurin@21",           "temurin-21-jdk",       ["backend"]),
        new("python",     "Python 3",              "Python.Python.3",                      "python@3",                  "python3",              ["backend"]),
        new("go",         "Go",                    "GoLang.Go",                            "go",                        "golang-go",            ["backend"]),
        new("ruby",       "Ruby",                  "RubyInstallerTeam.RubyWithDevKit.3.3", "ruby",                      "ruby",                 ["backend"]),
        new("php",        "PHP",                   "PHP.PHP",                              "php",                       "php",                  ["backend"]),

        // Bancos de dados locais
        new("postgresql", "PostgreSQL",            "PostgreSQL.PostgreSQL",                "postgresql",                "postgresql",           ["database"]),
        new("mysql",      "MySQL",                 "Oracle.MySQL",                         "mysql",                     "mysql-server",         ["database"]),
        new("mongodb",    "MongoDB Community",     "MongoDB.Server",                       "mongodb-community",         "mongodb-org",          ["database"]),
        new("sqlserver",  "SQL Server Express",    "Microsoft.SQLServer.2022.Express",      null,                        null,                   ["database"]),
        new("mariadb",    "MariaDB",               "MariaDB.Server",                       "mariadb",                   "mariadb-server",       ["database"]),
        new("redis",      "Redis",                 "Memurai.MemuraiDeveloper",              "redis",                     "redis-server",         ["database"]),
    ];

    // Stacks de backend
    public static readonly (string Key, string Label, string[] Tools)[] BackendStacks =
    [
        ("node-express",   "Node.js / Express (JavaScript)",   ["git", "node", "vscode", "docker", "postman", "wt"]),
        ("node-ts",        "Node.js / NestJS (TypeScript)",    ["git", "node", "vscode", "docker", "postman", "wt"]),
        ("dotnet",         "C# / ASP.NET Core",                ["git", "dotnet", "vscode", "docker", "postman", "wt"]),
        ("java-spring",    "Java / Spring Boot",               ["git", "java", "vscode", "docker", "postman", "wt"]),
        ("python-django",  "Python / Django",                  ["git", "python", "vscode", "docker", "postman", "wt"]),
        ("python-fastapi", "Python / FastAPI",                 ["git", "python", "vscode", "docker", "postman", "wt"]),
        ("go",             "Go",                               ["git", "go", "vscode", "docker", "postman", "wt"]),
        ("ruby",           "Ruby on Rails",                    ["git", "ruby", "vscode", "docker", "postman", "wt"]),
        ("php-laravel",    "PHP / Laravel",                    ["git", "php", "node", "vscode", "docker", "postman", "wt"]),
    ];

    // Stacks de frontend
    public static readonly (string Key, string Label, string[] Tools)[] FrontendStacks =
    [
        ("react",   "React",               ["node"]),
        ("nextjs",  "Next.js",             ["node"]),
        ("angular", "Angular",             ["node"]),
        ("vuejs",   "Vue.js",              ["node"]),
        ("svelte",  "Svelte / SvelteKit",  ["node"]),
        ("none",    "Nenhum (só backend)", []),
    ];

    // Bancos disponíveis no wizard
    public static readonly (string Key, string Label, string ToolName)[] Databases =
    [
        ("PostgreSQL", "PostgreSQL  (mais popular, open source)", "postgresql"),
        ("MySQL",      "MySQL       (muito usado, open source)",  "mysql"),
        ("SQL Server", "SQL Server  (Microsoft, só Windows)",     "sqlserver"),
        ("MongoDB",    "MongoDB     (NoSQL, documentos)",         "mongodb"),
        ("MariaDB",    "MariaDB     (fork do MySQL)",             "mariadb"),
        ("Redis",      "Redis       (cache / chave-valor)",       "redis"),
    ];

    public static readonly Dictionary<string, string[]> Profiles = new()
    {
        ["frontend"] = ["git", "node", "vscode", "wt"],
        ["backend"]  = ["git", "dotnet", "docker", "postman", "dbeaver", "vscode", "wt"],
        ["mobile"]   = ["git", "node", "vscode", "java", "wt"],
        ["devops"]   = ["git", "docker", "vscode", "wt"],
    };

    public static Tool? Find(string name) =>
        Tools.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}
