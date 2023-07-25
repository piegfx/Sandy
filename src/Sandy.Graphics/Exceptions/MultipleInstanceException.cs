using System;
using System.Runtime.Serialization;

namespace Sandy.Graphics.Exceptions;

public class MultipleInstanceException : Exception
{
    public MultipleInstanceException() { }
    protected MultipleInstanceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public MultipleInstanceException(string message) : base(message) { }
    public MultipleInstanceException(string message, Exception innerException) : base(message, innerException) { }
}