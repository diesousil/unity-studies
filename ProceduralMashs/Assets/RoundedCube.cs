using UnityEngine;

public class RoundedCube : Cube
{
    public int roundness;
    public RoundedCube(int roundness)
    {
        this.roundness = roundness;
    }

    protected override void InstantiateVertices()
    {
        base.InstantiateVertices();
        _normals = new Vector3[_vertices.Length];
    }

    protected override void SetVertex(int index, int x, int y, int z)
    {
        base.SetVertex(index, x, y, z);
        Vector3 inner = new Vector3 (x, y, z);

        if (x < roundness)
        {
            inner.x = roundness;
        }
        else if (x > xSize - roundness)
        {
            inner.x = xSize - roundness;
        }

        if (y < roundness)
        {
            inner.y = roundness;
        }
        else if (y > ySize - roundness)
        {
            inner.y = ySize - roundness;
        }

        if (z < roundness)
        {
            inner.z = roundness;
        }
        else if (z > zSize - roundness)
        {
            inner.z = zSize - roundness;
        }

        _normals[index] = (_vertices[index] - inner).normalized;
        _vertices[index] = inner + _normals[index] * roundness;
    }
}