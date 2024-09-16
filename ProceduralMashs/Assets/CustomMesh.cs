using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public abstract class CustomMesh : MonoBehaviour
{
    protected Mesh _mesh; 
    protected Vector3[] _vertices;
    protected Vector3[] _normals;
    protected Vector4[] _tangents;
    protected int[] _triangles;
    protected Vector2[] _uv;
    private string _name;

    public CustomMesh(string name)
    {
        _name = name;

    }
    private void OnDrawGizmos()
    {
        if (_vertices == null)
            return;

        Gizmos.color = Color.black;

        Support.DebugVertices(_vertices);
        Support.DebugTriangles(_vertices, _triangles);
    }

    public void Awake()
    {
        Generate();
    }

    private void GenerateUV()
    {
        for(int k=0;k<_vertices.Length;k++)
        {
            _uv[k] = new Vector2();
        }

    }

    protected void Generate()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = _name;

        GenerateVertices();
        GenerateTriangles();

        if(_vertices != null)
            _mesh.vertices = _vertices;

        if (_triangles != null)
            _mesh.triangles = _triangles;

        if (_uv != null)
            _mesh.uv = _uv;

        if (_tangents != null)
            _mesh.tangents = _tangents;

        if (_normals != null)
            _mesh.normals = _normals;

        if (_normals == null && _vertices != null && _triangles != null)
            _mesh.RecalculateNormals();
    }

    protected abstract void GenerateVertices();
    protected abstract void GenerateTriangles();



}