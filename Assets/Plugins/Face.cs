using System;
using UnityEngine;

public class Face : object
{
	public static GameObject dummyObject;
	
	public static Mesh2 newMesh(int side, Vector3 loc, string type, Color color, Vector3 size)
	{
		Mesh2 newMesh = new Mesh2();
		Vector3 normal;
		bool reverseTri = false;
		int[] triangles = new int[]{0, 2, 3, 0, 3, 1};
		
		Vector3[] verts = G.VertDef[side];
		verts[0] = Vector3.Scale(verts[0], size);
		verts[1] = Vector3.Scale(verts[1], size);
		verts[2] = Vector3.Scale(verts[2], size);
		verts[3] = Vector3.Scale(verts[3], size);
		newMesh.vertices = verts;


		normal = G.normalDef[side];
		
		Color[] colors = new Color[4];
		colors[0] = color;
		colors[1] = color;
		colors[2] = color;
		colors[3] = color;
		
		//newMesh.colors = colors;

		if(side == 2 || side == 4 || side == 5)
		{
			reverseTri = true;
		}
		
		if(reverseTri)
		{
			for(int i = 0; i < triangles.Length; i = i + 3)
			{
				int tmp = triangles[i];
				triangles[i] = triangles[i+2];
				triangles[i+2] = tmp;
			}
		}
		
		Vector2 uvLoc;
		
		if(type == "Grass")
		{
			uvLoc = new Vector2(0, 15);
		}
		else if(type == "Sand")
		{
			uvLoc = new Vector2(2, 14);
		}
		else if(type == "Water")
		{
			uvLoc = new Vector2(13, 3);
		}
		else if(type == "Stone")
		{
			uvLoc = new Vector2(1, 15);
		}
		else if(type == "Dirt")
		{
			uvLoc = new Vector2(2,15);
		}
		else if(type == "GrassSide")
		{
			uvLoc = new Vector2(3, 15);
		}
		else if(type == "Plank")
		{
			uvLoc = new Vector2(4, 15);
		}
		else if(type == "Bench")
		{
			uvLoc = new Vector2(4, 2);
		}
		else
		{
			uvLoc = new Vector2(4, 14);
		}
		
		float texOffset = 1;
		float texFixUp = 0.05f;
		//Rect uvCoords = 
		
		newMesh.uv = new Vector2[]{
		new Vector2((uvLoc.x + texOffset) * 0.0625f, (uvLoc.y + texOffset) * 0.0625f),
		new Vector2(uvLoc.x * 0.0625f, (uvLoc.y + texOffset) * 0.0625f),
		new Vector2((uvLoc.x + texOffset) * 0.0625f, uvLoc.y * 0.0625f),
		new Vector2(uvLoc.x * 0.0625f, uvLoc.y * 0.0625f)};
		
		
		
		
		//newMesh.uv[2] += new Vector2(0, texFixUp);
		//newMesh.uv[3] += new Vector2(0, texFixUp);
		
		newMesh.normals = new Vector3[]{normal, normal, normal, normal};
		newMesh.triangles = triangles;
		
		return newMesh;
	}

    public static Mesh2 createFace(string type)
    {
        return newMesh(0, new Vector3(0, 0, 0), type, new Color(0, 0, 0), new Vector3(1, 1, 1));
    }

