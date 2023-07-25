using Pie;
using Sandy.Math;

namespace Sandy.Graphics;

public class RenderTarget2D : Texture2D
{
    public Framebuffer PieFramebuffer;
    
    public Texture PieDepthTexture;
    
    public RenderTarget2D(Size<int> size, Format format = Format.R8G8B8A8_UNorm, Format? depthFormat = Format.D32_Float)
        : base(TextureDescription.Texture2D(size.Width, size.Height, format, 1, 1,
                TextureUsage.Framebuffer | TextureUsage.ShaderResource), null, false)
    {
        GraphicsDevice device = Renderer.Instance.Device;
        
        if (depthFormat != null)
        {
            PieDepthTexture = device.CreateTexture(TextureDescription.Texture2D(size.Width, size.Height,
                depthFormat.Value, 1, 1, TextureUsage.Framebuffer));

            PieFramebuffer = device.CreateFramebuffer(new FramebufferAttachment(PieTexture),
                new FramebufferAttachment(PieDepthTexture));
        }
        else
            PieFramebuffer = device.CreateFramebuffer(new FramebufferAttachment(PieTexture));
    }

    public RenderTarget2D(Framebuffer pieFramebuffer, Texture texture, Texture depthTexture) : base(texture)
    {
        PieFramebuffer = pieFramebuffer;
        PieDepthTexture = depthTexture;
    }

    public override void Dispose()
    {
        PieFramebuffer.Dispose();
        
        PieDepthTexture?.Dispose();
        
        base.Dispose();
    }
}