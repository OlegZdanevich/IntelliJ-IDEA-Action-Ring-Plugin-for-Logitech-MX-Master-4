# IntelliJ IDEA Action Ring Plugin for Logitech MX Master 4

**IntelliJ IDEA Action Ring Plugin for Logitech MX Master 4** is a Logi Actions SDK plugin for JetBrains IntelliJ IDEA. It adds ready-to-use Action Ring actions for code cleanup, refactoring, navigation, run/debug, Git, tool windows, and Maven.

This project is intended for people searching for a **Logitech Action Ring plugin for IntelliJ IDEA**, **Logi Options+ IntelliJ IDEA actions**, or an **MX Master 4 Action Ring plugin for JetBrains IDEs**.

## Project Info

- **Project name:** IntelliJ IDEA Action Ring Plugin for Logitech MX Master 4
- **Plugin display name:** IntelliJ IDEA Action Ring
- **Author:** Oleg Zdanevich
- **Version:** 1.0
- **License:** MIT
- **Primary target:** macOS with Logitech MX Master 4, Logi Options+, Logi Plugin Service, and JetBrains IntelliJ IDEA
- **Windows support:** build-ready best effort support for Maven Run Anything submission; runtime validation must be done on a Windows machine

## What This Plugin Does

The plugin gives IntelliJ IDEA a ready-made set of Action Ring actions:

- reformat code;
- optimize imports;
- show intentions;
- generate code;
- rename symbols;
- extract method, variable, or field;
- inline, move, change signature, safe delete;
- navigate to class, file, symbol, declaration, implementation;
- open recent files and recent locations;
- run, debug, stop, and build project;
- debugger step actions;
- commit, push, update project / pull, VCS operations;
- open Terminal and Project tool windows;
- submit `mvn clean install` through IntelliJ IDEA Run Anything.

## Why Maven Runs Through IntelliJ IDEA

The Maven action does not run `/bin/zsh`, `cmd.exe`, or an external `mvn` binary directly. It submits:

```text
mvn clean install
```

to IntelliJ IDEA **Run Anything**.

This matters because different projects often use different:

- project SDK / JDK;
- Maven home;
- Maven importer settings;
- Maven profiles;
- `.mvn/` project settings;
- company-specific IDEA configuration.

When IDEA runs Maven, it uses the same project context you already use manually.

## Important Maven Limitation

The plugin knows when it successfully submits `mvn clean install` to IntelliJ IDEA. It does **not** know the final Maven exit code, because the build result lives inside IntelliJ IDEA.
In other words, the plugin does not know the final Maven exit code.

Haptics therefore mean:

- command submission started;
- command submitted to IDEA;
- command submission failed.

They do not mean:

- Maven build succeeded;
- Maven build failed.

## Supported Platforms

### macOS

macOS support is implemented through AppleScript and System Events:

1. Activate IntelliJ IDEA by bundle id `com.jetbrains.intellij`.
2. Open Run Anything with double Control.
3. Type `mvn clean install`.
4. Press Return.

The action does not touch the clipboard.

### Windows

Windows support is implemented as a separate code path using Win32 `SendInput`:

1. Activate IntelliJ IDEA through the Logi SDK client application API.
2. Press double Control.
3. Type `mvn clean install` as Unicode input.
4. Press Enter.

This is included so the plugin can be built for Windows, but it still needs runtime validation on Windows with Logi Options+ and IntelliJ IDEA installed.

## Actions

### Code

- Reformat Code
- Optimize Imports
- Show Intentions
- Generate Code

### Refactor

- Rename Symbol
- Extract Method
- Extract Variable
- Extract Field
- Inline
- Change Signature
- Move
- Safe Delete

### Navigation

- Recent Files
- Recent Locations
- Go to Class
- Go to File
- Go to Symbol
- Go to Declaration
- Go to Implementation
- Navigate Back
- Navigate Forward
- File Structure
- Quick Documentation

### Build & Run

- Run
- Debug
- Stop
- Build Project

### Debug

- Toggle Breakpoint
- Evaluate Expression
- Step Over
- Step Into
- Resume Program

### Git

- Commit
- Push
- Update Project / Pull
- VCS Operations

### Maven

- `mvn clean install`

### Tools

- Terminal
- Project Window

## Intentionally Not Included

These actions are intentionally not shipped in this version:

- Find Action
- Search Everywhere
- Run Anything as a standalone Action Ring button
- Show Diff

They were unreliable in this setup through Action Ring. Some depended on keyboard layout, some opened the wrong UI, and `Show Diff` conflicted with editor behavior for `Cmd+D`.

## Install

Use the packaged plugin file:

```text
build/IntelliJIdeaActionRing_1_0.lplug4
```

Open it with Logi Plugin Service / Logi Options+ and then assign the actions to your MX Master 4 Action Ring.

If you are developing the plugin locally, `dotnet build` can also create a development link:

```text
~/Library/Application Support/Logi/LogiPluginService/Plugins/IntelliJIdeaPlugin.link
```

That link points to:

```text
build/bin/Release
```

## macOS Permissions

For the Maven Run Anything action, macOS may ask for Accessibility or Automation permissions.

Check:

- System Settings -> Privacy & Security -> Accessibility
- System Settings -> Privacy & Security -> Automation

The apps that may need permission are:

