using Sandy.Graphics.Renderers.Structs;

namespace Sandy.Graphics.Lighting;

public interface ILight
{
    public LightInfo LightInfo { get; }
}