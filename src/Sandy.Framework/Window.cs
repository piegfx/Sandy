using System;
using System.Drawing;
using System.Numerics;
using Pie.Windowing;
using Pie.Windowing.Events;
using Sandy.Math;

namespace Sandy.Framework;

public class Window : IDisposable
{
    internal Pie.Windowing.Window PieWindow;

    public event OnQuit Quit;

    public event OnResize Resize;

    public event OnKeyDown KeyDown;

    public event OnKeyUp KeyUp;

    public event OnMouseButtonDown MouseButtonDown;

    public event OnMouseButtonUp MouseButtonUp;

    public event OnMouseMove MouseMove;

    public event OnMouseScroll MouseScroll;

    public event OnTextInput TextInput;

    public Size<int> Size
    {
        get => (Size<int>) PieWindow.Size;
        set => PieWindow.Size = (System.Drawing.Size) value;
    }
    
    public Size<int> FramebufferSize => (Size<int>) PieWindow.FramebufferSize;

    public Vector2T<int> Position
    {
        get
        {
            Point pos = PieWindow.Position;
            return new Vector2T<int>(pos.X, pos.Y);
        }

        set => PieWindow.Position = new Point(value.X, value.Y);
    }

    public string Title
    {
        get => PieWindow.Title;
        set => PieWindow.Title = value;
    }

    public FullscreenMode FullscreenMode
    {
        get => PieWindow.FullscreenMode;
        set => PieWindow.FullscreenMode = value;
    }

    public bool Resizable
    {
        get => PieWindow.Resizable;
        set => PieWindow.Resizable = value;
    }

    public bool Borderless
    {
        get => PieWindow.Borderless;
        set => PieWindow.Borderless = value;
    }

    public bool Visible
    {
        get => PieWindow.Visible;
        set => PieWindow.Visible = value;
    }

    public bool Focused => PieWindow.Focused;

    internal Window(Pie.Windowing.Window pieWindow)
    {
        PieWindow = pieWindow;
        
        Quit = delegate { };
        Resize = delegate { };
        KeyDown = delegate { };
        KeyUp = delegate { };
        MouseButtonDown = delegate { };
        MouseButtonUp = delegate { };
        MouseMove = delegate { };
        MouseScroll = delegate { };
        TextInput = delegate { };
    }

    internal void ProcessEvents()
    {
        while (PieWindow.PollEvent(out IWindowEvent winEvent))
        {
            switch (winEvent)
            {
                case QuitEvent:
                    Quit.Invoke();
                    break;
                
                case ResizeEvent resize:
                    Resize.Invoke(new Size<int>(resize.Width, resize.Height));
                    break;
                
                case KeyEvent key:
                    switch (key.EventType)
                    {
                        case WindowEventType.KeyDown:
                            KeyDown.Invoke(key.Key);
                            break;
                        
                        case WindowEventType.KeyUp:
                            KeyUp.Invoke(key.Key);
                            break;
                    }

                    break;
                
                case MouseButtonEvent button:
                    switch (button.EventType)
                    {
                        case WindowEventType.MouseButtonDown:
                            MouseButtonDown.Invoke(button.Button);
                            break;
                        
                        case WindowEventType.MouseButtonUp:
                            MouseButtonUp.Invoke(button.Button);
                            break;
                    }

                    break;
                
                case MouseMoveEvent move:
                    MouseMove.Invoke(new Vector2(move.MouseX, move.MouseY), new Vector2(move.DeltaX, move.DeltaY));
                    break;
                
                case MouseScrollEvent scroll:
                    MouseScroll.Invoke(new Vector2(scroll.X, scroll.Y));
                    break;
                
                case TextInputEvent text:
                    foreach (char c in text.Text)
                        TextInput.Invoke(c);
                    break;
            }
        }
    }

    public void Focus() => PieWindow.Focus();

    public void Center() => PieWindow.Center();

    public void Maximize() => PieWindow.Maximize();

    public void Minimize() => PieWindow.Minimize();

    public void Restore() => PieWindow.Restore();

    public void Dispose()
    {
        PieWindow.Dispose();
    }

    public delegate void OnQuit();

    public delegate void OnResize(Size<int> size);

    public delegate void OnKeyDown(Key key);

    public delegate void OnKeyUp(Key key);

    public delegate void OnMouseButtonDown(MouseButton button);

    public delegate void OnMouseButtonUp(MouseButton button);

    public delegate void OnMouseMove(Vector2 position, Vector2 delta);

    public delegate void OnMouseScroll(Vector2 delta);

    public delegate void OnTextInput(char c);
}