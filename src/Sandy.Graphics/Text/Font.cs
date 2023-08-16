using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using Sandy.Graphics.Renderers;
using Sandy.Math;

namespace Sandy.Graphics.Text;

public class Font : IDisposable
{
    private FontAssistant _assistant;

    public Font(string path)
    {
        _assistant = new FontAssistant(File.ReadAllBytes(path));
    }
    
    public Font(byte[] data)
    {
        _assistant = new FontAssistant(data);
    }

    public void Draw(SpriteRenderer renderer, uint size, string text, Vector2 position, Color color)
    {
        int largestChar = 0;
        foreach (char c in text)
        {
            FontAssistant.Character character = _assistant.GetCharacter(c, size);

            if (character.Bearing.Y > largestChar)
                largestChar = character.Bearing.Y;
        }
        
        Vector2 pos = position;
        pos.Y += largestChar;

        foreach (char c in text)
        {
            FontAssistant.Character character = _assistant.GetCharacter(c, size);

            switch (c)
            {
                case ' ':
                    pos.X += character.Advance;
                    continue;
                
                case '\n':
                    pos.Y += size;
                    pos.X = position.X;
                    continue;
            }

            // TODO: Sort by texture
            // The only problem I can see with this multi-texture system is that it can lead to flip-flopping between
            // textures. So either I need to sort by texture, or implement texture array support into the sprite renderer
            // so that the font textures can be sent over at the same time.
            renderer.Draw(_assistant.Textures[character.Texture],
                new Vector2(pos.X + character.Bearing.X, pos.Y - character.Bearing.Y), character.SourceRect, color, 0,
                Vector2.One, Vector2.Zero);

            pos.X += character.Advance;
        }
    }

    public Size<int> MeasureString(uint size, string text)
    {
        Vector2 pos = Vector2.Zero;
        Size<int> totalSize = Size<int>.Zero;
        
        int largestChar = 0;
        foreach (char c in text)
        {
            FontAssistant.Character character = _assistant.GetCharacter(c, size);

            if (character.Bearing.Y > largestChar)
                largestChar = character.Bearing.Y;
        }

        pos.Y += largestChar;

        foreach (char c in text)
        {
            FontAssistant.Character character = _assistant.GetCharacter(c, size);

            switch (c)
            {
                case '\n':
                    pos.Y += size;
                    pos.X = 0;
                    break;
                
                default:
                    pos.X += character.Advance;
                    if (pos.X > totalSize.Width)
                        totalSize.Width = (int) pos.X;
                    break;
            }
        }

        if (pos.Y > totalSize.Height)
            totalSize.Height = (int) pos.Y;

        return totalSize;
    }

    public void Dispose()
    {
        _assistant.Dispose();
    }

    public static readonly Font Roboto = new Font(EmbeddedResource.Load(Assembly.GetExecutingAssembly(),
        "Sandy.Graphics.Text.Roboto-Regular.ttf"));
}