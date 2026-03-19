#!/usr/bin/env bash
# OutlookVault テスト実行スクリプト
# 使い方:
#   ./test.sh          # 全テスト実行
#   ./test.sh Release  # Release ビルドのテスト実行

set -euo pipefail

# vswhere で最新の Visual Studio から vstest.console.exe を検出
VSWHERE="C:\\Program Files (x86)\\Microsoft Visual Studio\\Installer\\vswhere.exe"
VS_PATH=$(powershell.exe -NoProfile -Command "& '$VSWHERE' -latest -requires Microsoft.VisualStudio.PackageGroup.TestTools.Core -property installationPath" | tr -d '\r')
if [ -z "$VS_PATH" ]; then
    echo "エラー: テストツールがインストールされた Visual Studio が見つかりません" >&2
    exit 1
fi
VSTEST="${VS_PATH}\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe"
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd -W | sed 's|/|\\\\|g')"
TEST_DLL="${SCRIPT_DIR}\\OutlookVault.Tests\\bin\\${1:-Debug}\\OutlookVault.Tests.dll"

echo "=== テスト実行 ==="
powershell.exe -NoProfile -Command "& '$VSTEST' '$TEST_DLL'"
