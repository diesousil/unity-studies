using System;
using System.Reflection;
using UnityEngine;

public abstract class Support
{
    public static int CreateQuad(int[] triangleVertices, int tIndex, int v00, int v10, int v01, int v11, bool invert = false)
    {
        triangleVertices[tIndex] = v00;

        triangleVertices[tIndex + 3] = v11;

        if (invert)
        {
            triangleVertices[tIndex + 1] = v10;
            triangleVertices[tIndex + 2] = v01;

            triangleVertices[tIndex + 4] = v01;
            triangleVertices[tIndex + 5] = v10;
        } else        {
            triangleVertices[tIndex + 1] = v01;
            triangleVertices[tIndex + 2] = v10;

            triangleVertices[tIndex + 4] = v10;
            triangleVertices[tIndex + 5] = v01;
        }

        return tIndex + 6;

    }
    public static int CreateQuad(int[] triangleVertices, int tIndex, int vIndex, int rowLength, bool invert = false)
    {
        return Support.CreateQuad(triangleVertices, tIndex, vIndex, vIndex + 1, vIndex + rowLength, vIndex + rowLength + 1, invert);
    }

    public static void DebugVertices(Vector3[] vertices)
    {
        for (int v = 0; v < vertices.Length; v++)
        {
            Gizmos.DrawSphere(vertices[v], .1f);
        }
    }

    public static void DebugTriangles(Vector3[] vertices, int[] triangles)
    {
        Vector3[] lines = new Vector3[triangles.Length];

        for (int v = 0; v < triangles.Length; v++)
        {
            lines[v] = vertices[triangles[v]];
            if((v+1) % 3 == 0 && v > 0)
            {
                Gizmos.DrawLine(lines[v], lines[v - 2]);
            }
        }

        Gizmos.DrawLineList(lines);
    }
}