	public static Mesh2 createBlock (Point loc, int type, Color[] lighting, bool[]  nearbyList)
	{
		string[] faceTex = new string[] {"Grass", "GrassSide", "GrassSide", "GrassSide", "GrassSide", "Dirt" };
		
		Vector2 chunkLoc = new Vector2(Mathf.Floor(loc.x / 16), Mathf.Floor(loc.z / 16));
	
		Point blockLoc = new Point(loc.x % 16, loc.y, loc.z % 16);
	
		if(type == 1)
		{
			faceTex = new string[]{"Grass", "GrassSide", "GrassSide", "GrassSide", "GrassSide", "Dirt"};
		}
		else if(type == 2)
		{
			faceTex = new string[]{"Stone", "Stone","Stone","Stone","Stone","Stone"};
		}
		else if(type == 3)
		{
			faceTex = new string[]{"Dirt","Dirt","Dirt","Dirt","Dirt","Dirt"};
		}
		else if(type == 4)
		{
			faceTex = new string[]{"Plank", "Plank","Plank","Plank","Plank","Plank"};
		}
		else if(type == 5)
		{
			faceTex = new string[] {"Water", "Water", "Water", "Water", "Water", "Water"};
		}
		else if(type == 6)
		{
			faceTex = new string[] {"Sand", "Sand", "Sand", "Sand", "Sand", "Sand"};
		}
	
		CombineInstance[] combine = new CombineInstance[6];
		int combineCount = 0;
		
		MeshSegment[] segments = new MeshSegment[6];
	
		for(int i = 0; i < 6; i++)
		{
			if(!nearbyList[i])
			{
	
				segments[combineCount] = new MeshSegment(newMesh(i, new Vector3(0, 0, 0), faceTex[i], lighting[0], new Vector3(1, 1, 1)), new Vector3(0, 0, 0));
				combineCount++;
			}
		}

		return combineMeshes2(segments);

	}
	
	
	public static Mesh combineMeshes(MeshSegment[] segments)
	{
		int i = 0;
		int i2 = 0;
	
		int vertexCount = 0;
		
		for (i = 0; i < segments.Length; i++) {
			if (segments[i] == null) {
				continue;
			}
			
			vertexCount += segments[i].getMesh ().vertices.Length;
		}
		
		Vector3[] verts = new Vector3[vertexCount];
		Vector3[] normals = new Vector3[vertexCount];
		int[] tris = new int[(int)(vertexCount * 1.5f)];
		Vector2[] uvs = new Vector2[vertexCount];
		int index = 0;
		
		for (i = 0; i < segments.Length; i++) {
			if (segments[i] == null) {
				continue;
			}
			
			for (i2 = 0; i2 < segments[i].getMesh ().vertices.Length; i2++) {
				verts[i2 + index] = segments[i].getMesh ().vertices[i2];
			}
			
			for (i2 = 0; i2 < segments[i].getMesh ().vertices.Length; i2++) {
				normals[i2 + index] = segments[i].getMesh ().normals[i2];
			}
			
			for (i2 = 0; i2 < segments[i].getMesh ().triangles.Length; i2++) {
				tris[(int)(i2 + index * 1.5f)] = segments[i].getMesh ().triangles[i2] + index;
			}
			
			for (i2 = 0; i2 < segments[i].getMesh ().uv.Length; i2++) {
				uvs[i2 + index] = segments[i].getMesh ().uv[i2];
			}
			
			index = index + segments[i].getMesh ().vertices.Length;
		}
		
		Mesh newMesh = new Mesh ();
		
		newMesh.vertices = verts;
		newMesh.normals = normals;
		newMesh.triangles = tris;
		newMesh.uv = uvs;
		Mesh2.calculateMeshTangents(newMesh);
		
	
		return newMesh;
	}
	
	public static Mesh2 combineMeshes2(MeshSegment[] segments)
	{
		int vertexCount = 0;
		
		int i = 0;
		int i2 = 0;
	
		for(i = 0; i < segments.Length; i++)
		{
			if(segments[i] == null)
			{
				continue;
			}
			
			vertexCount += segments[i].getMesh().vertices.Length;
		}
		
		Vector3[] verts = new Vector3[vertexCount];
		Vector3[] normals = new Vector3[vertexCount];
		int[] tris = new int[(int)(vertexCount * 1.5)];
		Vector2[] uvs = new Vector2[vertexCount];
		int index = 0;
		
		for(i = 0; i < segments.Length; i++)
		{
			if(segments[i] == null)
			{
				continue;
			}
			
			for(i2 = 0; i2 < segments[i].getMesh().vertices.Length; i2++)
			{
				verts[i2 + index] = segments[i].getMesh().vertices[i2];
			}
			
			for(i2 = 0; i2 < segments[i].getMesh().vertices.Length; i2++)
			{
				normals[i2 + index] = segments[i].getMesh().normals[i2];
			}
			
			for(i2 = 0; i2 < segments[i].getMesh().triangles.Length; i2++)
			{
				tris[(int)(i2 + index * 1.5f)] = segments[i].getMesh().triangles[i2] + index;
			}
			
			for(i2 = 0; i2 < segments[i].getMesh().uv.Length; i2++)
			{
				uvs[i2 + index] = segments[i].getMesh().uv[i2];
			}
			
			index = index + segments[i].getMesh().vertices.Length;
		}
		
		Mesh2 newMesh = new Mesh2();
		
		newMesh.vertices = verts;
		newMesh.normals = normals;
		newMesh.triangles = tris;
		newMesh.uv = uvs;
		
		return newMesh;
	}
}

public class DwRect
{
	//Vector2[
}
