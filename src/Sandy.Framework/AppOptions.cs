using System.Reflection;
using Pie.Windowing;
using Sandy.Math;

namespace Sandy.Framework;

public struct AppOptions
{
    public Size<int> Size;

    public string Title;

    public FullscreenMode FullscreenMode;

    public AppOptions()
    {
        Size = new Size<int>(1280, 720);
        Title = Assembly.GetEntryAssembly()?.GetName().Name ?? "Sandy Application";
        FullscreenMode = FullscreenMode.Windowed;
    }

    public AppOptions(Size<int>? size = null, string? title = null, FullscreenMode? fullscreenMode = null) : this()
    {
        Size = size ?? Size;
        Title = title ?? Title;
        FullscreenMode = fullscreenMode ?? FullscreenMode;
    }
}