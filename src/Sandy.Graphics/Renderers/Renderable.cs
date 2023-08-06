using System;
using Pie;
using Sandy.Graphics.Materials;

namespace Sandy.Graphics.Renderers;

public class Renderable : IDisposable
{
    public GraphicsBuffer VertexBuffer;
    public GraphicsBuffer IndexBuffer;

    public uint NumElements;

    public Material Material;

    public Renderable(VertexPositionTextureColorNormalTangent[] vertices, uint[] indices, Material material, bool dynamic = false)
    {
        GraphicsDevice device = Renderer.Instance.Device;

        VertexBuffer = device.CreateBuffer(BufferType.VertexBuffer, vertices, dynamic);

        if (indices == null)
            NumElements = (uint) vertices.Length;
        else
        {
            IndexBuffer = device.CreateBuffer(BufferType.IndexBuffer, indices, dynamic);
            NumElements = (uint) indices.Length;
        }

        Material = material;
    }

    public Renderable(GraphicsBuffer vertexBuffer, GraphicsBuffer indexBuffer, uint numElements, Material material)
    {
        VertexBuffer = vertexBuffer;
        IndexBuffer = indexBuffer;
        NumElements = numElements;
        Material = material;
    }

    public void Dispose()
    {
        VertexBuffer.Dispose();
        IndexBuffer.Dispose();
    }
}