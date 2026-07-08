#!/usr/bin/env bash
set -euo pipefail

BUILD_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$BUILD_DIR/.." && pwd)"

export DOTNET_CLI_HOME="$BUILD_DIR/.dotnet-home"
export DOTNET_ROOT="$BUILD_DIR/.dotnet"
export PATH="$DOTNET_ROOT:$PATH"

"$DOTNET_ROOT/dotnet" build "$ROOT_DIR/src/IntelliJIdeaPlugin.csproj" -c Release /p:CreatePluginLink=false
"$DOTNET_ROOT/dotnet" run --project "$ROOT_DIR/tests/IntelliJIdeaPlugin.Tests/IntelliJIdeaPlugin.Tests.csproj" -c Release
"$BUILD_DIR/.tools/logiplugintool" pack "$BUILD_DIR/bin/Release" "$BUILD_DIR/IntelliJIdeaActionRing_1_0.lplug4"
"$BUILD_DIR/.tools/logiplugintool" verify "$BUILD_DIR/IntelliJIdeaActionRing_1_0.lplug4"
