using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class World : object
{
    public static Func<Point, Cell> getCell;
    public static World world;
    public List<Actor> actorList = new List<Actor>();
    private List<BlockInput> blockInputList = new List<BlockInput>();
    public ChunkManager chunkManager;
    private Actor human2;
    private Actor human3;

    private IEnumerable<Cell> invalidatedBlockList;
    private Point mapSize = G.mapSize;
    public PathManager pathManager;
    private string savePath = G.savePath;
    private GameObject selectCube = new GameObject();
    private Point selectEnd = new Point(-1, -1, -1);
    private int selectMode = 0;
    private Point selectStart = new Point(-1, -1, -1);
    public TerrainManager terrainManager;

    [NonSerialized] private int viewLevel = 23;
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


        getCell = terrainManager.getCell;
        //entityManager.setupActor(new Actor(), new Point(2, 12, 3));
        //entityManager.setupActor(new Actor(), new Point(3, 12, 3));
        //entityManager.setupActor(new Actor(), new Point(4, 12, 3));
        //terrainManager.entityManager = entityManager;
        //entityManager.blockInputList = blockInputList;


        var stpMap = new int[2, 1, 2];
        for (var i = 0; i < stpMap.GetLength(0); i++)
        for (var i2 = 0; i2 < stpMap.GetLength(1); i2++)
        for (var i3 = 0; i3 < stpMap.GetLength(2); i3++)
            stpMap[i, i2, i3] = 1;

        //entityManager.createDispatcher(new Dispatcher(1, stpMap, new int[]{BlockDef.getId("Plank")}), new Point(11, 12, 11), true);

        //entityManager.createDispatcher(new Dispatcher(1, stpMap, new int[] { BlockDef.getId("Plank") }), new Point(19, 12, 24), true);

        stpMap = new int[1, 1, 1];
        stpMap[0, 0, 0] = 1;
        chunkManager.setViewLevel(viewLevel);
        //entityManager.createDispatcher(new Dispatcher(3, stpMap, new int[]{ BlockDef.getId("Plank")}), new Point(18, 2, 12), true);

        selectCube = Object.Instantiate(Resources.Load("WireCubePrefab")) as GameObject;

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


    private Block getBlock(Point loc)
    {
        return terrainManager.getBlock(loc);
    }

    private void moveActor(Actor actor, Point loc, Point newLoc)
    {
        //getCell(loc).moveActorFromCell(actor, getCell(newLoc));
    }


    public void CheckInput()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hit = CastRay.castRay(64f, mapSize, getBlock);

        var cameraLoc = Camera.main.transform.position;
        var currentLoc = Point.ToPoint(cameraLoc);

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
                    removeBlock(hit.pos);
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
                    var stpMap = new int[1, 1, 1];
                    stpMap[0, 0, 0] = 1;
                }
            }
            else if (Input.GetKeyDown("p"))
            {
                if (hit != null)
                    Debug.Log("Clicked " + hit.pos);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hit != null)
                    addBlock(hit.pos, 4, hit.side);
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
                var map = createMap(selectStart, selectEnd);

                selectCube.GetComponent<WireCube>().size =
                    new Vector3(map.GetLength(0), map.GetLength(1), map.GetLength(2));
                selectCube.GetComponent<WireCube>().center =
                    map.start.ToVector3() + selectCube.GetComponent<WireCube>().size / 2;
                selectCube.GetComponent<WireCube>().draw = true;

                if (Input.GetMouseButtonUp(2))
                    selectMode = 0;
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

        foreach (var actor in actorList)
            actor.Update();


        invalidatedBlockList = terrainManager.submitBlockInput(blockInputList);
        blockInputList.Clear();
        chunkManager.submitUpdateList(invalidatedBlockList);
        pathManager.submitUpdateList(invalidatedBlockList);
        terrainManager.invalidatedBlockList.Clear();
    }

    public DspMap createMap(Point start, Point end)
    {
        var newLoc = end - start + new Point(1, 1, 1);
        var tempLoc = new Point(0, 0, 0);
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

        var map = new int[newLoc.x, newLoc.y, newLoc.z];

        for (var i = 0; i < map.GetLength(0); i++)
        for (var i2 = 0; i2 < map.GetLength(1); i2++)
        for (var i3 = 0; i3 < map.GetLength(2); i3++)
        {
            newLoc = start + new Point(i, i2, i3);
            if (getCell(newLoc).getTerrain().getType() == 0 &&
                getCell(newLoc + new Point(0, -1, 0)).getTerrain().getType() > 0)
                map[i, i2, i3] = 1;
            else
                map[i, i2, i3] = 0;
        }

        return new DspMap(start, map);
    }

    public void removeBlock(Point loc)
    {
        //blockInputList.Add(new BlockInput(new Block(0), loc));
        //testLambda();
        //testLambda2();
        //tester(16)();
        var stpMap = new int[1, 1, 1];
        for (var i = 0; i < stpMap.GetLength(0); i++)
        for (var i2 = 0; i2 < stpMap.GetLength(1); i2++)
        for (var i3 = 0; i3 < stpMap.GetLength(2); i3++)
            stpMap[i, i2, i3] = 1;

        //Debug.Log("breaking dispatcher " + getCell(loc).dispatcher);
    }

    public void addBlock(Point loc, byte val, int side)
    {
        blockInputList.Add(new BlockInput(new Block(val), loc + G.transList[side]));
    }
}