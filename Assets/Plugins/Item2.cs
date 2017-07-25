using System;
using System.Collections.Generic;
using UnityEngine;

public class Item2 : Entity, IIndexable
{
    public Block block;

    public Item2(Block block)
    {
        this.block = block;
        //Debug.Log(GlobalList.GetInstance().getAll(block.getProperty("Type")).Count);
        init();
    }

    public override int[] getType()
    {
        return new int[] {block.type};
    }

    public void init()
    {
    }

    public Mesh2 getMesh()
    {
        var lightList = new Color[6];

        var i = 0;

        for (i = 0; i < 6; i++)
            lightList[i] = 1 * Color.white;

        var newMesh = Face.createBlock(new Point(0, 0, 0), block.type, lightList,
            new bool[] {false, false, false, false, false, false});

        var scaledMesh = new Mesh2();
        var verts = new Vector3[newMesh.vertices.Length];
        var size = 0.35f;


        //scaledMesh.vertices = new Vector3[
        for (i = 0; i < newMesh.vertices.Length; i++)
            verts[i] = newMesh.vertices[i] * size + new Vector3(0.35f, -0.65f, 0.35f);
        scaledMesh.vertices = verts;
        scaledMesh.triangles = newMesh.triangles;
        scaledMesh.uv = newMesh.uv;
        scaledMesh.normals = newMesh.normals;
        //newMesh.vertices = newMesh.vertices * 0.5f;

        return scaledMesh;
    }

    public string ToString()
    {
        return BlockDef.getType(getType()[0]);
    }


    public List<byte> getBytes()
    {
        var bytes = new List<byte>();

        bytes.AddRange(block.getBytes());

        bytes.InsertRange(0, BitConverter.GetBytes(bytes.Count));

        return bytes;
    }
}