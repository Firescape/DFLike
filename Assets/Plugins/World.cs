
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

public class World : object
{
    public static Func<Point, Cell> getCell;
    public static World world;
    private Point mapSize = G.mapSize;
    [System.NonSerialized]
    private int viewLevel = 23;
    private string savePath = G.savePath;
    private Actor human2;
    private Actor human3;
    private List<BlockInput> blockInputList = new List<BlockInput>();
    public TerrainManager terrainManager;
    public ChunkManager chunkManager;
    public PathManager pathManager;
    public List<Actor> actorList = new List<Actor>();
    //public EntityManager entityManager;
    //private Dictionary<IndexType, EntityIndex> entityIndices = new Dictionary<IndexType, EntityIndex>();

    public World()
    {
        //foreach(IndexType index in Enum.GetValues(typeof(IndexType)))
        //{
        //entityIndices.Add(index, new EntityIndex(mapSize));
        //}

        terrainManager = new TerrainManager(mapSize);


        chunkManager = new ChunkManager(mapSize, new Point(16, 32, 16), terrainManager.getCell);
        pathManager = new PathManager(mapSize, terrainManager.getBlock);
        //entityManager = new EntityManager(mapSize, terrainManager, pathManager.getNode, terrainManager.getCell);


        World.getCell = terrainManager.getCell;
        //entityManager.setupActor(new Actor(), new Point(2, 12, 3));
        //entityManager.setupActor(new Actor(), new Point(3, 12, 3));
        //entityManager.setupActor(new Actor(), new Point(4, 12, 3));
        //terrainManager.entityManager = entityManager;
        //entityManager.blockInputList = blockInputList;


        int[,,] stpMap = new int[2, 1, 2];
        for (int i = 0; i < stpMap.GetLength(0); i++)
        {
            for (int i2 = 0; i2 < stpMap.GetLength(1); i2++)
            {
                for (int i3 = 0; i3 < stpMap.GetLength(2); i3++)
                {
                    stpMap[i, i2, i3] = 1;
                }
            }
        }

        //entityManager.createDispatcher(new Dispatcher(1, stpMap, new int[]{BlockDef.getId("Plank")}), new Point(11, 12, 11), true);

        //entityManager.createDispatcher(new Dispatcher(1, stpMap, new int[] { BlockDef.getId("Plank") }), new Point(19, 12, 24), true);

        stpMap = new int[1, 1, 1];
        stpMap[0, 0, 0] = 1;
        chunkManager.setViewLevel(viewLevel);
        //entityManager.createDispatcher(new Dispatcher(3, stpMap, new int[]{ BlockDef.getId("Plank")}), new Point(18, 2, 12), true);

        selectCube = UnityEngine.Object.Instantiate(Resources.Load("WireCubePrefab")) as GameObject;

        setupActor(new Actor(), new Point(4, 12, 3));
    }

    public void setupActor(Actor actor, Point loc)
    {
        actorList.Add(actor);
        //if(avatar == null)
        //{
        //avatar = actor;
        //}

        loc = terrainManager.getTopMost(loc);
        //actor.nextTask += nextTask;
        actor.setLoc(loc);
        //actor.currentNode = getNode(loc);
        //actor.init();
        getCell(loc).addActor(actor);
        actorList.Add(actor);

        //foreach(int job in actor.jobList)
        //{
        //indexEntity(actor, IndexCategory.AvailableWokers, job);
        //}
    }


    Block getBlock(Point loc)
    {
        return terrainManager.getBlock(loc);
    }

    void moveActor(Actor actor, Point loc, Point newLoc)
    {
        //getCell(loc).moveActorFromCell(actor, getCell(newLoc));
    }

    IEnumerable<Cell> invalidatedBlockList;
    int selectMode = 0;
    Point selectStart = new Point(-1, -1, -1);
    Point selectEnd = new Point(-1, -1, -1);
    GameObject selectCube = new GameObject();


    public void CheckInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        GridHit hit = CastRay.castRay(64f, mapSize, getBlock);

        Vector3 cameraLoc = Camera.main.transform.position;
        Point currentLoc = Point.ToPoint(cameraLoc);

        if (Input.GetKeyDown("q"))
        {
            viewLevel--;
            chunkManager.setViewLevel(viewLevel);
        }
        else if (Input.GetKeyDown("e"))
        {
            viewLevel++;
            chunkManager.setViewLevel(viewLevel);
        }

