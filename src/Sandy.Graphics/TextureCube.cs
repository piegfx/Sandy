using System;
using Pie;
using Sandy.Math;

namespace Sandy.Graphics;

public class TextureCube : IDisposable
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
    
    public TextureCube(Bitmap right, Bitmap left, Bitmap top, Bitmap bottom, Bitmap front, Bitmap back, bool generateMipmaps = true) : 
        this(right.Size, PieUtils.Combine(right.Data, left.Data, top.Data, bottom.Data, front.Data, back.Data), right.Format, generateMipmaps) { }
    
    public TextureCube(Size<int> size, byte[] packedData, Format format = Format.R8G8B8A8_UNorm, bool generateMipmaps = true) 
        : this(TextureDescription.Cubemap(size.Width, size.Height, format, generateMipmaps ? 0 : 1, TextureUsage.ShaderResource), packedData, generateMipmaps) { }
    
    public TextureCube(in TextureDescription description, byte[] data, bool generateMipmaps = true)
    {
        if (description.TextureType != TextureType.Cubemap)
            throw new InvalidOperationException("Texture type must be Cubemap.");
        if (description.ArraySize != 1)
        {
            throw new InvalidOperationException(
                "Texture array size must be 1. Texture arrays are not supported by TextureCube.");
        }

        GraphicsDevice device = Renderer.Instance.Device;

        PieTexture = device.CreateTexture(description, data);
        
        if (generateMipmaps)
            device.GenerateMipmaps(PieTexture);
    }

    public void Dispose()
    {
        PieTexture.Dispose();
    }
}