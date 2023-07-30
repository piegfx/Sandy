using System.Reflection;
using Pie;
using Pie.Windowing;
using Sandy.Math;

namespace Sandy.Framework;

public struct AppOptions
{
    public Size<int> Size;

    public string Title;

    public FullscreenMode FullscreenMode;

    public bool Resizable;

    public Format ColorBufferFormat;

    public Format? DepthStencilBufferFormat;

    public AppOptions()
    {
        Size = new Size<int>(1280, 720);
        Title = Assembly.GetEntryAssembly()?.GetName().Name ?? "Sandy Application";
        FullscreenMode = FullscreenMode.Windowed;
        Resizable = false;

        ColorBufferFormat = Format.B8G8R8A8_UNorm;
        DepthStencilBufferFormat = null;
    }

    public AppOptions(Size<int>? size = null, string? title = null, FullscreenMode? fullscreenMode = null,
        bool? resizable = null, Format? colorBufferFormat = null, Format? depthStencilBufferFormat = null) : this()
    {
        Size = size ?? Size;
        Title = title ?? Title;
        FullscreenMode = fullscreenMode ?? FullscreenMode;
        Resizable = resizable ?? Resizable;
        ColorBufferFormat = colorBufferFormat ?? ColorBufferFormat;
        DepthStencilBufferFormat = depthStencilBufferFormat ?? DepthStencilBufferFormat;
    }
}