        if (selectMode == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit != null)
                {
                    removeBlock(hit.pos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && !Input.GetMouseButton(1))
            {

                //float start = Time.realtimeSinceStartup;
                //if(hit != null)
                //{
                //entityManager.avatar.sendTo(new Point(hit.pos.x, hit.pos.y + 1, hit.pos.z));
                //}
                //Debug.Log("all updates took " + (Time.realtimeSinceStartup - start));

                if (hit != null)
                {
                    int[,,] stpMap = new int[1, 1, 1];
                    stpMap[0, 0, 0] = 1;


                }
            }
            else if (Input.GetKeyDown("p"))
            {
                if (hit != null)
                {
                    Debug.Log("Clicked " + hit.pos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hit != null)
                {
                    addBlock(hit.pos, 4, hit.side);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //actorList[0].sendTo(hit.pos);
            }
        }
        else if (selectMode == 1)
        {
            if (hit != null)
            {
                selectEnd = hit.pos + G.transList[hit.side];
                selectEnd.y = selectStart.y;

                //selectStart = 
                Debug.Log(selectStart + "," + selectEnd);
                DspMap map = createMap(selectStart, selectEnd);

                selectCube.GetComponent<WireCube>().size = new Vector3(map.GetLength(0), map.GetLength(1), map.GetLength(2));
                selectCube.GetComponent<WireCube>().center = map.start.ToVector3() + selectCube.GetComponent<WireCube>().size / 2;
                selectCube.GetComponent<WireCube>().draw = true;

                if (Input.GetMouseButtonUp(2))
                {

                    selectMode = 0;
                }
            }
        }
    }

    public void Update()
    {
        //int i;
        if (G.debugString != "")
        {
            Debug.Log(G.debugString);
            G.debugString = "";
        }

        foreach (Actor actor in actorList)
        {
            actor.Update();
        }


        invalidatedBlockList = terrainManager.submitBlockInput(blockInputList);
        blockInputList.Clear();
        chunkManager.submitUpdateList(invalidatedBlockList);
        pathManager.submitUpdateList(invalidatedBlockList);
        terrainManager.invalidatedBlockList.Clear();
    }

    public DspMap createMap(Point start, Point end)
    {
        Point newLoc = end - start + new Point(1, 1, 1);
        Point tempLoc = new Point(0, 0, 0);
        Debug.Log(newLoc);
        if (newLoc.x < 0 && newLoc.z < 0)
        {
            tempLoc = start;
            start = end;
            end = tempLoc - new Point(1, 0, 1);
        }
        else if (newLoc.z < 0)
        {
            Debug.Log(start + ", " + end);
            tempLoc = start;
            start.z = end.z;
            end.z = tempLoc.z - 1;
            Debug.Log(start + ", " + end);
        }
        else if (newLoc.x < 0)
        {
            tempLoc = start;
            start.x = end.x;
            end.x = tempLoc.x - 1;
        }

        if (newLoc.x == 0)
        {
            //end.x++;
        }

        if (newLoc.z == 0)
        {
            //end.z++;
        }

        newLoc = end - start + new Point(1, 1, 1);
        Debug.Log(newLoc);
        //newLoc = newLoc.

        int[,,] map = new int[newLoc.x, newLoc.y, newLoc.z];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int i2 = 0; i2 < map.GetLength(1); i2++)
            {
                for (int i3 = 0; i3 < map.GetLength(2); i3++)
                {
                    newLoc = start + new Point(i, i2, i3);
                    if (getCell(newLoc).getTerrain().getType() == 0 &&
                       getCell(newLoc + new Point(0, -1, 0)).getTerrain().getType() > 0)
                    {
                        map[i, i2, i3] = 1;
                    }
                    else
                    {
                        map[i, i2, i3] = 0;
                    }
                }
            }
        }

        return new DspMap(start, map);
    }

    public void removeBlock(Point loc)
    {
        //blockInputList.Add(new BlockInput(new Block(0), loc));
        //testLambda();
        //testLambda2();
        //tester(16)();
        int[,,] stpMap = new int[1, 1, 1];
        for (int i = 0; i < stpMap.GetLength(0); i++)
        {
            for (int i2 = 0; i2 < stpMap.GetLength(1); i2++)
            {
                for (int i3 = 0; i3 < stpMap.GetLength(2); i3++)
                {
                    stpMap[i, i2, i3] = 1;
                }
            }
        }

        //Debug.Log("breaking dispatcher " + getCell(loc).dispatcher);
    }

    public void addBlock(Point loc, System.Byte val, int side)
    {
        blockInputList.Add(new BlockInput(new Block(val), loc + G.transList[side]));
    }

}