- Logi Plugin Service;
- Logi Options+;
- IntelliJ IDEA.

## Developer Overview

The project is a C# Logi Actions SDK plugin.

```text
src/
  Actions/                    Plugin actions shown in Logi Options+
  Helpers/                    Platform submitters, haptics, resources, logging
  package/
    metadata/                 LoupedeckPackage.yaml and icon
    events/                   Haptic event metadata and mappings
tests/
  IntelliJIdeaPlugin.Tests/   Console-based unit tests without NuGet dependencies
build/
  build.sh                    Build, test, package, and verify script
  IntelliJIdeaActionRing_1_0.lplug4
  .dotnet/                    Project-local .NET SDK
  .tools/                     Project-local LogiPluginTool
  dotnet-install.sh           Local SDK installer
```

## Architecture

### Action Layer

Most actions inherit from `ShortcutCommand`. They activate IntelliJ IDEA and send one stable IDE shortcut through the Logi SDK.

`MavenCleanInstallCommand` is the only custom action. It submits `mvn clean install` to IDEA Run Anything through `MavenRunAnythingSubmitter`.

### Platform Layer

`MavenRunAnythingSubmitter` selects the platform at runtime:

- macOS: `MavenRunAnythingScriptBuilder` builds AppleScript for System Events.
- Windows: `WindowsKeyboard` sends double Control, Unicode text, and Enter through Win32 `SendInput`.

### Package Layer

Logi metadata lives under:

```text
src/package
```

Important files:

- `metadata/LoupedeckPackage.yaml`
- `events/DefaultEventSource.yaml`
- `events/extra/eventMapping.yaml`

## Dependencies

Required to build:

- .NET 8 SDK
- Logi Plugin Service with `PluginApi.dll`
- LogiPluginTool

This workspace uses project-local tooling:

- `build/.dotnet/`
- `build/.tools/`

The tests do not use NuGet packages and do not require internet access.

## Build

Build without creating or updating the Logi Plugin Service development link:

```bash
DOTNET_CLI_HOME="$PWD/build/.dotnet-home" \
DOTNET_ROOT="$PWD/build/.dotnet" \
PATH="$PWD/build/.dotnet:$PATH" \
./build/.dotnet/dotnet build src/IntelliJIdeaPlugin.csproj -c Release /p:CreatePluginLink=false
```

Build and refresh the development link:

```bash
DOTNET_CLI_HOME="$PWD/build/.dotnet-home" \
DOTNET_ROOT="$PWD/build/.dotnet" \
PATH="$PWD/build/.dotnet:$PATH" \
./build/.dotnet/dotnet build src/IntelliJIdeaPlugin.csproj -c Release
```

## Test

Run the unit tests:

```bash
DOTNET_CLI_HOME="$PWD/build/.dotnet-home" \
DOTNET_ROOT="$PWD/build/.dotnet" \
PATH="$PWD/build/.dotnet:$PATH" \
./build/.dotnet/dotnet run --project tests/IntelliJIdeaPlugin.Tests/IntelliJIdeaPlugin.Tests.csproj -c Release
```

The tests cover the current behavior:

- macOS Run Anything AppleScript;
- no clipboard or paste usage;
- AppleScript string escaping;
- current action catalog;
- action construction;
- shortcut action inheritance;
- action group names;
- haptic event YAML consistency;
- README and license metadata.

## Package

Build the `.lplug4` file:

```bash
DOTNET_ROOT="$PWD/build/.dotnet" \
PATH="$PWD/build/.dotnet:$PATH" \
./build/.tools/logiplugintool pack "$PWD/build/bin/Release" "$PWD/build/IntelliJIdeaActionRing_1_0.lplug4"
```

Verify the package:

```bash
DOTNET_ROOT="$PWD/build/.dotnet" \
PATH="$PWD/build/.dotnet:$PATH" \
./build/.tools/logiplugintool verify "$PWD/build/IntelliJIdeaActionRing_1_0.lplug4"
```

Or run the full local gate with one command:

```bash
./build/build.sh
```

## Merge Rules for `master`

Before merging to `master`:

- build the plugin in Release mode;
- run all unit tests;
- package the `.lplug4`;
- run `logiplugintool verify`;
- update README when behavior, actions, platform support, or install steps change;
- do not merge untested changes to shortcuts, haptic events, or platform automation;
- keep generated build outputs and local tool folders out of git.

Recommended local gate:

```bash
./build/build.sh
```

## Haptics

Current haptic mappings:

- `actionStarted` -> `subtle_collision`
- `actionTriggered` -> `sharp_state_change`
- `mavenStarted` -> `wave`
- `mavenSubmitted` -> `happy_alert`
- `mavenFailed` -> `angry_alert`
- `mavenAlreadyRunning` -> `damp_collision`

## Troubleshooting

### Old actions are still visible

Restart Logi Options+ or Logi Plugin Service. The service can cache old action lists.

### Maven does not open Run Anything

Check that double Control opens Run Anything in IntelliJ IDEA. If your keymap changes that shortcut, update the platform submitter.

### macOS blocks automation

Grant Accessibility and Automation permissions to Logi Plugin Service.

### Windows build works but runtime does not

Windows support is build-ready but needs validation on a Windows machine. Check that IntelliJ IDEA can be activated by Logi Plugin Service and that double Control opens Run Anything.
