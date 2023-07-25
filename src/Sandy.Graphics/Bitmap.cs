using System.IO;
using Pie;
using Sandy.Math;
using StbImageSharp;

namespace Sandy.Graphics;

public class Bitmap
{
    public readonly byte[] Data;

    public readonly Size<int> Size;

    public readonly Format Format;

    public Bitmap(string path) : this(File.ReadAllBytes(path)) { }

    public Bitmap(byte[] data)
    {
        ImageResult result = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);
        Data = result.Data;
        Size = new Size<int>(result.Width, result.Height);
        Format = Format.R8G8B8A8_UNorm;
    }
    
    public Bitmap(byte[] data, Size<int> size, Format format)
    {
        Data = data;
        Size = size;
        Format = format;
    }
}