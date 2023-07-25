using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Sandy.Math;

namespace Sandy.Graphics.Models;

// Super simple and cheap obj loader. Not designed to load anything advanced, that will be handled by modelo + gltf.
public class Obj
{
    public VertexPositionTextureColorNormalTangent[] Vertices;
    public uint[] Indices;
    
    public Obj(string path)
    {
        string obj = File.ReadAllText(path);

        string[] splitObj = obj.Split('\n');

        List<Vector3> positions = new List<Vector3>();
        List<Vector2> texCoords = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<uint> indices = new List<uint>();

        List<VertexPositionTextureColorNormalTangent> vertices = new List<VertexPositionTextureColorNormalTangent>();
        Dictionary<string, uint> addedVertices = new Dictionary<string, uint>();
        uint currentIndex = 0;

        foreach (string l in splitObj)
        {
            string line = l.Trim();
            
            if (line.StartsWith('#'))
                continue;

            string[] splitLine = line.Split(' ');

            switch (splitLine[0])
            {
                case "v":
                    Vector3 position = new Vector3();
                    position.X = float.Parse(splitLine[1]);
                    position.Y = float.Parse(splitLine[2]);
                    position.Z = float.Parse(splitLine[3]);
                    positions.Add(position);

                    break;
                
                case "vt":
                    Vector2 texCoord = new Vector2();
                    texCoord.X = float.Parse(splitLine[1]);
                    texCoord.Y = float.Parse(splitLine[2]);
                    texCoords.Add(texCoord);

                    break;
                
                case "vn":
                    Vector3 normal = new Vector3();
                    normal.X = float.Parse(splitLine[1]);
                    normal.Y = float.Parse(splitLine[2]);
                    normal.Z = float.Parse(splitLine[3]);
                    normals.Add(normal);

                    break;
                
                case "f":
                    for (int i = 1; i < splitLine.Length; i++)
                    {
                        ref string f = ref splitLine[i];

                        string[] splitF = f.Split('/');

                        int index = int.Parse(splitF[0]) - 1;

                        if (addedVertices.TryGetValue(f, out uint relIndex))
                        {
                            indices.Add(relIndex);
                            continue;
                        }

                        relIndex = currentIndex;
                        addedVertices.Add(f, currentIndex++);
                        indices.Add(relIndex);

                        switch (splitF.Length)
                        {
                            case 1:
                                vertices.Add(new VertexPositionTextureColorNormalTangent()
                                {
                                    Position = positions[index],
                                    Color = Color.White
                                });

                                break;
                            
                            case 2:
                                vertices.Add(new VertexPositionTextureColorNormalTangent()
                                {
                                    Position = positions[index],
                                    TexCoord = texCoords[int.Parse(splitF[1]) - 1],
                                    Color = Color.White
                                });

                                break;
                            
                            case 3:
                                Vector2 tCoord = Vector2.Zero;

                                if (!string.IsNullOrWhiteSpace(splitF[1]))
                                    tCoord = texCoords[int.Parse(splitF[1]) - 1];
                                
                                vertices.Add(new VertexPositionTextureColorNormalTangent()
                                {
                                    Position = positions[index],
                                    TexCoord = tCoord,
                                    Normal = normals[int.Parse(splitF[2]) - 1],
                                    Color = Color.White
                                });

                                break;
                            
                            default:
                                throw new Exception("what");
                        }
                    }
                    
                    break;
            }
        }

        Vertices = vertices.ToArray();
        Indices = indices.ToArray();
    }
}