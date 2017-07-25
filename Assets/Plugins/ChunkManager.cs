using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class ChunkManager : object
{
    private Chunk[,] chunkList;
    private Point chunkSize;
    private Func<Point, Cell> getCell;
    private Point mapSize;

    private int viewLevel = 32;

    public ChunkManager(Point mapSize, Point chunkSize, Func<Point, Cell> getCell)
    {
        this.mapSize = mapSize;
        this.chunkSize = chunkSize;
        this.getCell = getCell;

        var chunkCount = new Point((int) (mapSize.x / chunkSize.x), (int) (mapSize.y / chunkSize.y),
            (int) (mapSize.z / chunkSize.z));
        chunkList = new Chunk[chunkCount.x, chunkCount.z];

        GameObject newChunk;

        for (var i = 0; i < chunkCount.x; i++)
        for (var i2 = 0; i2 < chunkCount.z; i2++)
        {
            newChunk = Object.Instantiate(Resources.Load("ChunkPrefab")) as GameObject;
            //Debug.Log(new Vector2(i, i2));
            chunkList[i, i2] = newChunk.GetComponent<Chunk>();
            chunkList[i, i2].gameObject.transform.position = new Vector3(chunkSize.x * i, 1, chunkSize.z * i2);
            //TODO: reorder hierarchy for chunk/chunkprefab

            //chunkList[i, i2].loc = new Vector3(chunkSize.x * i, 1, chunkSize.z * i2);
            chunkList[i, i2].mask = Object.Instantiate(Resources.Load("Mask")) as GameObject;
        }
        renderArea(new Point(0, 0, 0), mapSize);


        rebuildAllChunks();
    }


    public void submitUpdateList(IEnumerable<Cell> invalidatedBlockList)
    {
        foreach (var invBlock in invalidatedBlockList)
            renderBlock(invBlock.loc, true, true);
    }

    private void renderArea(Point start, Point size)
    {
        for (var i = start.x; i < start.x + size.x; i++)
        for (var i2 = start.y; i2 < start.y + size.y; i2++)
        for (var i3 = start.z; i3 < start.z + size.z; i3++)
            renderBlock(new Point(i, i2, i3), false, false);
    }

    private Point getChunkLoc(Point loc)
    {
        return new Point(Mathf.FloorToInt(loc.x / chunkSize.x), Mathf.FloorToInt(loc.y / chunkSize.y),
            Mathf.FloorToInt(loc.z / chunkSize.z));
    }

    private Point getChunkBlockLoc(Point loc)
    {
        return new Point(loc.x % chunkSize.x, loc.y, loc.z % chunkSize.z);
    }

    public void renderBlock(Point loc, bool fixNearby, bool fixChunk)
    {
        var nearbyList = nearbyOpaqueBlocks(loc);
        var nearbyListLocs = nearbyBlocksLocs(loc);
        var nearbyBlock = nearbyBlocks(loc);

        var chunkLoc = getChunkLoc(loc);
        var blockLoc = getChunkBlockLoc(loc);
        var newLight = 0;

        var i = 0;


        if (fixNearby)
        {
            renderBlock(loc, false, false);
            for (i = 0; i < 6; i++)
            {
                clearFace(nearbyListLocs[i]);
                renderBlock(nearbyListLocs[i], false, false);
                //newLight = getBlock(loc).light - 2;
                //if(newLight <= 0)
                //{
                //continue;
                //}
                //if(!G.withinLim(nearbyListLocs[i], mapSize)) continue;
                //if(nearbyBlock[i].light < newLight && newLight != 0)
                //{
                //print(newLight);
                //blockLayout[To1D(nearbyListLocs[i])].light = newLight;
                //renderBlock(loc, true, false);
                //}
            }
        }

        var nearbyCount = 0;
        var lightList = new Color[6];

//		for(i = 0; i < 6; i++)
//		{
//			if(i == 0 && nearbyList[i] && getBlock(loc).getType() > 0)
//			{
//				chunkList[chunkLoc.x, chunkLoc.y].setMask(blockLoc, true);
//			}
//			else if(i == 0)
//			{
//				chunkList[chunkLoc.x, chunkLoc.y].setMask(blockLoc, false);
//			}
//		}
        //Debug.Log(chunkList[chunkLoc.x, chunkLoc.z]);
        chunkList[chunkLoc.x, chunkLoc.z].addMesh(blockLoc, getCell(loc).getMesh(nearbyList));

        if (G.withinLim(loc + new Point(0, 1, 0), G.mapSize) &&
            getCell(loc + new Point(0, 1, 0)).getTerrain().getType() > 0)
            chunkList[chunkLoc.x, chunkLoc.z].setMask(blockLoc, true);
        else
            chunkList[chunkLoc.x, chunkLoc.z].setMask(blockLoc, false);

        if (getCell(loc).getTerrain().getType() == 0)
            chunkList[chunkLoc.x, chunkLoc.z].setMask(blockLoc, false);

        if (fixChunk)
        {
            chunkList[chunkLoc.x, chunkLoc.z].setupRebuild(viewLevel);

            if (blockLoc.x == 0 && loc.x != 0)
                chunkList[chunkLoc.x - 1, chunkLoc.z].setupRebuild(viewLevel);
            else if (blockLoc.x == 15 && loc.x != mapSize.x - 1)
                chunkList[chunkLoc.x + 1, chunkLoc.z].setupRebuild(viewLevel);

            if (blockLoc.z == 0 && loc.z != 0)
                chunkList[chunkLoc.x, chunkLoc.z - 1].setupRebuild(viewLevel);
            else if (blockLoc.z == 15 && loc.z != mapSize.z - 1)
                chunkList[chunkLoc.x, chunkLoc.z + 1].setupRebuild(viewLevel);
        }
    }

    public Point[] nearbyBlocksLocs(Point loc)
    {
        var nearbyList = new Point[6];
        Point newLoc;
        for (var i = 0; i < 6; i++)
        {
            newLoc = loc + G.transList[i];
            if (!G.withinLim(newLoc, mapSize)) continue;
            if (getCell(newLoc).getTerrain().getType() > 0)
                nearbyList[i] = loc + G.transList[i];
        }
        return nearbyList;
    }

    public Block[] nearbyBlocks(Point loc)
    {
        var nearbyList = new Block[6];
        Point newLoc;

        for (var i = 0; i < 6; i++)
        {
            newLoc = loc + G.transList[i];
            if (!G.withinLim(newLoc, mapSize)) continue;

            nearbyList[i] = getCell(loc + G.transList[i]).getTerrain();
        }
        return nearbyList;
    }

    public void downViewLevel()
    {
        viewLevel--;
        rebuildAllChunks();
    }

    public void setViewLevel(int level)
    {
        viewLevel = level;
        rebuildAllChunks();
    }


    public bool[] nearbyOpaqueBlocks(Point loc)
    {
        var nearbyList = new bool[6];

        Point newLoc;
        int type;

        for (var i = 0; i < 6; i++)
        {
            newLoc = loc + G.transList[i];


            if (i == 5 && loc.y == 0)
                nearbyList[i] = true;


            if (!G.withinLim(newLoc, mapSize)) continue;

            type = getCell(newLoc).getTerrain().type;

            if (getCell(newLoc).getTerrain().hasProperty("Opaque"))
                nearbyList[i] = true;
            else if (getCell(newLoc).getTerrain().getType() == getCell(loc).getTerrain().getType())
                nearbyList[i] = true;
        }

        return nearbyList;
    }

    public void clearFace(Point loc)
    {
        var chunkLoc = getChunkLoc(loc);
        var blockLoc = getChunkBlockLoc(loc);

        //chunkList[chunkLoc.x, chunkLoc.z].addMesh(blockLoc, null);
    }

    private void rebuildAllChunks()
    {
        foreach (var chunk in chunkList)
            chunk.setupRebuild(viewLevel);
        //Debug.Log(chunk.loc);
    }
}