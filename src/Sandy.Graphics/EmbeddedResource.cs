using System.IO;
using System.Reflection;
using System.Text;
using Sandy.Graphics.Exceptions;

namespace Sandy.Graphics;

public static class EmbeddedResource
{
    public static byte[] Load(Assembly assembly, string resName)
    {
        using Stream stream = assembly.GetManifestResourceStream(resName);
        if (stream == null)
            throw new ResourceNotFoundException(assembly, resName);

        using MemoryStream memStream = new MemoryStream();
        stream.CopyTo(memStream);
        return memStream.ToArray();
    }

    public static string LoadString(Assembly assembly, string resName, Encoding encoding = null)
    {
        byte[] resource = Load(assembly, resName);

        return (encoding ?? Encoding.Default).GetString(resource);
    }
}