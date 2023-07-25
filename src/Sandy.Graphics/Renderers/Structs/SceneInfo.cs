using System.Runtime.InteropServices;

namespace Sandy.Graphics.Renderers.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct SceneInfo
{
    public LightInfo DirectionalLight;
    
    public float AmbientColor;

    private fixed float _padding[3];
}