namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;
    using System.IO;
    using System.Linq;

    // This class can be used to connect the Loupedeck plugin to an application.

    public class IntelliJIdeaApplication : ClientApplication
    {
        private static readonly String[] BundleNames =
        {
            "com.jetbrains.intellij",
            "com.jetbrains.intellij.ce",
        };

        private static readonly String[] ProcessNames =
        {
            "idea",
            "IntelliJ IDEA",
        };

        public IntelliJIdeaApplication()
        {
        }

        // This method can be used to link the plugin to a Windows application.
        protected override String GetProcessName() => "idea";

        protected override String[] GetProcessNames() => ProcessNames;

        // This method can be used to link the plugin to a macOS application.
        protected override String GetBundleName() => BundleNames[0];

        protected override String[] GetBundleNames() => BundleNames;

        // This method can be used to check whether the application is installed or not.
        public override ClientApplicationStatus GetApplicationStatus()
        {
            var homeApplications = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Applications");

            var appCandidates = new[]
            {
                "/Applications/IntelliJ IDEA.app",
                Path.Combine(homeApplications, "IntelliJ IDEA.app"),
            };

            return appCandidates.Any(Directory.Exists)
                ? ClientApplicationStatus.Installed
                : ClientApplicationStatus.NotInstalled;
        }
    }
}
