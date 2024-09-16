using System;
using UnityEngine;

public class Cube : CustomMesh
{
    public int xSize, ySize, zSize;
    private int _roundLength;

    public Cube():base("Procedural Cube")
    {
    }

    #region Vertices

    protected virtual void SetVertex(int index, int x, int y, int z)
    {
        _vertices[index] = new Vector3(x, y, z);
    }

    protected virtual void InstantiateVertices()
    {
        int cornersQt = 8;
        int edgesVerticesQt = 4 * (xSize + ySize + zSize - 3);
        int facesInnerVerticesQt = 2 * ((xSize - 1) * (ySize - 1) + (ySize - 1) * (zSize - 1) + (xSize - 1) * (zSize - 1));

        int totalVerticesQt = cornersQt + edgesVerticesQt + facesInnerVerticesQt;
        _vertices = new Vector3[totalVerticesQt];

    }

    private int GenerateVerticesRound(int y, int index)
    {
        for (int x = 0; x <= xSize; x++)
        {
            SetVertex(index++, x, y, 0);
        }

        for (int z = 1; z <= zSize; z++)
        {
            SetVertex(index++, xSize, y, z);
        }

        for (int x = xSize - 1; x >= 0; x--)
        {
            SetVertex(index++, x, y, zSize);
        }

        for (int z = zSize-1; z > 0 ; z--)
        {
            SetVertex(index++, 0, y, z);
        }

        return index;
    }

    private int GenerateLateralVertices()
    {
        int vIndex = 0;
        for (int y = 0; y <= ySize; y++)
        {
            vIndex = GenerateVerticesRound(y, vIndex);
        }

        return vIndex;

    }

    private int GenerateFaceInnerVertices(int y, int vIndex)
    {
        for (int z = 1; z < zSize ; z++)
        {
            for (int x = 1; x < xSize ; x++)
            {
                SetVertex(vIndex++, x, y, z);
            }
        }

        return vIndex;
    }


    private int GenerateUpDownFacesVertices(int vIndex)
    {
        // y=0 => down
        vIndex = GenerateFaceInnerVertices(0, vIndex);

        // y=ySize => up
        vIndex = GenerateFaceInnerVertices(ySize, vIndex);

        return vIndex;
    }

    protected override void GenerateVertices()
    {
        InstantiateVertices();

        int vIndex = GenerateLateralVertices();
        GenerateUpDownFacesVertices(vIndex);
    }

    #endregion Vertices

    #region Triangles
    private void InstantiateTriangles()
    {
        int qtTriangles = 12 * (xSize*ySize + zSize*ySize  + xSize*zSize);
        _triangles = new int[qtTriangles];
    }

    private int GenerateFinalRoundTriangle(int tIndex, int vIndex, int roundLength)
    {
        _triangles[tIndex] = vIndex;
        _triangles[tIndex + 1] = vIndex + roundLength;
        _triangles[tIndex + 2] = vIndex - roundLength + 1;

        _triangles[tIndex + 3] = vIndex + 1;
        _triangles[tIndex + 4] = _triangles[tIndex + 2];
        _triangles[tIndex + 5] = _triangles[tIndex + 1];

        return tIndex + 6;
    }
    private int GenerateRoundTriangles(int tIndex)
    {
        int vIndex = tIndex/6;

        for(int i=0;i< _roundLength - 1;i++,vIndex++)
        {
            tIndex = Support.CreateQuad(_triangles, tIndex, vIndex, _roundLength);
        }

        tIndex = GenerateFinalRoundTriangle(tIndex, vIndex, _roundLength);

        return tIndex;
    }

    private int GenerateFaceInnerTriangles(int tIndex, int vIndex, bool invert = false)
    {

        for (int z = 0; z < zSize - 2; z++)
        {
            for (int x = 0; x < xSize - 2; x++, vIndex++)
            {
                tIndex = Support.CreateQuad(_triangles, tIndex, vIndex, xSize - 1, invert);
            }
            vIndex++;
        }

        return tIndex;

    }



