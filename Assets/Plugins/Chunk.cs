using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour
{
    public Vector3 loc = new Vector3(0, 0, 0);
    public GameObject mask;
    private bool[] maskList = new bool[G.chunkSize.x * G.chunkSize.y * G.chunkSize.z];
    private Mesh2[] meshList = new Mesh2[G.chunkSize.x * G.chunkSize.y * G.chunkSize.z];
    private bool rebuildOnVisible = false;
    private int viewLevel = 32;

    public void addMesh(Point loc, Mesh2 mesh)
    {
        meshList[To1D(loc)] = mesh;
    }


    public void setMask(Point loc, bool val)
    {
        maskList[To1D(loc)] = val;
    }

    public void setupRebuild(int viewLevel)
    {
        this.viewLevel = viewLevel;

        rebuildOnVisible = true;

        //if(gameObject.GetComponent<MeshRenderer>().isVisible)
        //{
        //rebuild();
        //}
        //else
        //{
        //print("block will be rebuilt later");
        //rebuildOnVisible = true;
        //}
    }

    public void rebuild()
    {
        var segments = new MeshSegment[16384];
        var combineCount = 0;

        var i = 0;
        var i2 = 0;
        var i3 = 0;
        //mask.GetComponent<MeshRenderer>().material.mainTexture = Instantiate(Resources.Load("Backing")) as Texture2D;
        //Texture2D texture =  mask.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        //Texture2D texture = Instantiate(Resources.Load("Backing")) as Texture2D;
        int e;
        for (i = 0; i < G.chunkSize.x; i++)
        for (i2 = 0; i2 < viewLevel; i2++)
        for (i3 = 0; i3 < G.chunkSize.z; i3++)
        {
            e = To1D(i, i2, i3);
            if (meshList[e] != null)
            {
                //combine[combineCount].mesh = meshList[e];
                //dummyObject.transform.position = new Vector3(i, i2, i3);
                //combine[combineCount].transform = dummyObject.transform.localToWorldMatrix;
                //combineCount++;
                segments[combineCount] = new MeshSegment(meshList[e], new Vector3(i, i2, i3));
                combineCount++;
            }

            if (i2 == viewLevel - 1)
                if (maskList[e])
                {
                    //texture.SetPixel(i, i3, Color.black);
                }
        }

        var colorList = new Color[6];

        colorList[0] = new Color();
        colorList[1] = new Color();
        colorList[2] = new Color();
        colorList[3] = new Color();
        colorList[4] = new Color();
        colorList[5] = new Color();

        var newMesh = Face.combineMeshes(segments);
        Destroy(gameObject.GetComponent<MeshFilter>().mesh);
        gameObject.GetComponent<MeshFilter>().mesh = newMesh;
        newMesh.RecalculateBounds();

        setMaskPosition(viewLevel);
        //texture.Apply();
        //mask.GetComponent<MeshRenderer>().material.mainTexture = texture;
    }


    public void Update()
    {
        if (rebuildOnVisible)
        {
            rebuildOnVisible = false;
            rebuild();
        }
    }

    public void setMaskPosition(int level)
    {
        mask.transform.position = new Vector3(loc.x + 8, loc.y + level - 1, loc.z + 8) + gameObject.transform.position;
    }

    public int To1D(Point loc)
    {
        return G.To1D(loc, G.chunkSize);
    }

    public int To1D(int x, int y, int z)
    {
        return G.To1D(new Point(x, y, z), G.chunkSize);
    }

    public void onRemove()
    {
        //GameObject.Destroy(transform.parent.gameObject);
    }

    public void OnBecameVisible()
    {
        //if(rebuildOnVisible)
        //{
        //rebuild();
        //rebuildOnVisible = false;
        //}
    }
}