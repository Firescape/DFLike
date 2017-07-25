using System;
using System.Collections.Generic;
using UnityEngine;

public class Block : object
{
    public int type;

    public Block(int type)
    {
        this.type = type;
    }

    public Block(byte[] bytes)
    {
        type = BitConverter.ToInt32(bytes, 0);
    }

    public List<byte> getBytes()
    {
        var bytes = new List<byte>();
        bytes.InsertRange(0, BitConverter.GetBytes(type));
        bytes.InsertRange(0, BitConverter.GetBytes(bytes.Count));
        return bytes;
    }

    public byte[] getTypeBytes()
    {
        return null;
    }

    public int getType()
    {
        return checked(type * 1);
    }

    public Mesh2 getMesh()
    {
        return getMesh(new bool[] {false, false, false, false, false, false});
    }

    public Mesh2 getMesh(bool[] nearbyList)
    {
        var lightList = new Color[6];
        for (var i = 0; i < 6; i++)
            if (!nearbyList[i])
                lightList[i] = (float) 1 * Color.white;
        return Face.createBlock(new Point(1, 1, 1), type, lightList, nearbyList);
    }


    public bool hasProperty(string val)
    {
        return BlockDef.GetInstance().table[type].Contains(val);
    }

    public object getProperty(string val)
    {
        return BlockDef.GetInstance().table[type][val];
    }
}