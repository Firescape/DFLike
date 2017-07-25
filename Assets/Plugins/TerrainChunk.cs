using System;
using System.Diagnostics;
using UnityEngine;

public class TerrainChunk : object
{
    private static Point CHUNK_SIZE = new Point(16, 16, 32);
    private int[] blocks;
    public string txt = "";


    public TerrainChunk()
    {
        var mult = 32;

        blocks = new int[CHUNK_SIZE.x * CHUNK_SIZE.y * CHUNK_SIZE.z * mult];
        blocks.Initialize();
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        //blocks[0] = new Block2(5);
        var bl = blocks.Length;
        for (var i = 0; i < bl; i++)
            blocks[i] = blocks[i] | 17;
        //blocks[i].shift(36);
        //blocks[i].shift(17);
        //blocks[i].shift(4);
        //blocks[i].shift(17);
        //blocks[i].shift(5);

        stopWatch.Stop();
        //Debug.Log("It took " + stopWatch.Elapsed.TotalMilliseconds + "ms out of 10.0 ms");
        txt = "It took " + stopWatch.Elapsed.TotalMilliseconds / mult + "ms out of 10.0 ms." + "Block count:" +
              CHUNK_SIZE.x * CHUNK_SIZE.y * CHUNK_SIZE.z * mult;

        //Debug.Log(blocks[2].ToString());
    }

    //public Block2 getBlock(Point pos)
    //{
    //return blocks[pos.x + pos.y * CHUNK_SIZE.y + pos.z * CHUNK_SIZE.y * CHUNK_SIZE.z];
    //}

    public void destroyBlock(Point pos)
    {
    }

    public void placeBlock(Point pos)
    {
    }
}