using System;
using UnityEngine;

public class TerrainChunk: object
{
	int[] blocks;
	private static Point CHUNK_SIZE = new Point(16, 16, 32);
	public string txt = "";

	
	public TerrainChunk()
	{
		int mult = 32;
		
		blocks = new int[CHUNK_SIZE.x * CHUNK_SIZE.y * CHUNK_SIZE.z * mult];
		blocks.Initialize();
		System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();
		//blocks[0] = new Block2(5);
		int bl = blocks.Length;
		for(int i = 0; i < bl; i++)
		{
			blocks[i] = blocks[i] | 17;
			//blocks[i].shift(36);
			//blocks[i].shift(17);
			//blocks[i].shift(4);
			//blocks[i].shift(17);
			//blocks[i].shift(5);
		}
			
		stopWatch.Stop();
		//Debug.Log("It took " + stopWatch.Elapsed.TotalMilliseconds + "ms out of 10.0 ms");
		txt = "It took " + stopWatch.Elapsed.TotalMilliseconds / mult + "ms out of 10.0 ms." + "Block count:" + CHUNK_SIZE.x * CHUNK_SIZE.y * CHUNK_SIZE.z * mult;
		
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


