using System;
using System.Diagnostics;
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

        Window = builder.Build(out GraphicsDevice gd);

        Renderer = new Renderer(gd);
        
        Initialize();
        
        Stopwatch deltaWatch = Stopwatch.StartNew();
        Stopwatch totalWatch = Stopwatch.StartNew();

        while (!_wantsClose)
        {
            while (Window.PollEvent(out IWindowEvent winEvent))
            {
                switch (winEvent)
                {
                    case QuitEvent:
                        _wantsClose = true;
                        break;
                }
            }

            Time time = new Time(deltaWatch.Elapsed, totalWatch.Elapsed);
            
            Update(time);
            Draw(time);
            
            Renderer.Present();
        }
    }

    public void Close()
    {
        _wantsClose = true;
    }

    protected virtual void Initialize() { }

    protected virtual void Update(Time time) { }

    protected virtual void Draw(Time time) { }

    public void Dispose()
    {
        Renderer.Dispose();
        Window.Dispose();
        
        Instance = null;
    }

    public static SandyApp Instance { get; private set; }
}