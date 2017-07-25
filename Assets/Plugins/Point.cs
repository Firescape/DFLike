using System.Collections.Generic;
using System;
using UnityEngine;

public struct Point
{
    public int x;
    public int y;
    public int z;

    public Point(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    public Point(byte[] bytes)
    {
        x = BitConverter.ToInt32(bytes, 0);
        y = BitConverter.ToInt32(bytes, 4);
        z = BitConverter.ToInt32(bytes, 8);
    }

    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ")";
    }

    public static Point operator *(Point a, int b)
    {
        return new Point(a.x * b, a.y * b, a.z * b);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(Point a, Point b)
    {
        return a.x != b.x || a.y != b.y || a.z != b.z;
    }

    public override bool Equals(object obj)
    {
        return this == (Point) obj;
    }

    public override int GetHashCode()
    {
        return x + y + z;
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    private static Point op_Division(Point a, int b)
    {
        return new Point(a.x / b, a.y / b, a.z / b);
    }

    private static Vector3 ToVector3(Point a)
    {
        return new Vector3(a.x, a.y, a.z);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public float distanceTo(Point loc)
    {
        return (float) Math.Sqrt(Math.Pow(loc.x - x, 2) + Math.Pow(loc.y - y, 2) + Math.Pow(loc.z - z, 2));
    }

    public byte[] GetBytes()
    {
        var bytes = new List<byte>();

        bytes.AddRange(BitConverter.GetBytes(x));
        bytes.AddRange(BitConverter.GetBytes(y));
        bytes.AddRange(BitConverter.GetBytes(z));

        return bytes.ToArray();
    }

    public static Point ToPoint(Vector3 a)
    {
        return new Point((int) a.x, (int) a.y, (int) a.z);
    }
}