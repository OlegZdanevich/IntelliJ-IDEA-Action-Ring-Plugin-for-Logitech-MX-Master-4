namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    internal static class IntelliJActivator
    {
        public static void Activate(Plugin plugin)
        {
            try
            {
                plugin.ClientApplication.Activate();
            }
            catch (Exception ex)
            {
                PluginLog.Info($"Could not activate IntelliJ through ClientApplication: {ex.Message}");
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/osascript",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                startInfo.ArgumentList.Add("-e");
                startInfo.ArgumentList.Add("tell application id \"com.jetbrains.intellij\" to activate");

                using var process = Process.Start(startInfo);
                process.WaitForExit(1200);
            }
            catch (Exception ex)
            {
                PluginLog.Info($"Could not activate IntelliJ through AppleScript: {ex.Message}");
            }

            Thread.Sleep(120);
        }
    }
}
