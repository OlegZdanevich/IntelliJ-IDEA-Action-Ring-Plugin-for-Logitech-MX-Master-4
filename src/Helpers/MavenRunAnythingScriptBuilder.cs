namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;

    internal static class MavenRunAnythingScriptBuilder
    {
        public const String CommandText = "mvn clean install";
        public const String IntelliJBundleId = "com.jetbrains.intellij";

        public static String BuildMacScript() =>
            $"tell application id {QuoteAppleScript(IntelliJBundleId)} to activate\n" +
            "delay 0.25\n" +
            "tell application \"System Events\"\n" +
            "  key down control\n" +
            "  key up control\n" +
            "  delay 0.10\n" +
            "  key down control\n" +
            "  key up control\n" +
            "  delay 0.35\n" +
            $"  keystroke {QuoteAppleScript(CommandText)}\n" +
            "  delay 0.10\n" +
            "  key code 36\n" +
            "end tell\n";

        public static String QuoteAppleScript(String value) =>
            "\"" + value.Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal) + "\"";
    }
}
