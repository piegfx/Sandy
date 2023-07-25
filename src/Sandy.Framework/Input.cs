using System.Collections.Generic;
using System.Numerics;
using Pie.Windowing;

namespace Sandy.Framework;

public class Input
{
    internal HashSet<Key> KeysDown;
    internal HashSet<Key> PerFrameKeys;

    internal HashSet<MouseButton> ButtonsDown;
    internal HashSet<MouseButton> PerFrameButtons;
    
    public Vector2 MousePosition { get; internal set; }
    
    public Vector2 MouseDelta { get; internal set; }

    internal Input()
    {
        KeysDown = new HashSet<Key>();
        PerFrameKeys = new HashSet<Key>();
        ButtonsDown = new HashSet<MouseButton>();
        PerFrameButtons = new HashSet<MouseButton>();
    }

    public bool IsKeyDown(Key key) => KeysDown.Contains(key);

    public bool IsKeyPressed(Key key) => PerFrameKeys.Contains(key);

    public bool IsMouseButtonDown(MouseButton button) => ButtonsDown.Contains(button);

    public bool IsMouseButtonPressed(MouseButton button) => PerFrameButtons.Contains(button);
}