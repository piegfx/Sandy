using System;
using System.Collections.Generic;
using Pie;
using Pie.Text;
using Sandy.Math;
using FtChar = Pie.Text.Character;

namespace Sandy.Graphics.Text;

internal class FontAssistant : IDisposable
{
    public static readonly FreeType FreeType;

    private List<Face> _faces;

    public Texture2D[] Textures;
    private uint _currentTexture;

    private Vector2T<int> _currentPosition;
    private uint _largestCharOnRow;

    private Dictionary<(char, uint), Character> _characters;

    private Size<int> _textureSize;

    private const int Padding = 2;

    static FontAssistant()
    {
        FreeType = new FreeType();
    }

    public FontAssistant(byte[] data)
    {
        _faces = new List<Face>()
        {
            FreeType.CreateFace(data)
        };

        Textures = new Texture2D[4];
        _currentTexture = 0;

        _characters = new Dictionary<(char, uint), Character>();

        _textureSize = new Size<int>(1024, 1024);
        Textures[0] = new Texture2D(_textureSize, null);
    }

    public void AddFace(byte[] data)
    {
        _faces.Add(FreeType.CreateFace(data));
    }

    public Character GetCharacter(char c, uint size)
    {
        if (!_characters.TryGetValue((c, size), out Character character))
        {
            Renderer.Instance.LogMessage(LogType.Debug, $"Creating character '{c}'.");

            Face face = null;
            foreach (Face f in _faces)
            {
                if (f.CharacterExists(c))
                {
                    face = f;
                    break;
                }
            }

            if (face == null)
                face = _faces[0];
            
            FtChar chr = face.GetCharacter(c, size);

            Size<int> chrSize = new Size<int>(chr.Width, chr.Height);
            
            if (chr.Height > _largestCharOnRow)
                _largestCharOnRow = (uint) chr.Height;
            
            if (_currentPosition.X + chr.Width >= _textureSize.Width)
            {
                _currentPosition.Y += (int) _largestCharOnRow + Padding;
                _currentPosition.X = 0;

                _largestCharOnRow = 0;

                if (_currentPosition.Y + chr.Height >= _textureSize.Height)
                {
                    _currentTexture++;
                    _currentPosition = Vector2T<int>.Zero;
                    
                    Renderer.Instance.LogMessage(LogType.Debug, $"Creating new font texture with size {_textureSize}.");

                    Texture2D fontTexture = new Texture2D(_textureSize, null);

                    if (Textures.Length <= _currentTexture)
                    {
                        Renderer.Instance.LogMessage(LogType.Debug, "Resizing font texture array.");
                        Array.Resize(ref Textures, Textures.Length << 1);
                    }

                    Textures[_currentTexture] = fontTexture;
                }
            }
            
            character = new Character
            {
                Size = chrSize,
                Bearing = new Vector2T<int>(chr.BitmapLeft, chr.BitmapTop),
                Advance = chr.Advance,
                Texture = _currentTexture,
                SourceRect = new Rectangle<int>(_currentPosition, chrSize)
            };

            Textures[_currentTexture].Update(_currentPosition.X, _currentPosition.Y, chr.Width, chr.Height, chr.Bitmap);

            _currentPosition.X += chr.Width + Padding;

            _characters.Add((c, size), character);
        }

        return character;
    }

    public void Dispose()
    {
        foreach (Face face in _faces)
            face.Dispose();
    }

    public struct Character
    {
        public Size<int> Size;
        public Vector2T<int> Bearing;
        public int Advance;

        public uint Texture;
        public Rectangle<int> SourceRect;
    }
}