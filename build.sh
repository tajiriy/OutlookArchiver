#!/usr/bin/env bash
# OutlookArchiver ビルドスクリプト
# 使い方:
#   ./build.sh          # Debug ビルド（デフォルト）
#   ./build.sh Release  # Release ビルド

set -euo pipefail

# vswhere で最新の Visual Studio から MSBuild.exe を検出
VSWHERE="C:\\Program Files (x86)\\Microsoft Visual Studio\\Installer\\vswhere.exe"
VS_PATH=$(powershell.exe -NoProfile -Command "& '$VSWHERE' -latest -requires Microsoft.Component.MSBuild -property installationPath" | tr -d '\r')
if [ -z "$VS_PATH" ]; then
    echo "エラー: MSBuild がインストールされた Visual Studio が見つかりません" >&2
    exit 1
fi
MSBUILD="${VS_PATH}\\MSBuild\\Current\\Bin\\MSBuild.exe"
PROJECT="d:\\Development\\VisualStudioProjects\\OutlookArchiver\\OutlookArchiver\\OutlookArchiver.vbproj"
TEST_PROJECT="d:\\Development\\VisualStudioProjects\\OutlookArchiver\\OutlookArchiver.Tests\\OutlookArchiver.Tests.vbproj"
CONFIG="${1:-Debug}"

echo "=== NuGet パッケージ復元 ==="
powershell.exe -NoProfile -Command "& '$MSBUILD' '$PROJECT' /t:Restore /p:Configuration=$CONFIG"

echo ""
echo "=== ビルド (${CONFIG}) ==="
powershell.exe -NoProfile -Command "& '$MSBUILD' '$PROJECT' /p:Configuration=$CONFIG"

echo ""
echo "=== テストプロジェクト: NuGet パッケージ復元 ==="
powershell.exe -NoProfile -Command "& '$MSBUILD' '$TEST_PROJECT' /t:Restore /p:Configuration=$CONFIG"

echo ""
echo "=== テストプロジェクト: ビルド (${CONFIG}) ==="
powershell.exe -NoProfile -Command "& '$MSBUILD' '$TEST_PROJECT' /p:Configuration=$CONFIG"
