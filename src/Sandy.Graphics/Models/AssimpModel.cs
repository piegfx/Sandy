using System.Runtime.CompilerServices;
using Sandy.Math;
using Silk.NET.Assimp;
using Material = Sandy.Graphics.Materials.Material;

namespace Sandy.Graphics.Models;

public class AssimpModel
{
    private static Assimp _assimp;

    public Mesh[] Meshes;
    public Material[] Materials;

    static AssimpModel()
    {
        _assimp = Assimp.GetApi();
    }

    public unsafe AssimpModel(string path)
    {
        Scene* scene = _assimp.ImportFile(path,
            (uint) (PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.FlipUVs | PostProcessSteps.PreTransformVertices | PostProcessSteps.CalculateTangentSpace));

        Meshes = new Mesh[scene->MNumMeshes];

        for (int m = 0; m < Meshes.Length; m++)
        {
            Silk.NET.Assimp.Mesh* mesh = scene->MMeshes[m];

            VertexPositionTextureColorNormalTangent[] vertices =
                new VertexPositionTextureColorNormalTangent[mesh->MNumVertices];

            for (int v = 0; v < vertices.Length; v++)
            {
                vertices[v] = new VertexPositionTextureColorNormalTangent(mesh->MVertices[v],
                    mesh->MTextureCoords.Element0[v].ToVector2(), Color.White, mesh->MNormals[v], mesh->MTangents[v]);
            }

            uint numIndices = 0;
            for (int i = 0; i < mesh->MNumFaces; i++)
                numIndices += mesh->MFaces[i].MNumIndices;

            uint[] indices = new uint[numIndices];
            numIndices = 0;

            fixed (uint* indPtr = indices)
            {
                for (int i = 0; i < mesh->MNumFaces; i++)
                {
                    Unsafe.CopyBlock(indPtr + numIndices, mesh->MFaces[i].MIndices,
                        mesh->MFaces[i].MNumIndices * sizeof(uint));
                    numIndices += mesh->MFaces[i].MNumIndices;
                }
            }

            Meshes[m] = new Mesh(vertices, indices, new Material(Texture2D.White));
        }
        
        //ProcessNode(scene, scene->MRootNode, ref meshes);

        _assimp.FreeScene(scene);
    }

    /*private unsafe void ProcessNode(Scene* scene, Node* node, ref List<Mesh> meshes)
    {
        node->
    }*/
}