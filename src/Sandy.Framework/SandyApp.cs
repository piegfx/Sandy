using System;
using System.Diagnostics;
using System.Numerics;
using Pie;
using Pie.Windowing;
using Pie.Windowing.Events;
using Sandy.Graphics;
using Sandy.Graphics.Exceptions;

namespace Sandy.Framework;

public abstract class SandyApp : IDisposable
{
    private AppOptions _options;
    private bool _wantsClose;
    private Input _input;

    public Window Window;

    public Renderer Renderer;

    public SandyApp(AppOptions options)
    {
        _options = options;

        if (Instance != null)
        {
            throw new MultipleInstanceException(
                "Sandy does not currently support multiple applications running in a single process, sorry!");
        }

        Instance = this;
    }

    public void Run()
    {
        WindowBuilder builder = new WindowBuilder()
            .Size(_options.Size.Width, _options.Size.Height)
            .Title(_options.Title)
            .FullscreenMode(_options.FullscreenMode);

        if (_options.Resizable)
            builder.Resizable();

        Window = new Window(builder.Build(out GraphicsDevice gd));
        Window.Quit += Close;
        Window.KeyDown += WindowOnKeyDown;
        Window.KeyUp += WindowOnKeyUp;
        Window.MouseButtonDown += WindowOnMouseButtonDown;
        Window.MouseButtonUp += WindowOnMouseButtonUp;
        Window.MouseMove += WindowOnMouseMove;

        Renderer = new Renderer(gd);

        Initialize();
        
        Stopwatch deltaWatch = Stopwatch.StartNew();
        Stopwatch totalWatch = Stopwatch.StartNew();
        _input = new Input();

        while (!_wantsClose)
        {
            _input.PerFrameKeys.Clear();
            _input.PerFrameButtons.Clear();
            _input.MouseDelta = Vector2.Zero;
            Window.ProcessEvents();

            Time time = new Time(deltaWatch.Elapsed, totalWatch.Elapsed);

            deltaWatch.Restart();
            
            Update(time, _input);
            Draw(time, _input);

            Renderer.Present();
        }
    }

    public void Close()
    {
        _wantsClose = true;
    }

    protected virtual void Initialize() { }

    protected virtual void Update(Time time, Input input) { }

    protected virtual void Draw(Time time, Input input) { }

    public virtual void Dispose()
    {
        Renderer.Dispose();
        Window.Dispose();
        
        Instance = null;
    }
    
    private void WindowOnKeyDown(Key key)
    {
        _input.KeysDown.Add(key);
        _input.PerFrameKeys.Add(key);
    }
    
    private void WindowOnKeyUp(Key key)
    {
        _input.KeysDown.Remove(key);
        _input.PerFrameKeys.Remove(key);
    }
    
    private void WindowOnMouseButtonDown(MouseButton button)
    {
        _input.ButtonsDown.Add(button);
        _input.PerFrameButtons.Add(button);
    }
    
    private void WindowOnMouseButtonUp(MouseButton button)
    {
        _input.ButtonsDown.Remove(button);
        _input.PerFrameButtons.Remove(button);
    }
    
    private void WindowOnMouseMove(Vector2 position, Vector2 delta)
    {
        _input.MousePosition = position;
        _input.MouseDelta += delta;
    }

    public static SandyApp Instance { get; private set; }
}