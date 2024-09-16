using UnityEngine;

public class Grid : CustomMesh
{
    public int xSize, ySize;

    private readonly Vector4 tangent = new Vector4(1f, 0, 0, -1);

    public Grid():base("Procedural Grid")
    {

    }

    private void InstantiateTriangles()
    {
        int trianglesVerticesQt = xSize * ySize * 6;
        _triangles = new int[trianglesVerticesQt];
    }

    protected override void GenerateTriangles()
    {
        InstantiateTriangles();
        int tIndex = 0;

        for (int y=0, vIndex=0 ; y < ySize; y++)
        {
            for(int x=0; x < xSize; x++, vIndex++)
            {
                tIndex = Support.CreateQuad(_triangles, tIndex, vIndex, xSize+1);
            }
            vIndex++;
        }
    }
    private void InstantiateVertices()
    {
        int verticesQt = (xSize + 1) * (ySize + 1);
        _vertices = new Vector3[verticesQt];
        _uv = new Vector2[verticesQt];
        _tangents = new Vector4[verticesQt];
    }

    protected override void GenerateVertices()
    {
        InstantiateVertices();

        for(int y=0,vIndex=0; y<= ySize;y++)
        {
            for(int x=0; x<= xSize;x++,vIndex++)
            {
                _vertices[vIndex] = new Vector3(x, y);
                _uv[vIndex] = new Vector2((float)x/xSize, (float)y/ySize);
                _tangents[vIndex] = tangent;
            }
        }
    }


}
