# OutlookVault テスト実行スクリプト (PowerShell)
# 使い方:
#   .\test.ps1          # 全テスト実行
#   .\test.ps1 Release  # Release ビルドのテスト実行

param(
    [string]$Config = "Debug"
)

$ErrorActionPreference = "Stop"

# vswhere で最新の Visual Studio から vstest.console.exe を検出
$vswhere = "C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe"
$vsPath = & $vswhere -latest -requires Microsoft.VisualStudio.PackageGroup.TestTools.Core -property installationPath
if (-not $vsPath) {
    Write-Error "エラー: テストツールがインストールされた Visual Studio が見つかりません"
    exit 1
}
$vstest = Join-Path $vsPath "Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
$scriptDir = $PSScriptRoot
$testDll = Join-Path $scriptDir "OutlookVault.Tests\bin\$Config\OutlookVault.Tests.dll"

Write-Host "=== テスト実行 ==="
& $vstest $testDll
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
