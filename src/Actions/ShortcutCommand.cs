namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;
    public abstract class ShortcutCommand : PluginDynamicCommand
    {
        private readonly ModifierKey _modifierKeys;
        private readonly VirtualKeyCode _virtualKeyCode;

        protected ShortcutCommand(String displayName, VirtualKeyCode virtualKeyCode, ModifierKey modifierKeys, String description, String groupName)
            : base(displayName, description, groupName)
        {
            this._virtualKeyCode = virtualKeyCode;
            this._modifierKeys = modifierKeys;
        }

        protected override void RunCommand(String actionParameter)
        {
            this.RaiseEvent(IntelliJHapticEvents.ActionStarted);
            IntelliJActivator.Activate(this.Plugin);
            this.Plugin.ClientApplication.SendKeyboardShortcut(this._virtualKeyCode, this._modifierKeys);
            this.RaiseEvent(IntelliJHapticEvents.ActionTriggered);
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
