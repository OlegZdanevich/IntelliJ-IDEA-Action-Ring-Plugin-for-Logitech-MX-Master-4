namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;

    public sealed class ReformatCodeCommand : ShortcutCommand
    {
        public ReformatCodeCommand()
            : base("Reformat Code", VirtualKeyCode.KeyL, ModifierKey.Command | ModifierKey.Option, "Format the current file or selection", Groups.Code)
        {
        }
    }

    public sealed class OptimizeImportsCommand : ShortcutCommand
    {
        public OptimizeImportsCommand()
            : base("Optimize Imports", VirtualKeyCode.KeyO, ModifierKey.Control | ModifierKey.Option, "Optimize imports in the current file", Groups.Code)
        {
        }
    }

    public sealed class ShowIntentionsCommand : ShortcutCommand
    {
        public ShowIntentionsCommand()
            : base("Show Intentions", VirtualKeyCode.Return, ModifierKey.Option, "Show intention actions and quick fixes", Groups.Code)
        {
        }
    }

    public sealed class GenerateCodeCommand : ShortcutCommand
    {
        public GenerateCodeCommand()
            : base("Generate Code", VirtualKeyCode.KeyN, ModifierKey.Command, "Open IntelliJ code generation actions", Groups.Code)
        {
        }
    }

    public sealed class RenameSymbolCommand : ShortcutCommand
    {
        public RenameSymbolCommand()
            : base("Rename Symbol", VirtualKeyCode.F6, ModifierKey.Shift, "Rename the symbol under the caret", Groups.Refactor)
        {
        }
    }

    public sealed class ExtractMethodCommand : ShortcutCommand
    {
        public ExtractMethodCommand()
            : base("Extract Method", VirtualKeyCode.KeyM, ModifierKey.Command | ModifierKey.Option, "Extract selected code into a method", Groups.Refactor)
        {
        }
    }

    public sealed class ExtractVariableCommand : ShortcutCommand
    {
        public ExtractVariableCommand()
            : base("Extract Variable", VirtualKeyCode.KeyV, ModifierKey.Command | ModifierKey.Option, "Extract selected expression into a variable", Groups.Refactor)
        {
        }
    }

    public sealed class ExtractFieldCommand : ShortcutCommand
    {
        public ExtractFieldCommand()
            : base("Extract Field", VirtualKeyCode.KeyF, ModifierKey.Command | ModifierKey.Option, "Extract selected expression into a field", Groups.Refactor)
        {
        }
    }

    public sealed class InlineCommand : ShortcutCommand
    {
        public InlineCommand()
            : base("Inline", VirtualKeyCode.KeyN, ModifierKey.Command | ModifierKey.Option, "Inline a variable, method, or class", Groups.Refactor)
        {
        }
    }

    public sealed class ChangeSignatureCommand : ShortcutCommand
    {
        public ChangeSignatureCommand()
            : base("Change Signature", VirtualKeyCode.F6, ModifierKey.Command, "Change method or constructor signature", Groups.Refactor)
        {
        }
    }

    public sealed class MoveCommand : ShortcutCommand
    {
        public MoveCommand()
            : base("Move", VirtualKeyCode.F6, ModifierKey.None, "Move a class, file, or member", Groups.Refactor)
        {
        }
    }

    public sealed class SafeDeleteCommand : ShortcutCommand
    {
        public SafeDeleteCommand()
            : base("Safe Delete", VirtualKeyCode.Delete, ModifierKey.Command, "Safely delete the selected symbol", Groups.Refactor)
        {
        }
    }

    public sealed class RecentFilesCommand : ShortcutCommand
    {
        public RecentFilesCommand()
            : base("Recent Files", VirtualKeyCode.KeyE, ModifierKey.Command, "Open recent files", Groups.Navigation)
        {
        }
    }

    public sealed class RecentLocationsCommand : ShortcutCommand
    {
        public RecentLocationsCommand()
            : base("Recent Locations", VirtualKeyCode.KeyE, ModifierKey.Command | ModifierKey.Shift, "Open recent locations", Groups.Navigation)
        {
        }
    }

    public sealed class GoToClassCommand : ShortcutCommand
    {
        public GoToClassCommand()
            : base("Go to Class", VirtualKeyCode.KeyO, ModifierKey.Command, "Navigate to a class", Groups.Navigation)
        {
        }
    }

    public sealed class GoToFileCommand : ShortcutCommand
    {
        public GoToFileCommand()
            : base("Go to File", VirtualKeyCode.KeyO, ModifierKey.Command | ModifierKey.Shift, "Navigate to a file", Groups.Navigation)
        {
        }
    }

    public sealed class GoToSymbolCommand : ShortcutCommand
    {
        public GoToSymbolCommand()
            : base("Go to Symbol", VirtualKeyCode.KeyO, ModifierKey.Command | ModifierKey.Option, "Navigate to a symbol", Groups.Navigation)
        {
        }
    }

    public sealed class GoToDeclarationCommand : ShortcutCommand
    {
        public GoToDeclarationCommand()
            : base("Go to Declaration", VirtualKeyCode.KeyB, ModifierKey.Command, "Go to declaration or usages", Groups.Navigation)
        {
        }
    }

    public sealed class GoToImplementationCommand : ShortcutCommand
    {
        public GoToImplementationCommand()
            : base("Go to Implementation", VirtualKeyCode.KeyB, ModifierKey.Command | ModifierKey.Option, "Go to implementation", Groups.Navigation)
        {
        }
    }

    public sealed class NavigateBackCommand : ShortcutCommand
    {
        public NavigateBackCommand()
            : base("Navigate Back", VirtualKeyCode.ArrowLeft, ModifierKey.Command | ModifierKey.Option, "Navigate back", Groups.Navigation)
        {
        }
    }

    public sealed class NavigateForwardCommand : ShortcutCommand
    {
        public NavigateForwardCommand()
            : base("Navigate Forward", VirtualKeyCode.ArrowRight, ModifierKey.Command | ModifierKey.Option, "Navigate forward", Groups.Navigation)
        {
        }
    }

    public sealed class FileStructureCommand : ShortcutCommand
    {
        public FileStructureCommand()
            : base("File Structure", VirtualKeyCode.F12, ModifierKey.Command, "Open file structure popup", Groups.Navigation)
        {
        }
    }

    public sealed class QuickDocumentationCommand : ShortcutCommand
    {
        public QuickDocumentationCommand()
            : base("Quick Documentation", VirtualKeyCode.F1, ModifierKey.None, "Show quick documentation", Groups.Navigation)
        {
        }
    }

    public sealed class RunCommand : ShortcutCommand
    {
        public RunCommand()
            : base("Run", VirtualKeyCode.KeyR, ModifierKey.Control, "Run the selected configuration", Groups.BuildRun)
        {
        }
    }

    public sealed class DebugCommand : ShortcutCommand
    {
        public DebugCommand()
            : base("Debug", VirtualKeyCode.KeyD, ModifierKey.Control, "Debug the selected configuration", Groups.BuildRun)
        {
        }
    }

    public sealed class StopCommand : ShortcutCommand
    {
        public StopCommand()
            : base("Stop", VirtualKeyCode.F2, ModifierKey.Command, "Stop the running process", Groups.BuildRun)
        {
        }
    }

    public sealed class BuildProjectCommand : ShortcutCommand
    {
        public BuildProjectCommand()
            : base("Build Project", VirtualKeyCode.F9, ModifierKey.Command, "Build the IntelliJ project", Groups.BuildRun)
        {
        }
    }

    public sealed class ToggleBreakpointCommand : ShortcutCommand
    {
        public ToggleBreakpointCommand()
            : base("Toggle Breakpoint", VirtualKeyCode.F8, ModifierKey.Command, "Toggle line breakpoint", Groups.Debug)
        {
        }
    }

    public sealed class EvaluateExpressionCommand : ShortcutCommand
    {
        public EvaluateExpressionCommand()
            : base("Evaluate Expression", VirtualKeyCode.F8, ModifierKey.Option, "Evaluate expression while debugging", Groups.Debug)
        {
        }
    }

    public sealed class StepOverCommand : ShortcutCommand
    {
        public StepOverCommand()
            : base("Step Over", VirtualKeyCode.F8, ModifierKey.None, "Debugger step over", Groups.Debug)
        {
        }
    }

    public sealed class StepIntoCommand : ShortcutCommand
    {
        public StepIntoCommand()
            : base("Step Into", VirtualKeyCode.F7, ModifierKey.None, "Debugger step into", Groups.Debug)
        {
        }
    }

    public sealed class ResumeProgramCommand : ShortcutCommand
    {
        public ResumeProgramCommand()
            : base("Resume Program", VirtualKeyCode.KeyR, ModifierKey.Command | ModifierKey.Option, "Resume debugging", Groups.Debug)
        {
        }
    }

    public sealed class CommitCommand : ShortcutCommand
    {
        public CommitCommand()
            : base("Commit", VirtualKeyCode.KeyK, ModifierKey.Command, "Open commit tool window", Groups.Git)
        {
        }
    }

    public sealed class PushCommand : ShortcutCommand
    {
        public PushCommand()
            : base("Push", VirtualKeyCode.KeyK, ModifierKey.Command | ModifierKey.Shift, "Push commits", Groups.Git)
        {
        }
    }

    public sealed class PullCommand : ShortcutCommand
    {
        public PullCommand()
            : base("Update Project / Pull", VirtualKeyCode.KeyT, ModifierKey.Command, "Update project from VCS", Groups.Git)
        {
        }
    }

    public sealed class VcsOperationsCommand : ShortcutCommand
    {
        public VcsOperationsCommand()
            : base("VCS Operations", VirtualKeyCode.KeyV, ModifierKey.Control, "Open VCS operations popup", Groups.Git)
        {
        }
    }

    public sealed class OpenTerminalCommand : ShortcutCommand
    {
        public OpenTerminalCommand()
            : base("Terminal", VirtualKeyCode.F12, ModifierKey.Option, "Open IntelliJ Terminal tool window", Groups.Tools)
        {
        }
    }

    public sealed class ProjectWindowCommand : ShortcutCommand
    {
        public ProjectWindowCommand()
            : base("Project Window", VirtualKeyCode.Key1, ModifierKey.Command, "Open Project tool window", Groups.Tools)
        {
        }
    }

    internal static class Groups
    {
        public const String BuildRun = "IntelliJ IDEA###Build & Run";
        public const String Code = "IntelliJ IDEA###Code";
        public const String Debug = "IntelliJ IDEA###Debug";
        public const String Git = "IntelliJ IDEA###Git";
        public const String Maven = "IntelliJ IDEA###Maven";
        public const String Navigation = "IntelliJ IDEA###Navigation";
        public const String Refactor = "IntelliJ IDEA###Refactor";
        public const String Tools = "IntelliJ IDEA###Tools";
    }
}
