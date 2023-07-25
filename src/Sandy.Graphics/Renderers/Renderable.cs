using System;
using Pie;
using Sandy.Graphics.Materials;

namespace Sandy.Graphics.Renderers;

public class Renderable : IDisposable
{
    public GraphicsBuffer VertexBuffer;
    public GraphicsBuffer IndexBuffer;

    public uint NumIndices;

    public Material Material;

    public Renderable(VertexPositionTextureColorNormalTangent[] vertices, uint[] indices, Material material, bool dynamic = false)
    {
        GraphicsDevice device = Renderer.Instance.Device;

        VertexBuffer = device.CreateBuffer(BufferType.VertexBuffer, vertices, dynamic);
        IndexBuffer = device.CreateBuffer(BufferType.IndexBuffer, indices, dynamic);

        NumIndices = (uint) indices.Length;
        
        Material = material;
    }

    public Renderable(GraphicsBuffer vertexBuffer, GraphicsBuffer indexBuffer, uint numIndices, Material material)
    {
        VertexBuffer = vertexBuffer;
        IndexBuffer = indexBuffer;
        NumIndices = numIndices;
        Material = material;
    }

    public void Dispose()
    {
        VertexBuffer.Dispose();
        IndexBuffer.Dispose();
    }
}