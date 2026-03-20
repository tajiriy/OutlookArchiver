# OutlookVault ビルドスクリプト (PowerShell)
# 使い方:
#   .\build.ps1          # Debug ビルド（デフォルト）
#   .\build.ps1 Release  # Release ビルド

param(
    [string]$Config = "Debug"
)

$ErrorActionPreference = "Stop"

# vswhere で最新の Visual Studio から MSBuild.exe を検出
$vswhere = "C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe"
$vsPath = & $vswhere -latest -requires Microsoft.Component.MSBuild -property installationPath
if (-not $vsPath) {
    Write-Error "エラー: MSBuild がインストールされた Visual Studio が見つかりません"
    exit 1
}
$msbuild = Join-Path $vsPath "MSBuild\Current\Bin\MSBuild.exe"
$scriptDir = $PSScriptRoot
$project = Join-Path $scriptDir "OutlookVault\OutlookVault.vbproj"
$testProject = Join-Path $scriptDir "OutlookVault.Tests\OutlookVault.Tests.vbproj"

# === バージョン PATCH +1 ===
$assemblyInfo = Join-Path $scriptDir "OutlookVault\My Project\AssemblyInfo.vb"

if (Test-Path $assemblyInfo) {
    $content = Get-Content $assemblyInfo -Raw
    if ($content -match 'AssemblyVersion\("(\d+)\.(\d+)\.(\d+)\.(\d+)"\)') {
        $major = $Matches[1]
        $minor = $Matches[2]
        $patch = [int]$Matches[3] + 1
        $current = "$($Matches[1]).$($Matches[2]).$($Matches[3]).$($Matches[4])"
        $newVer = "$major.$minor.$patch.0"
        $content = $content -replace "AssemblyVersion\(`"$current`"\)", "AssemblyVersion(`"$newVer`")"
        $content = $content -replace "AssemblyFileVersion\(`"$current`"\)", "AssemblyFileVersion(`"$newVer`")"
        Set-Content $assemblyInfo -Value $content -NoNewline
        Write-Host "=== バージョン更新: $current -> $newVer ==="
    }
}

Write-Host ""
Write-Host "=== NuGet パッケージ復元 ==="
& $msbuild $project /t:Restore /p:Configuration=$Config
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host ""
Write-Host "=== ビルド ($Config) ==="
& $msbuild $project /p:Configuration=$Config
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host ""
Write-Host "=== テストプロジェクト: NuGet パッケージ復元 ==="
& $msbuild $testProject /t:Restore /p:Configuration=$Config
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host ""
Write-Host "=== テストプロジェクト: ビルド ($Config) ==="
& $msbuild $testProject /p:Configuration=$Config
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
