using System;
using System.Reflection;

namespace Sandy.Graphics.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(Assembly assembly, string resName, string message = null) : base(
        $"The given resource \"{resName}\" was not found in assembly {assembly}.") { }
}