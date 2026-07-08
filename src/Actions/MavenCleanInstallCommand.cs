namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class MavenCleanInstallCommand : PluginDynamicCommand
    {
        private static Int32 _isRunning;
        private String _status = "Ready";

        public MavenCleanInstallCommand()
            : base("mvn clean install", "Submits mvn clean install through IntelliJ IDEA Run Anything", Groups.Maven)
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (Interlocked.Exchange(ref _isRunning, 1) == 1)
            {
                this._status = "Already running";
                this.ActionImageChanged();
                this.RaiseEvent(IntelliJHapticEvents.MavenAlreadyRunning);
                return;
            }

            this._status = "Opening IDEA";
            this.ActionImageChanged();
            this.RaiseEvent(IntelliJHapticEvents.MavenStarted);

            Task.Run(() => this.SubmitToIdeaRunAnything());
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            $"mvn clean install{Environment.NewLine}{this._status}";

        private void SubmitToIdeaRunAnything()
        {
            try
            {
                MavenRunAnythingSubmitter.Submit(this.Plugin);
                this._status = "Sent to IDEA";
                this.ActionImageChanged();
                this.RaiseEvent(IntelliJHapticEvents.MavenSubmitted);
                PluginLog.Info("Submitted 'mvn clean install' through IntelliJ IDEA Run Anything.");
            }
            catch (Exception ex)
            {
                this._status = "Error";
                this.ActionImageChanged();
                this.RaiseEvent(IntelliJHapticEvents.MavenFailed);
                PluginLog.Info($"Could not submit mvn clean install through IntelliJ IDEA Run Anything: {ex}");
            }
            finally
            {
                Interlocked.Exchange(ref _isRunning, 0);
            }
        }

        private void RaiseEvent(String eventName)
        {
            try
            {
                this.Plugin.PluginEvents.RaiseEvent(eventName);
            }
            catch (Exception ex)
            {
                PluginLog.Info($"Could not raise haptic event '{eventName}': {ex.Message}");
            }
        }
    }
}
