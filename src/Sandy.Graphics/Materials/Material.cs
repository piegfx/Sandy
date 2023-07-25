using Pie;

namespace Sandy.Graphics.Materials;

public sealed class Material
{
    public Texture2D Albedo;
    
    public Texture2D Normal;
    
    public Texture2D Metallic;
    
    public Texture2D Roughness;
    
    public Texture2D AmbientOcclusion;

    public PrimitiveType PrimitiveType;

    public RasterizerState RasterizerState;

    public Material(Texture2D albedo, Texture2D normal = null, Texture2D metallic = null, Texture2D roughness = null,
        Texture2D ambientOcclusion = null)
    {
        Albedo = albedo;
        Normal = normal ?? Texture2D.EmptyNormal;
        Metallic = metallic ?? Texture2D.Black;
        Roughness = roughness ?? Texture2D.White;
        AmbientOcclusion = ambientOcclusion ?? Texture2D.White;

        PrimitiveType = PrimitiveType.TriangleList;
        RasterizerState = RasterizerState.CullCounterClockwise;
    }
}