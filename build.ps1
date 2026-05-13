param(
    [string]$Version = "1.0.0"
)

$project  = "DevKit/DevKit.csproj"
$output   = "dist"
$targets  = @(
    @{ RID = "win-x64";   Name = "devkit-win-x64.exe"   },
    @{ RID = "osx-x64";   Name = "devkit-osx-x64"       },
    @{ RID = "osx-arm64"; Name = "devkit-osx-arm64"      },
    @{ RID = "linux-x64"; Name = "devkit-linux-x64"      },
    @{ RID = "linux-arm64"; Name = "devkit-linux-arm64"  }
)

if (Test-Path $output) { Remove-Item $output -Recurse -Force }
New-Item $output -ItemType Directory | Out-Null

foreach ($t in $targets) {
    Write-Host "Publicando $($t.RID)..." -ForegroundColor Cyan

    dotnet publish $project `
        -c Release `
        -r $t.RID `
        --self-contained true `
        -p:PublishSingleFile=true `
        -p:EnableCompressionInSingleFile=true `
        -p:IncludeNativeLibrariesForSelfExtract=true `
        -o "dist/tmp/$($t.RID)" | Out-Null

    $exeName = if ($t.RID -like "win*") { "DevKit.exe" } else { "DevKit" }
    $src     = "dist/tmp/$($t.RID)/$exeName"

    if (Test-Path $src) {
        Copy-Item $src "$output/$($t.Name)"
        Write-Host "  -> $output/$($t.Name)" -ForegroundColor Green
    } else {
        Write-Warning "  -> Binário não encontrado: $src"
    }
}

Remove-Item "dist/tmp" -Recurse -Force

Write-Host ""
Write-Host "Binários gerados em '$output/':" -ForegroundColor Yellow
Get-ChildItem $output | Format-Table Name, @{L="Tamanho";E={"{0:N0} KB" -f ($_.Length / 1KB)}}

Write-Host "Próximos passos:" -ForegroundColor Cyan
Write-Host "  1. Crie uma Release v$Version no GitHub e faça upload dos arquivos em '$output/'"
Write-Host "  2. Atualize a versão em installer/package.json para '$Version'"
Write-Host "  3. cd installer ; npm publish"
