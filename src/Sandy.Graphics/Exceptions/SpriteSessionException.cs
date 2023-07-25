using System;

namespace Sandy.Graphics.Exceptions;

public class SpriteSessionException : Exception
{
    public SpriteSessionException(string message) : base(message) { }
}