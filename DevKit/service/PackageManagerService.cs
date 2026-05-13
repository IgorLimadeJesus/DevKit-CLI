namespace DevKit;

using System.Diagnostics;
using System.Runtime.InteropServices;

public static class PackageManagerService
{
    public static string PlatformName =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS" :
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" :
        "desconhecido";

    public static string? GetPackageId(Tool tool) =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? tool.WingetId :
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? tool.BrewId :
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? tool.AptId :
        null;

    public static bool IsInstalled(Tool tool)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return tool.WingetId is not null && CheckWinget(tool.WingetId);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return tool.BrewId is not null && CheckBrew(tool.BrewId);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return tool.AptId is not null && CheckLinux(tool.AptId);

        return false;
    }

    public static bool Install(Tool tool)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return tool.WingetId is not null && RunWinget(tool.WingetId);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return tool.BrewId is not null && RunBrew(tool.BrewId);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return tool.AptId is not null && RunApt(tool.AptId);

        return false;
    }

    private static bool CheckWinget(string id)
    {
        var output = RunSilent("winget", $"list --id {id} -e --accept-source-agreements");
        return output is not null && output.Contains(id, StringComparison.OrdinalIgnoreCase);
    }

    private static bool RunWinget(string id)
        => RunInteractive("winget",
            $"install --id {id} -e --accept-source-agreements --accept-package-agreements") == 0;

    private static bool CheckBrew(string id)
    {
        bool isCask = id.StartsWith("cask:");
        string pkg = isCask ? id[5..] : id;
        string args = isCask ? $"list --cask {pkg}" : $"list {pkg}";
        return RunSilent("brew", args) is not null;
    }

    private static bool RunBrew(string id)
    {
        bool isCask = id.StartsWith("cask:");
        string pkg = isCask ? id[5..] : id;
        string args = isCask ? $"install --cask {pkg}" : $"install {pkg}";
        return RunInteractive("brew", args) == 0;
    }

    private static bool CheckLinux(string id)
    {
        if (id.StartsWith("snap:"))
        {
            string pkg = id[5..];
            var output = RunSilent("snap", $"list {pkg}");
            return output is not null && output.Contains(pkg, StringComparison.OrdinalIgnoreCase);
        }

        var dpkg = RunSilent("dpkg", $"-s {id}");
        return dpkg is not null && dpkg.Contains("Status: install ok installed");
    }

    private static bool RunApt(string id)
    {
        if (id.StartsWith("snap:"))
        {
            string pkg = id[5..];
            string classic = pkg is "code" or "postman" or "insomnia" ? " --classic" : "";
            return RunInteractive("snap", $"install {pkg}{classic}") == 0;
        }

        return RunInteractive("sudo", $"apt install -y {id}") == 0;
    }

    private static string? RunSilent(string file, string args)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            using var p = Process.Start(psi)!;
            var errTask = p.StandardError.ReadToEndAsync();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            _ = errTask.Result;

            return p.ExitCode == 0 ? output : null;
        }
        catch { return null; }
    }

    private static int RunInteractive(string file, string args)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                UseShellExecute = false,
            };

            using var p = Process.Start(psi)!;
            p.WaitForExit();
            return p.ExitCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  -> Erro ao executar '{file}': {ex.Message}");
            return -1;
        }
    }
}
