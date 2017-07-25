using System;
using UnityEngine;

public class DspMap : object
{
    public int[,,] map;
    public Point start;

    public DspMap(Point start, int[,,] map)
    {
        this.start = start;
        this.map = map;
    }

    public int this[int i, int i2, int i3]
    {
        get { return map[i, i2, i3]; }
        set { map[i, i2, i3] = value; }
    }

    public int GetLength(int i)
    {
        return map.GetLength(i);
    }
}