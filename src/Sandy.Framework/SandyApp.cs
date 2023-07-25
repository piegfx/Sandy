using Pie.Windowing;
using Sandy.Graphics;

namespace Sandy.Framework;

public abstract class SandyApp : IDisposable
{
    public Window Window;

    public Renderer Renderer;
    
    public static SandyApp Instance { get; private set; }
}