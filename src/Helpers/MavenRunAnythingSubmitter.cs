namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal static class MavenRunAnythingSubmitter
    {
        public static void Submit(Plugin plugin)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                SubmitOnMac();
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SubmitOnWindows(plugin);
                return;
            }

            throw new PlatformNotSupportedException("Submitting Maven through IntelliJ IDEA Run Anything is supported on macOS and Windows only.");
        }

        private static void SubmitOnMac() => RunAppleScript(MavenRunAnythingScriptBuilder.BuildMacScript());

        private static void SubmitOnWindows(Plugin plugin)
        {
            try
            {
                plugin.ClientApplication.Activate();
            }
            catch (Exception ex)
            {
                PluginLog.Info($"Could not activate IntelliJ IDEA before Windows Run Anything submission: {ex.Message}");
            }

            Thread.Sleep(250);

            WindowsKeyboard.PressAndReleaseControl();
            Thread.Sleep(100);
            WindowsKeyboard.PressAndReleaseControl();
            Thread.Sleep(350);
            WindowsKeyboard.TypeUnicodeText(MavenRunAnythingScriptBuilder.CommandText);
            Thread.Sleep(100);
            WindowsKeyboard.PressAndReleaseKey(WindowsVirtualKey.Return);
        }

        private static void RunAppleScript(String script)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/osascript",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            startInfo.ArgumentList.Add("-e");
            startInfo.ArgumentList.Add(script);

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            var stdout = process.StandardOutput.ReadToEnd();
            var stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"osascript exited with code {process.ExitCode}. stdout: {stdout}; stderr: {stderr}");
            }
        }
    }
}
