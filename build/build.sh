#!/usr/bin/env bash
set -euo pipefail

BUILD_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$BUILD_DIR/.." && pwd)"
TOOL_DIR="$BUILD_DIR/.tools"
LOGI_PLUGIN_TOOL_VERSION="${LOGI_PLUGIN_TOOL_VERSION:-6.1.4.22672}"

export DOTNET_CLI_HOME="${DOTNET_CLI_HOME:-$BUILD_DIR/.dotnet-home}"
mkdir -p "$DOTNET_CLI_HOME" "$TOOL_DIR"

if [ -z "${DOTNET_BIN:-}" ]; then
    if [ -x "$BUILD_DIR/.dotnet/dotnet" ]; then
        export DOTNET_ROOT="${DOTNET_ROOT:-$BUILD_DIR/.dotnet}"
        export PATH="$DOTNET_ROOT:$PATH"
        DOTNET_BIN="$DOTNET_ROOT/dotnet"
    else
        DOTNET_BIN="dotnet"
    fi
fi

LOGI_PLUGIN_TOOL="$TOOL_DIR/logiplugintool"
if [ ! -x "$LOGI_PLUGIN_TOOL" ] && [ -x "$TOOL_DIR/logiplugintool.exe" ]; then
    LOGI_PLUGIN_TOOL="$TOOL_DIR/logiplugintool.exe"
fi

if [ ! -x "$LOGI_PLUGIN_TOOL" ]; then
    "$DOTNET_BIN" tool install LogiPluginTool --tool-path "$TOOL_DIR" --version "$LOGI_PLUGIN_TOOL_VERSION"
fi

PLUGIN_API_DLL="$(find "$TOOL_DIR/.store/logiplugintool" -path "*/tools/*/any/PluginApi.dll" -print -quit 2>/dev/null || true)"
if [ -z "$PLUGIN_API_DLL" ]; then
    echo "PluginApi.dll was not found in LogiPluginTool package." >&2
    exit 1
fi

PLUGIN_API_DIR="$(cd "$(dirname "$PLUGIN_API_DLL")" && pwd)/"
PACKAGE_FILE="$BUILD_DIR/IntelliJIdeaActionRing_1_1.lplug4"

"$DOTNET_BIN" build "$ROOT_DIR/src/IntelliJIdeaPlugin.csproj" -c Release /p:CreatePluginLink=false /p:PluginApiDir="$PLUGIN_API_DIR"
"$DOTNET_BIN" run --project "$ROOT_DIR/tests/IntelliJIdeaPlugin.Tests/IntelliJIdeaPlugin.Tests.csproj" -c Release -p:PluginApiDir="$PLUGIN_API_DIR"
"$LOGI_PLUGIN_TOOL" pack "$BUILD_DIR/bin/Release" "$PACKAGE_FILE"
"$LOGI_PLUGIN_TOOL" verify "$PACKAGE_FILE"
