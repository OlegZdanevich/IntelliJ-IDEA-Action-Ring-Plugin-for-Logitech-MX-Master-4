namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;

    // This class contains the plugin-level logic of the Loupedeck plugin.

    public class IntelliJIdeaPlugin : Plugin
    {
        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean UsesApplicationApiOnly => false;

        // Gets a value indicating whether this is a Universal plugin or an Application plugin.
        public override Boolean HasNoApplication => false;

        // Initializes a new instance of the plugin class.
        public IntelliJIdeaPlugin()
        {
            // Initialize the plugin log.
            PluginLog.Init(this.Log);

            // Initialize the plugin resources.
            PluginResources.Init(this.Assembly);
        }

        // This method is called when the plugin is loaded.
        public override void Load()
        {
            this.PluginEvents.AddEvent(
                IntelliJHapticEvents.ActionStarted,
                "IntelliJ action started",
                "Triggered before a shortcut command is sent to IntelliJ IDEA.");
            this.PluginEvents.AddEvent(
                IntelliJHapticEvents.ActionTriggered,
                "IntelliJ action triggered",
                "Triggered after an IntelliJ IDEA action is sent.");
            this.PluginEvents.AddEvent(
                IntelliJHapticEvents.MavenStarted,
                "Maven Run Anything opened",
                "Triggered when the plugin starts submitting mvn clean install to IntelliJ IDEA Run Anything.");
            this.PluginEvents.AddEvent(
                IntelliJHapticEvents.MavenSubmitted,
                "Maven command submitted",
                "Triggered after mvn clean install is submitted to IntelliJ IDEA Run Anything.");
            this.PluginEvents.AddEvent(
                IntelliJHapticEvents.MavenFailed,
                "Maven submission failed",
                "Triggered when the plugin cannot submit mvn clean install to IntelliJ IDEA Run Anything.");
            this.PluginEvents.AddEvent(
                IntelliJHapticEvents.MavenAlreadyRunning,
                "Maven submission already running",
                "Triggered when mvn clean install is requested while the plugin is already submitting it.");
        }

        // This method is called when the plugin is unloaded.
        public override void Unload()
        {
        }
    }
}
