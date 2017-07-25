using UnityEngine;

public class MeshSegment
{
    private Mesh2 mesh;
    //private Vector3 transform;

    public MeshSegment(Mesh2 mesh, Vector3 transform)
    {
        this.mesh = new Mesh2();

        this.mesh.vertices = mesh.vertices.Clone() as Vector3[];
        this.mesh.normals = mesh.normals.Clone() as Vector3[];
        this.mesh.triangles = mesh.triangles.Clone() as int[];
        this.mesh.uv = mesh.uv.Clone() as Vector2[];

        var verts = this.mesh.vertices;

        for (var i = 0; i < verts.Length; i++)
            verts[i] += transform;

        this.mesh.vertices = verts;
    }

    public Mesh2 getMesh()
    {
        return mesh;
    }
}