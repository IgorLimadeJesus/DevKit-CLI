namespace DevKit;

public record Tool(
    string Name,
    string DisplayName,
    string? WingetId,   // Windows – winget
    string? BrewId,     // macOS   – brew  (prefix "cask:" para casks)
    string? AptId,      // Linux   – apt / snap (prefix "snap:" para snaps)
    string[] Tags
);
