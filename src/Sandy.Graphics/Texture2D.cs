using System;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using Pie;
using Sandy.Math;

namespace Sandy.Graphics;

public class Texture2D : IDisposable
{
    public Pie.Texture PieTexture;

    public Size<int> Size
    {
        get
        {
            TextureDescription description = PieTexture.Description;
            return new Size<int>(description.Width, description.Height);
        }
    }

    public Texture2D(string path, bool generateMipmaps = true) : this(new Bitmap(path), generateMipmaps) { }

    public Texture2D(Bitmap bitmap, bool generateMipmaps = true) : this(bitmap.Size, bitmap.Data, bitmap.Format,
        generateMipmaps) { }

    public Texture2D(Size<int> size, byte[] data, Format format = Format.R8G8B8A8_UNorm, bool generateMipmaps = true) :
        this(
            new TextureDescription(TextureType.Texture2D, size.Width, size.Height, 0, format, generateMipmaps ? 0 : 1,
                1, TextureUsage.ShaderResource), data, generateMipmaps) { }

    public Texture2D(in TextureDescription description, byte[] data, bool generateMipmaps = true)
    {
        if (description.TextureType != TextureType.Texture2D)
            throw new InvalidOperationException("Texture type must be Texture2D.");
        if (description.ArraySize != 1)
        {
            throw new InvalidOperationException(
                "Texture array size must be 1. Texture arrays are not supported by Texture2D.");
        }

        GraphicsDevice device = Renderer.Instance.Device;

        PieTexture = device.CreateTexture(description, data);
        
        if (generateMipmaps)
            device.GenerateMipmaps(PieTexture);
    }

    public Texture2D(Pie.Texture pieTexture)
    {
        PieTexture = pieTexture;
    }

    public void Update(int x, int y, int width, int height, byte[] data)
    {
        GraphicsDevice device = Renderer.Instance.Device;
        
        device.UpdateTexture(PieTexture, 0, 0, x, y, 0, width, height, 0, data);
    }
    
    public virtual void Dispose()
    {
        PieTexture.Dispose();
    }

    public static readonly Texture2D White = new Texture2D(new Size<int>(1, 1), new byte[] { 255, 255, 255, 255 });

    public static readonly Texture2D Black = new Texture2D(new Size<int>(1, 1), new byte[] { 0, 0, 0, 255 });

    public static readonly Texture2D
        EmptyNormal = new Texture2D(new Size<int>(1, 1), new byte[] { 128, 128, 255, 255 });

    public static readonly Texture2D Debug =
        new Texture2D(new Bitmap(EmbeddedResource.Load(Assembly.GetExecutingAssembly(),
            "Sandcastle.Graphics.DEBUG.png")));
}