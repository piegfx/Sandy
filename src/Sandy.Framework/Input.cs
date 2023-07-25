using System.Collections.Generic;
using Pie.Windowing;

namespace Sandy.Framework;

public class Input
{
    internal HashSet<Key> KeysDown;
    internal HashSet<Key> PerFrameKeys;

    internal Input()
    {
        KeysDown = new HashSet<Key>();
        PerFrameKeys = new HashSet<Key>();
    }

    public bool IsKeyDown(Key key) => KeysDown.Contains(key);

    public bool IsKeyPressed(Key key) => PerFrameKeys.Contains(key);
}