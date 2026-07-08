namespace Loupedeck.IntelliJIdeaPlugin
{
    using System;
    using System.Runtime.InteropServices;

    internal static class WindowsKeyboard
    {
        private const UInt32 InputKeyboard = 1;
        private const UInt32 KeyEventKeyUp = 0x0002;
        private const UInt32 KeyEventUnicode = 0x0004;

        public static void PressAndReleaseControl()
        {
            SendVirtualKey(WindowsVirtualKey.Control, keyUp: false);
            SendVirtualKey(WindowsVirtualKey.Control, keyUp: true);
        }

        public static void PressAndReleaseKey(UInt16 virtualKey)
        {
            SendVirtualKey(virtualKey, keyUp: false);
            SendVirtualKey(virtualKey, keyUp: true);
        }

        public static void TypeUnicodeText(String text)
        {
            foreach (var character in text)
            {
                SendUnicode(character, keyUp: false);
                SendUnicode(character, keyUp: true);
            }
        }

        private static void SendVirtualKey(UInt16 virtualKey, Boolean keyUp)
        {
            var input = Input.CreateVirtualKey(virtualKey, keyUp);
            SendInputChecked(input);
        }

        private static void SendUnicode(Char character, Boolean keyUp)
        {
            var input = Input.CreateUnicode(character, keyUp);
            SendInputChecked(input);
        }

        private static void SendInputChecked(Input input)
        {
            var sent = SendInput(1, new[] { input }, Marshal.SizeOf<Input>());
            if (sent != 1)
            {
                throw new InvalidOperationException($"SendInput failed with Win32 error {Marshal.GetLastWin32Error()}.");
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 SendInput(UInt32 inputCount, Input[] inputs, Int32 inputSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct Input
        {
            public UInt32 Type;
            public InputUnion Data;

            public static Input CreateVirtualKey(UInt16 virtualKey, Boolean keyUp) =>
                new()
                {
                    Type = InputKeyboard,
                    Data = new InputUnion
                    {
                        Keyboard = new KeyboardInput
                        {
                            VirtualKey = virtualKey,
                            ScanCode = 0,
                            Flags = keyUp ? KeyEventKeyUp : 0,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero,
                        },
                    },
                };

            public static Input CreateUnicode(Char character, Boolean keyUp) =>
                new()
                {
                    Type = InputKeyboard,
                    Data = new InputUnion
                    {
                        Keyboard = new KeyboardInput
                        {
                            VirtualKey = 0,
                            ScanCode = character,
                            Flags = KeyEventUnicode | (keyUp ? KeyEventKeyUp : 0),
                            Time = 0,
                            ExtraInfo = IntPtr.Zero,
                        },
                    },
                };
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public KeyboardInput Keyboard;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public UInt16 VirtualKey;
            public UInt16 ScanCode;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }
    }

    internal static class WindowsVirtualKey
    {
        public const UInt16 Control = 0x11;
        public const UInt16 Return = 0x0D;
    }
}
