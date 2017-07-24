using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using UnityEngine;

public class G
{
    public static bool loadLevel = false;
    public static string savePath = "default.dat";
    public static Point mapSize = new Point(64, 32, 64);
    public static Point chunkSize = new Point(16, 32, 16);
    public static Point bucketSize = new Point(16, 32, 16);
    public static string debugString = "";

    public static Point[] transList =
    new Point[]{
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

    public static Vector3[][] VertDef =
    new Vector3[][]{
    new Vector3[]{new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(1,0,1)},
    new Vector3[]{new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(0,-1,1), new Vector3(1,-1,1)},
    new Vector3[]{new Vector3(1,0,0), new Vector3(1,0,1), new Vector3(1,-1,0), new Vector3(1,-1,1)},
    new Vector3[]{new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(0,-1,0), new Vector3(0,-1,1)},
    new Vector3[]{new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,-1,0), new Vector3(1,-1,0)},
    new Vector3[]{new Vector3(0,-1,0), new Vector3(1,-1,0), new Vector3(0,-1,1), new Vector3(1,-1,1)}};

    public static int To1D(Point loc, Point dimensions)
    {
        return (dimensions.x * dimensions.z * (loc.y - 0) + loc.x + dimensions.x * (loc.z - 0));
    }


    public static Vector3[] normalDef =
    new Vector3[]{
    new Vector3(0, 1, 0),
    new Vector3(0, 0, 1),
    new Vector3(1, 0, 0),
    new Vector3(-1, 0, 0),
    new Vector3(0, 0, -1),
    new Vector3(0, -1, 0)};





    public static int invTransIndex(int index)
    {
        for (int i = 0; i < 18; i++)
        {
            if (transList[index] * -1 == transList[i])
            {
                //Debug.Log(new Vector2(index, i));
                return i;
            }
        }
        throw (new System.Exception("Unhandled index provided"));
    }

    public static int transIndex(Point dir)
    {
        for (int i = 0; i < 18; i++)
        {
            if (transList[i] == dir)
            {
                return i;
            }
        }
        throw (new System.Exception("Unhandled direction provided"));
    }



    public static Point vectorClamp(Point loc, Point lim)
    {
        Point newLoc = loc;

        newLoc.x = Mathf.Clamp(newLoc.x, 0, lim.x - 1);
        newLoc.y = Mathf.Clamp(newLoc.y, 0, lim.y - 1);
        newLoc.z = Mathf.Clamp(newLoc.z, 0, lim.z - 1);

        return newLoc;
    }

    public static bool withinLim(Point loc, Point trans, Point lim)
    {
        Point transLoc = new Point(loc.x + trans.x, loc.y + trans.y, loc.z + trans.z);

        if (transLoc.x >= 0 && transLoc.y >= 0 && transLoc.z >= 0
        && transLoc.x < lim.x && transLoc.y < lim.y && transLoc.z < lim.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool withinLim(Point loc, Point lim)
    {
        if (loc.x >= 0 && loc.y >= 0 && loc.z >= 0
       && loc.x < lim.x && loc.y < lim.y && loc.z < lim.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
