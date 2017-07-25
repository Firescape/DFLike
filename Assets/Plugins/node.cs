// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class Node
{
    public static Point[] transList =
        new Point[]
        {
            new Point(0, 1, 0), //0
            new Point(0, 0, 1), //1
            new Point(1, 0, 0), //2
            new Point(-1, 0, 0), //3
            new Point(0, 0, -1), //4
            new Point(0, -1, 0), //5

            new Point(1, -1, 0), //6
            new Point(-1, -1, 0), //7
            new Point(1, 1, 0), //8
            new Point(-1, 1, 0), //9

            new Point(0, -1, 1), //10
            new Point(0, -1, -1), //11
            new Point(0, 1, 1), //12
            new Point(0, 1, -1), //13

            new Point(1, 0, 1), //14
            new Point(1, 0, -1), //15
            new Point(-1, 0, 1), //16
            new Point(-1, 0, -1) //17
        };

    public bool closed = false;
    public float f;

    public float g;

    //public FIXME_VAR_TYPE vectors= new VectorLine[18];
    private GameObject gameObject;

    public int group = -1;
    public float h;
    public Node[] linkedNodes = new Node[18];
    public int[] linkSizes = new int[18];
    public bool open = false;
    public Node parentNode;
    public bool traversable = true;
    public int x;
    public int y;
    public int z;

    public Node(Point loc)
    {
        x = loc.x;
        y = loc.y;
        z = loc.z;
        clearAllLinkSizes();
        //draw();
    }

    public Node(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        clearAllLinkSizes();
        //draw();
    }

    public void showDebugCube(string text)
    {
        if (gameObject == null)
        {
            //gameObject = UnityEngine.Object.Instantiate(Resources.Load("WireCubePrefab")) as GameObject;
            //gameObject.GetComponent<WireCube>().size = new Vector3(1, 1, 1);
            //gameObject.GetComponent<WireCube>().center = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);	
        }
        //gameObject.GetComponent<WireCube>().setLabel(text);
        //gameObject.GetComponent<WireCube>().draw = true;
    }

    public void hideDebugCube()
    {
        if (gameObject != null)
            Object.Destroy(gameObject);
    }

    public void clearAllLinkSizes()
    {
        for (var i = 0; i < 18; i++)
            linkSizes[i] = 0;
    }

    public void setLinkSize(int index, int val, bool setLinked)
    {
        linkSizes[index] = val;

        if (setLinked)
            if (linkedNodes[index] != null)
                linkedNodes[index].setLinkSize(invTransIndex(index), val, false);
    }

    public int getLinkSize(Point dir) //given a direction get the link size
    {
        if (linkedNodes[transIndex(dir)] != null)
            return linkSizes[transIndex(dir)];
        else
            return -1;
    }

    public static int invTransIndex(int index)
    {
        for (var i = 0; i < 18; i++)
            if (transList[index] * -1 == transList[i])
                return i;

        throw new Exception("No inverse index could be found");
    }

    public Node[] getConnected()
    {
        var arr = new Node[18];

        for (var i = 0; i < 18; i++)
            if (linkSizes[i] != -1)
                arr[i] = linkedNodes[i];

        return arr;
    }

    public Point getLoc()
    {
        return new Point(x, y, z);
    }

    public void clearLink(int index)
    {
        if (linkedNodes[index] != null)
        {
            linkedNodes[index].linkedNodes[invTransIndex(index)] = null;
            linkedNodes[index] = null;
        }

        //int[] hey = [5];
    }

    public static int transIndex(Point dir)
    {
        for (var i = 0; i < 18; i++)
            if (transList[i] == dir)
                return i;

        throw new Exception("No translation index could be found.");
    }
}