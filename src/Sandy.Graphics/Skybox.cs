using System;
using Sandy.Graphics.Models.Primitives;
using Sandy.Graphics.Renderers;

namespace Sandy.Graphics;

public class Skybox : IDisposable
{
    public TextureCube Texture;

    public Renderable PrimitiveRenderable;
    
    public Skybox(Bitmap right, Bitmap left, Bitmap top, Bitmap bottom, Bitmap front, Bitmap back)
    {
        Texture = new TextureCube(right, left, top, bottom, front, back);

        Cube cube = new Cube();
        PrimitiveRenderable = new Renderable(cube.Vertices, cube.Indices, null);
    }

    public void Dispose()
    {
        Texture.Dispose();
    }
}