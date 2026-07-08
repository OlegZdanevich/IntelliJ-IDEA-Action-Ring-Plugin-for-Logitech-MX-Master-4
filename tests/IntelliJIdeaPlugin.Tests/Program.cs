namespace IntelliJIdeaPlugin.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Loupedeck.IntelliJIdeaPlugin;

    internal static class Program
    {
        public static Int32 Main()
        {
            var tests = new (String Name, Action Body)[]
            {
                ("Run Anything script activates IntelliJ by bundle id", RunAnythingScriptActivatesIntelliJ),
                ("Run Anything script opens palette with double Control", RunAnythingScriptUsesDoubleControl),
                ("Run Anything script submits Maven command", RunAnythingScriptSubmitsMavenCommand),
                ("Run Anything script does not touch clipboard or paste", RunAnythingScriptDoesNotTouchClipboard),
                ("AppleScript quoting escapes quotes and backslashes", AppleScriptQuotingEscapesSpecialCharacters),
                ("Windows virtual keys match Run Anything flow", WindowsVirtualKeysMatchRunAnythingFlow),
                ("Concrete action catalog matches expected actions", ConcreteActionCatalogMatchesExpectedActions),
                ("All concrete action classes can be constructed", AllConcreteActionClassesCanBeConstructed),
                ("Shortcut actions use the shortcut base class", ShortcutActionsUseShortcutBaseClass),
                ("Action groups use stable IntelliJ folders", ActionGroupsUseStableIntelliJFolders),
                ("Haptic YAML matches event constants", HapticYamlMatchesEventConstants),
                ("Package metadata documents product identity", PackageMetadataDocumentsProductIdentity),
                ("MIT license is present", MitLicenseIsPresent),
                ("README documents current behavior", ReadmeDocumentsCurrentBehavior),
            };

            var failures = new List<String>();

            foreach (var test in tests)
            {
                try
                {
                    test.Body();
                    Console.WriteLine($"PASS {test.Name}");
                }
                catch (Exception ex)
                {
                    failures.Add($"{test.Name}: {ex.Message}");
                    Console.WriteLine($"FAIL {test.Name}");
                    Console.WriteLine($"     {ex.Message}");
                }
            }

            if (failures.Count == 0)
            {
                Console.WriteLine($"{tests.Length} tests passed.");
                return 0;
            }

            Console.WriteLine($"{failures.Count} test(s) failed.");
            return 1;
        }

        private static void RunAnythingScriptActivatesIntelliJ()
        {
            var script = MavenRunAnythingScriptBuilder.BuildMacScript();

            AssertContains(script, "tell application id \"com.jetbrains.intellij\" to activate");
        }

        private static void RunAnythingScriptUsesDoubleControl()
        {
            var script = MavenRunAnythingScriptBuilder.BuildMacScript();

            AssertEqual(2, CountOccurrences(script, "key down control"), "Expected exactly two Control key-down events.");
            AssertEqual(2, CountOccurrences(script, "key up control"), "Expected exactly two Control key-up events.");
        }

        private static void RunAnythingScriptSubmitsMavenCommand()
        {
            var script = MavenRunAnythingScriptBuilder.BuildMacScript();

            AssertContains(script, "keystroke \"mvn clean install\"");
            AssertContains(script, "key code 36");
        }

        private static void RunAnythingScriptDoesNotTouchClipboard()
        {
            var script = MavenRunAnythingScriptBuilder.BuildMacScript();

            AssertDoesNotContain(script, "clipboard");
            AssertDoesNotContain(script, "key code 9");
            AssertDoesNotContain(script, "command down");
            AssertDoesNotContain(script, "pbcopy");
            AssertDoesNotContain(script, "pbpaste");
        }

        private static void AppleScriptQuotingEscapesSpecialCharacters()
        {
            var quoted = MavenRunAnythingScriptBuilder.QuoteAppleScript("a \"b\" c\\d");

            AssertEqual("\"a \\\"b\\\" c\\\\d\"", quoted, "AppleScript quoted string was not escaped correctly.");
        }

        private static void WindowsVirtualKeysMatchRunAnythingFlow()
        {
            AssertEqual((UInt16)0x11, WindowsVirtualKey.Control, "Windows Control virtual key changed.");
            AssertEqual((UInt16)0x0D, WindowsVirtualKey.Return, "Windows Return virtual key changed.");
            AssertEqual("mvn clean install", MavenRunAnythingScriptBuilder.CommandText);
        }

        private static void ConcreteActionCatalogMatchesExpectedActions()
        {
            var actual = GetConcreteActionTypes()
                .Select(type => type.Name)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            var expected = new[]
            {
                "BuildProjectCommand",
                "ChangeSignatureCommand",
                "CommitCommand",
                "DebugCommand",
                "EvaluateExpressionCommand",
                "ExtractFieldCommand",
                "ExtractMethodCommand",
                "ExtractVariableCommand",
                "FileStructureCommand",
                "GenerateCodeCommand",
                "GoToClassCommand",
                "GoToDeclarationCommand",
                "GoToFileCommand",
                "GoToImplementationCommand",
                "GoToSymbolCommand",
                "InlineCommand",
                "MavenCleanInstallCommand",
                "MoveCommand",
                "NavigateBackCommand",
                "NavigateForwardCommand",
                "OpenTerminalCommand",
                "OptimizeImportsCommand",
                "ProjectWindowCommand",
                "PullCommand",
                "PushCommand",
                "QuickDocumentationCommand",
                "RecentFilesCommand",
                "RecentLocationsCommand",
                "ReformatCodeCommand",
                "RenameSymbolCommand",
                "ResumeProgramCommand",
                "RunCommand",
                "SafeDeleteCommand",
                "ShowIntentionsCommand",
                "StepIntoCommand",
                "StepOverCommand",
                "StopCommand",
                "ToggleBreakpointCommand",
                "VcsOperationsCommand",
            }.OrderBy(name => name, StringComparer.Ordinal).ToArray();

            AssertSequenceEqual(expected, actual, "Concrete action catalog changed unexpectedly.");
        }

        private static void AllConcreteActionClassesCanBeConstructed()
        {
            foreach (var actionType in GetConcreteActionTypes())
            {
                var instance = Activator.CreateInstance(actionType);
                if (instance == null)
                {
                    throw new InvalidOperationException($"{actionType.Name} did not create an instance.");
                }
            }
        }

        private static void ShortcutActionsUseShortcutBaseClass()
        {
            var nonShortcutActions = GetConcreteActionTypes()
                .Where(type => type != typeof(MavenCleanInstallCommand))
                .Where(type => !type.IsSubclassOf(typeof(ShortcutCommand)))
                .Select(type => type.Name)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            AssertSequenceEqual(Array.Empty<String>(), nonShortcutActions, "Only Maven should use custom non-shortcut behavior.");
        }

        private static void ActionGroupsUseStableIntelliJFolders()
        {
            AssertEqual("IntelliJ IDEA###Build & Run", Groups.BuildRun);
            AssertEqual("IntelliJ IDEA###Code", Groups.Code);
            AssertEqual("IntelliJ IDEA###Debug", Groups.Debug);
            AssertEqual("IntelliJ IDEA###Git", Groups.Git);
            AssertEqual("IntelliJ IDEA###Maven", Groups.Maven);
            AssertEqual("IntelliJ IDEA###Navigation", Groups.Navigation);
            AssertEqual("IntelliJ IDEA###Refactor", Groups.Refactor);
            AssertEqual("IntelliJ IDEA###Tools", Groups.Tools);
        }

        private static void HapticYamlMatchesEventConstants()
        {
            var root = FindRepositoryRoot();
            var sourceRoot = FindSourceRoot(root);
            var eventSource = File.ReadAllText(Path.Combine(sourceRoot, "package", "events", "DefaultEventSource.yaml"));
            var eventMapping = File.ReadAllText(Path.Combine(sourceRoot, "package", "events", "extra", "eventMapping.yaml"));
            var expected = CurrentHapticEventNames().OrderBy(name => name, StringComparer.Ordinal).ToArray();

            var sourceNames = eventSource
                .Split('\n')
                .Select(line => line.Trim())
                .Where(line => line.StartsWith("- name: ", StringComparison.Ordinal))
                .Select(line => line["- name: ".Length..].Trim())
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            var mappingNames = eventMapping
                .Split('\n')
                .Where(line => line.StartsWith("  ", StringComparison.Ordinal) && !line.StartsWith("    ", StringComparison.Ordinal))
                .Select(line => line.Trim())
                .Where(line => line.EndsWith(":", StringComparison.Ordinal))
                .Select(line => line.TrimEnd(':'))
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            AssertSequenceEqual(expected, sourceNames, "DefaultEventSource.yaml event list does not match current constants.");
            AssertSequenceEqual(expected, mappingNames, "eventMapping.yaml haptic list does not match current constants.");

            foreach (var eventName in expected)
            {
                AssertContains(eventSource, $"name: {eventName}");
                AssertContains(eventMapping, $"{eventName}:");
            }
        }

        private static void ReadmeDocumentsCurrentBehavior()
        {
            var root = FindRepositoryRoot();
            var readme = File.ReadAllText(Path.Combine(root, "README.md"));

            AssertContains(readme, "IntelliJ IDEA Action Ring Plugin for Logitech MX Master 4");
            AssertContains(readme, "Run Anything");
            AssertContains(readme, "does not touch the clipboard");
            AssertContains(readme, "does not know the final Maven exit code");
            AssertContains(readme, "Windows support is build-ready");
            AssertContains(readme, "Merge Rules for `master`");
            AssertContains(readme, "src/");
            AssertContains(readme, "build/build.sh");
            AssertContains(readme, "build/IntelliJIdeaActionRing_1_0.lplug4");
            AssertContains(readme, "MIT");
        }

        private static void PackageMetadataDocumentsProductIdentity()
        {
            var root = FindRepositoryRoot();
            var metadata = File.ReadAllText(Path.Combine(FindSourceRoot(root), "package", "metadata", "LoupedeckPackage.yaml"));

            AssertContains(metadata, "displayName: IntelliJ IDEA Action Ring");
            AssertContains(metadata, "author: Oleg Zdanevich");
            AssertContains(metadata, "license: MIT");
            AssertContains(metadata, "pluginFolderWin: bin");
            AssertContains(metadata, "pluginFolderMac: bin");
            AssertContains(metadata, "Logitech Action Ring plugin");
        }

        private static void MitLicenseIsPresent()
        {
            var root = FindRepositoryRoot();
            var license = File.ReadAllText(Path.Combine(root, "LICENSE"));

            AssertContains(license, "MIT License");
            AssertContains(license, "Copyright (c) 2026 Oleg Zdanevich");
            AssertContains(license, "Permission is hereby granted, free of charge");
            AssertContains(license, "THE SOFTWARE IS PROVIDED \"AS IS\"");
        }

        private static Type[] GetConcreteActionTypes() =>
            typeof(ReformatCodeCommand)
                .Assembly
                .GetTypes()
                .Where(type => !type.IsAbstract)
                .Where(IsPluginDynamicCommand)
                .OrderBy(type => type.Name, StringComparer.Ordinal)
                .ToArray();

        private static Boolean IsPluginDynamicCommand(Type type)
        {
            for (var current = type.BaseType; current != null; current = current.BaseType)
            {
                if (current.FullName == "Loupedeck.PluginDynamicCommand")
                {
                    return true;
                }
            }

            return false;
        }

        private static String[] CurrentHapticEventNames() =>
            new[]
            {
                IntelliJHapticEvents.ActionStarted,
                IntelliJHapticEvents.ActionTriggered,
                IntelliJHapticEvents.MavenAlreadyRunning,
                IntelliJHapticEvents.MavenFailed,
                IntelliJHapticEvents.MavenStarted,
                IntelliJHapticEvents.MavenSubmitted,
            };

        private static String FindRepositoryRoot()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "README.md")) &&
                    Directory.Exists(Path.Combine(directory.FullName, "src")) &&
                    Directory.Exists(Path.Combine(directory.FullName, "build")))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            throw new InvalidOperationException("Could not find repository root.");
        }

        private static String FindSourceRoot(String repositoryRoot) =>
            Path.Combine(repositoryRoot, "src");

        private static Int32 CountOccurrences(String text, String value)
        {
            var count = 0;
            var index = 0;

            while ((index = text.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += value.Length;
            }

            return count;
        }

        private static void AssertContains(String text, String expected)
        {
            if (!text.Contains(expected, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Expected to find '{expected}'.");
            }
        }

        private static void AssertDoesNotContain(String text, String unexpected)
        {
            if (text.Contains(unexpected, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Did not expect to find '{unexpected}'.");
            }
        }

        private static void AssertEqual<T>(T expected, T actual, String message = null)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                throw new InvalidOperationException(message ?? $"Expected '{expected}', got '{actual}'.");
            }
        }

        private static void AssertSequenceEqual(String[] expected, String[] actual, String message)
        {
            if (!expected.SequenceEqual(actual, StringComparer.Ordinal))
            {
                throw new InvalidOperationException(
                    $"{message}{Environment.NewLine}Expected: {String.Join(", ", expected)}{Environment.NewLine}Actual:   {String.Join(", ", actual)}");
            }
        }
    }
}