    private int GenerateFaceLaterals(int tIndex, int vIndex, bool invert = false, bool isUp = false)
    {
        int baseVerticeIndex = (isUp ? ySize * _roundLength : 0);
        
        for (int x = 1; x < xSize - 1; x++)
        {
            tIndex = Support.CreateQuad(_triangles, tIndex, baseVerticeIndex + x, baseVerticeIndex + x + 1, vIndex + x - 1, vIndex + x, invert);
        }

        int baseTIndex = baseVerticeIndex + xSize;
        int baseVIndex = vIndex + xSize - 2;
        
        for (int z = 1; z < zSize - 1; z++)
        {
            int v00 = baseVIndex + (xSize - 1) * (z - 1);
            int v10 = baseTIndex + z;
            int v01 = v00 + xSize - 1;
            int v11 = v10 + 1;
            tIndex = Support.CreateQuad(_triangles, tIndex, v00, v10, v01, v11, invert);
        }

        baseTIndex = baseVerticeIndex + xSize + zSize;
        baseVIndex = vIndex + (xSize - 1) * (zSize - 1) - 1;

        for (int x = 0; x < xSize - 2; x++)
        {
            int v00 = baseVIndex - x - 1;
            int v10 = baseVIndex - x;
            int v01 = baseTIndex + 2 + x;
            int v11 = baseTIndex + 1 + x;
            tIndex = Support.CreateQuad(_triangles, tIndex, v00, v10, v01, v11, invert);
        }

        baseTIndex = baseVerticeIndex + 2 * xSize + zSize + 2;
        baseVIndex = vIndex + (xSize - 1) * (zSize - 3);

        for (int z = 0; z < zSize - 2; z++)
        {
            int v00 = baseTIndex + z;
            int v10 = baseVIndex - (xSize - 1) * z;
            int v01 = v00 - 1;
            int v11 = v10 + (xSize - 1);
            tIndex = Support.CreateQuad(_triangles, tIndex, v00, v10, v01, v11, invert);
        }

        return tIndex;

    }

    private int GenerateDownFaceCorners(int tIndex, int vIndex)
    {
        // bottom left
        tIndex = Support.CreateQuad(_triangles, tIndex, 0, 1, _roundLength - 1, vIndex, true);


        // bottom right
        if(xSize > 1)
        {
            tIndex = Support.CreateQuad(_triangles, tIndex, xSize - 1, xSize, vIndex + xSize - 2, xSize + 1, true);
        }

        // top right
        int baseVertice = xSize + zSize + 1;
        int innerFaceBaseVertice = vIndex + (xSize - 1) * (zSize - 1);

        if(xSize > 1)
        {
            innerFaceBaseVertice -= 1;
        }

        tIndex = Support.CreateQuad(_triangles, tIndex, innerFaceBaseVertice, baseVertice - 2, baseVertice, baseVertice - 1, true);

        // top left
        if(xSize > 1)
        {
            baseVertice = 2 * xSize + zSize + 1;
            if (xSize > 2)
            {
                innerFaceBaseVertice = vIndex + (xSize - 1) * (zSize - 2);
            }

            tIndex = Support.CreateQuad(_triangles, tIndex, baseVertice, innerFaceBaseVertice, baseVertice - 1, baseVertice - 2, true);
        }


        return tIndex;

    }

    private int GenerateUpFaceCorners(int tIndex, int vIndex)
    {
        int baseVertice = ySize * _roundLength;

        // bottom left
        tIndex = Support.CreateQuad(_triangles, tIndex, baseVertice, baseVertice + 1, baseVertice + _roundLength - 1, vIndex);

        // bottom right
        tIndex = Support.CreateQuad(_triangles, tIndex, baseVertice + xSize - 1, baseVertice + xSize, vIndex + xSize - 2, baseVertice + xSize + 1);

        // top right
        baseVertice += xSize + zSize;
        int innerFaceBaseVertice = vIndex + (xSize - 1) * (zSize - 1) - 1;
        tIndex = Support.CreateQuad(_triangles, tIndex, innerFaceBaseVertice, baseVertice - 1, baseVertice+1, baseVertice);

        // top left
        baseVertice += xSize;
        innerFaceBaseVertice = vIndex + (xSize - 1) * (zSize - 2);
        tIndex = Support.CreateQuad(_triangles, tIndex, baseVertice + 1, innerFaceBaseVertice, baseVertice, baseVertice - 1);

        return tIndex;

    }

    private int GenerateDownFace(int tIndex, int vIndex)
    {
        tIndex = GenerateFaceInnerTriangles(tIndex, vIndex, true);
        tIndex = GenerateDownFaceCorners(tIndex, vIndex);
        tIndex = GenerateFaceLaterals(tIndex, vIndex, true);

        return tIndex;
    }

    private int GenerateUpFace(int tIndex, int vIndex)
    {
        tIndex = GenerateFaceInnerTriangles(tIndex, vIndex);
        tIndex = GenerateUpFaceCorners(tIndex, vIndex);
        tIndex = GenerateFaceLaterals(tIndex, vIndex, false, true);

        return tIndex;
    }

    private int GenerateUpDownFacesTriangles(int tIndex)
    {
        int downFaceTIndex = tIndex + _roundLength * 6;
        int vIndex = downFaceTIndex / 6;
        tIndex = GenerateDownFace(tIndex, vIndex);
        vIndex += (xSize - 1) * (zSize - 1);

        return GenerateUpFace(tIndex, vIndex);
    }

    private int GenerateLateralTriangles()
    {
        int tIndex = 0;

        for(int y=0;y<ySize;y++)
        {
            tIndex = GenerateRoundTriangles(tIndex);
        }

        return tIndex;
    }

    protected override void GenerateTriangles()
    {
        _roundLength = 2 * (xSize + zSize);
        InstantiateTriangles();

        int tIndex = GenerateLateralTriangles();
        GenerateUpDownFacesTriangles(tIndex);
    }

    #endregion Triangles

}
