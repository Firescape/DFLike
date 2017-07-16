using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TerrainManager: object
{
	private Cell[] terrainLayout;
	private Point mapSize;
	
	[System.NonSerialized]
	public Dictionary<int, Cell> invalidatedBlockList = new Dictionary<int, Cell>(1024);
    //public EntityManager entityManager;

	public event EventHandler onWhatever = delegate {};
	
	//private int[,,] testArray = new int[128,64,128];
	//private int[] testArray = new int[1048576];

	//private PerlinNoise perlineNoise = new PerlinNoise(UnityEngine.Random.seed);
	private int[,] heightMap;
	private int[,,] rawMap;
    public int viewLevel = -1;

	public TerrainManager(Point mapSize)
	{
		this.mapSize = mapSize;
		heightMap = new int[mapSize.x, mapSize.z];
		rawMap = new int[mapSize.x, mapSize.y, mapSize.z];

		generateTerrain();
		setupTerrain();
	}

    public Point getTopMost(Point loc)
    {
        Point newLoc = loc;

        if (getCell(loc).getTerrain().hasProperty("Solid"))
        {
            newLoc = newLoc + G.transList[0];
            for (int i = 0; i < 32; i++)
            {
                newLoc = newLoc + G.transList[0];
                if(!getCell(newLoc).getTerrain().hasProperty("Solid"))
                {
                    loc = newLoc;
                    break;
                }
            }
        }
        else if (!getCell(loc - G.transList[0]).getTerrain().hasProperty("Solid"))
        {
            for (int i = 0; i < 32; i++)
            {
                newLoc = newLoc - G.transList[0];
                if (getCell(newLoc).getTerrain().hasProperty("Solid"))
                {
                    loc = newLoc + G.transList[0];
                    break;
                }
            }
        }

        return loc;
    }

	public void generateTerrain()
	{
		int octaveCount = 1;

		float[] octPersistance = new float[]{
			0.5f,
			0.25f,
			0.125f,
			0.625f};

		float[] octScale = new float[]{
			0.001f,
			0.0005f,
			0.005f,
			0.01f};

		float octaveSum = 0.0f;
		float[][] noiseMap = GetEmptyArray<float>(mapSize.x, mapSize.z);// = GeneratePerlinNoise(128, 128, 4);
		noiseMap = GeneratePerlinNoise(mapSize.x, mapSize.z, 6); 

		for(int i = 0;i < mapSize.x; i++)
		{
			for(int i2 = 0; i2 < mapSize.z; i2++)
			{
				for(int octNum = 0;octNum < octaveCount;octNum++)
				{
					octaveSum = octaveSum + PerlinSimplexNoise.noise(i * octScale[octNum], i2 * octScale[octNum]) * octPersistance[octNum];
				}
				//Debug.Log(octaveSum);
				//heightMap[i, i2] = octaveSum * 16;
				heightMap[i, i2] = (int)(noiseMap[i][i2] * (mapSize.y / 2f) + 9);
				//heightMap[i, i2] = 16;
				//Debug.Log(new Vector2(i, i2));
				octaveSum = 0;
			}
		}

		Point loc = new Point();
		Point newLoc = new Point();

		for(int i = 0;i < mapSize.x;i++)
		{
			for(int i2 = 0;i2 < mapSize.y;i2++)
			{
				for(int i3 = 0;i3 < mapSize.z;i3++)
				{
					
					if(rawMap[i, i2, i3] == BlockDef.getId("Sand"))
						continue;

					if(i2 <= heightMap[i, i3] - 3)
					{
						rawMap[i, i2, i3] = BlockDef.getId("Stone");
					}
					else if(i2 == heightMap[i, i3] - 2 || i2 == heightMap[i, i3] - 1)
					{
						rawMap[i, i2, i3] = BlockDef.getId("Dirt");
					}
					else if(i2 == heightMap[i, i3])
					{
						rawMap[i, i2, i3] = BlockDef.getId("Grass");
					}
					else
					{
						rawMap[i, i2, i3] = BlockDef.getId("Air");
					}
				}
			}
		}

		for(int i = 0;i < mapSize.x; i++)
		{
			for(int i2 = 0;i2 < mapSize.y;i2++)
			{
				for(int i3 = 0;i3 < mapSize.z;i3++)
				{
					loc = new Point(i, i2, i3);
					if(i2 < 16)
					{
						if(rawMap[i, i2, i3] == BlockDef.getId("Air"))
						{
							rawMap[i, i2, i3] = BlockDef.getId("Water");

							for(int i4 = 0;i4 < G.transList.Length;i4++)
							{
								newLoc = loc + G.transList[i4];
								if(!(G.withinLim(newLoc, mapSize))) continue;
								if(rawMap[newLoc.x, newLoc.y, newLoc.z] != BlockDef.getId("Air") && rawMap[newLoc.x, newLoc.y, newLoc.z] != BlockDef.getId("Water"))
								{
									rawMap[newLoc.x, newLoc.y, newLoc.z] = BlockDef.getId("Sand");
								}
							}
						}
					}
				}
			}
		}

	}

	public static System.Random random = new System.Random();

	public static float[][] GenerateWhiteNoise(int width, int height)
	{
		float[][] noise = GetEmptyArray<float>(width, height);

		for(int i = 0;i < width;i++)
		{
			for(int j = 0;j < height;j++)
			{
				
				noise[i][j] = (float)random.NextDouble() % 1;
			}
		}

		return noise;
	}

	public static float Interpolate(float x0, float x1, float alpha)
	{
		return x0 * (1 - alpha) + alpha * x1;
	}

	public static T[][] GetEmptyArray<T>(int width, int height)
	{
		T[][] image = new T[width][];

		for(int i = 0;i < width;i++)
		{
			image[i] = new T[height];
		}

		return image;
	}

	public static float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
	{
		int width = baseNoise.Length;
		int height = baseNoise[0].Length;

		float[][] smoothNoise = GetEmptyArray<float>(width, height);

		int samplePeriod = 1 << octave; // calculates 2 ^ k
		float sampleFrequency = 1.0f / samplePeriod;

		for(int i = 0;i < width;i++)
		{
			//calculate the horizontal sampling indices
			int sample_i0 = (i / samplePeriod) * samplePeriod;
			int sample_i1 = (sample_i0 + samplePeriod) % width; //wrap around
			float horizontal_blend = (i - sample_i0) * sampleFrequency;

			for(int j = 0;j < height;j++)
			{
				//calculate the vertical sampling indices
				int sample_j0 = (j / samplePeriod) * samplePeriod;
				int sample_j1 = (sample_j0 + samplePeriod) % height; //wrap around
				float vertical_blend = (j - sample_j0) * sampleFrequency;

				//blend the top two corners
				float top = Interpolate(baseNoise[sample_i0][sample_j0],
					baseNoise[sample_i1][sample_j0], horizontal_blend);

				//blend the bottom two corners
				float bottom = Interpolate(baseNoise[sample_i0][sample_j1],
					baseNoise[sample_i1][sample_j1], horizontal_blend);

				//final blend
				smoothNoise[i][j] = Interpolate(top, bottom, vertical_blend);
			}
		}

		return smoothNoise;
	}

	public static float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
	{
		int width = baseNoise.Length;
		int height = baseNoise[0].Length;

		float[][][] smoothNoise = new float[octaveCount][][]; //an array of 2D arrays containing

		float persistance = 0.35f;

		//generate smooth noise
		for(int i = 0;i < octaveCount;i++)
		{
			smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
		}

		float[][] perlinNoise = GetEmptyArray<float>(width, height); //an array of floats initialised to 0

		float amplitude = 1.0f;
		float totalAmplitude = 0.0f;

		//blend noise together
		for(int octave = octaveCount - 1;octave >= 0;octave--)
		{
			amplitude *= persistance;
			totalAmplitude += amplitude;

			for(int i = 0;i < width;i++)
			{
				for(int j = 0;j < height;j++)
				{
					perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
				}
			}
		}

		//normalisation
		for(int i = 0;i < width;i++)
		{
			for(int j = 0;j < height;j++)
			{
				perlinNoise[i][j] /= totalAmplitude;
			}
		}

		return perlinNoise;
	}

	public static float[][] GeneratePerlinNoise(int width, int height, int octaveCount)
	{
		float[][] baseNoise = GenerateWhiteNoise(width, height);

		return GeneratePerlinNoise(baseNoise, octaveCount);
	}
	
	public void setupTerrain()
	{
		terrainLayout = new Cell[mapSize.x * mapSize.y * mapSize.z];

		int e;
		Point loc;

		for(int i = 0;i < mapSize.x;i++)
		{
			for(int i2 = 0;i2 < mapSize.y;i2++)
			{
				for(int i3 = 0;i3 < mapSize.z;i3++)
				{
					loc = new Point(i, i2, i3);

					setupCell(loc, new Block(rawMap[i, i2, i3]));
				}
			}
		}
	}

    
	
	public IEnumerable<Cell> submitBlockInput(List<BlockInput> inputList)
	{
		foreach(BlockInput blockInput in inputList)
		{
			useBlockOnTerrain(blockInput.container, blockInput.target);
		}
	
		return invalidatedBlockList.Values;
	}
	
	private void useBlockOnTerrain(Block block, Point loc)
	{
        if (block != null)
        {
		    Cell cell = getCell(loc);
		    int tempId = cell.getTerrain().getType();

            if (tempId != BlockDef.getId("Air"))
            {
                breakTerrain(loc);
            }
            else if (block.type != BlockDef.getId("Air"))
            {
                cell.setTerrain(block);
            }
        }
		checkBlock(loc, true);
		invalidateCell(loc);
	}

	private void breakTerrain(Point loc)
	{
		int tempId = getCell(loc).getTerrain().getType();

		getCell(loc).setTerrain(new Block(BlockDef.getId("Air")));
		addItem(loc, new Item2(new Block(tempId)));
	}

	private void addItem(Point loc, Item2 item)
	{
		Cell cell = getCell(loc);
		cell.addItem(item);
        invalidateCell(loc);
        //entityManager.indexEntity(item, IndexType.FreeItems, item.block.type);
	}
	
	private void invalidateCell(Point loc)
	{
		if(!invalidatedBlockList.ContainsKey(G.To1D(loc, mapSize)))
		{
			invalidatedBlockList.Add(G.To1D(loc, mapSize), getCell(loc));
		}
		else
		{
			//Debug.Log("duplicate invalidation attempt");
		}
	}
	
	private void checkBlock(Point loc, bool updateNearby)
	{
		Point newLoc;
		
		int i = 0;
		int i2 = 0;
		int temp = 0;
	
		if(getBlock(loc).type == 0 && getCell(loc).itemList.Count > 0)
		{
			newLoc = loc + G.transList[5];
			if(!getBlock(newLoc).hasProperty("Solid"))
			{
				for(i = loc.y - 1; i >= 0; i--)
				{
					newLoc = new Point(loc.x, i, loc.z);
					if(getBlock(newLoc).hasProperty("Solid"))
					{
						newLoc = new Point(loc.x, i + 1, loc.z);
						break;
					}
				}
				//TODO: remove indexing from here
				getCell(newLoc).moveItemsFromCell(getCell(loc));
				invalidateCell(loc);
			}
		}
		int tempType = getBlock(loc).type;
		
		if(G.withinLim(loc + G.transList[5], mapSize) && getBlock(loc).hasProperty("Gravity"))
		{
			newLoc = loc + G.transList[5];
			//when a block can be affected by gravity, it checks if there are any block below it
			//if there are none iterate through all of the blocks that are below it
			//until it reaches a block that can support it, and adds the block on top of it
			if(!getBlock(newLoc).hasProperty("Solid"))
			{
				for(i = loc.y - 2; i >= 0; i--)
				{
					newLoc = new Point(loc.x, i, loc.z);
					if(getBlock(newLoc).hasProperty("Solid"))
					{
						newLoc = new Point(loc.x, i + 1, loc.z);
						break;
					}
				}

				useBlockOnTerrain(new Block(tempType), newLoc);
				useBlockOnTerrain(new Block(0), loc);
			}
		}
		
		if(updateNearby)
		{
			for(i = 0; i < 6; i++)
			{
				newLoc = loc + G.transList[i];
				if(!G.withinLim(newLoc, mapSize)) continue;
				checkBlock(newLoc, false);
			}
		}
	}
	
	public void processSunlight(int startX, int startZ, int w, int l)
	{
		for(int i = startX; i < startX + w; i++) 
		{
			for(int i3 = startZ; i3 < startZ + l; i3++) 
			{
				for(int i2 = 31; i2 >= 0; i2--) 
				{
					if(getBlock(new Point(i, i2, i3)).hasProperty("Opaque")) 
					{
						break;
					}
					//blockLayout[To1D(i, i2, i3)].light = 16;
				}
			}
		}
	}
	
	public Cell getCell(Point loc)
	{
		return terrainLayout[G.To1D(loc, mapSize)];
	}
	
	public Block getBlock(Point loc)
	{
		return terrainLayout[G.To1D(loc, mapSize)].getTerrain();
	}
	
	public void setupCell(Point loc, Block block)
	{
		terrainLayout[G.To1D(loc, mapSize)] = new Cell(block);
		terrainLayout[G.To1D(loc, mapSize)].loc = loc;
	}
	
}

