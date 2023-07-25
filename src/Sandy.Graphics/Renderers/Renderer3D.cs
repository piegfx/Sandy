using System;
using System.Numerics;
using Sandy.Graphics.Structs;
using Sandy.Math;

namespace Sandy.Graphics.Renderers;

public abstract class Renderer3D : IDisposable
{
    public abstract RenderTarget2D MainTarget { get; protected set; }

    internal abstract void BeginPass(in CameraInfo cameraInfo);

    internal abstract void EndPass();

    internal abstract void Draw(Renderable renderable, in Matrix4x4 worldMatrix);

    internal abstract void Resize(Size<int> newSize);

    public abstract (string, Texture2D)[] GetRenderPassTextures();

    public abstract void Dispose();
}