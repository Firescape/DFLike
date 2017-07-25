using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Object = UnityEngine.Object;

public enum ActorState
{
    Idle,
    Moving,
    Interacting,
    Attacking
}

public class Actor : Entity, IIndexable
{
    public Node currentNode;
    public ActorTask currentTask;

    [NonSerialized] public GameObject gameObject;

    public Item2 heldObject;

    [NonSerialized] private GameObject heldObjectMesh;

    private int jobCount = 3;
    public int[] jobList;
    public Node nextNode;
    private Point nextTile;
    public List<Node> path = new List<Node>();
    public Cell reservedCell;
    public Entity reservedItem;
    private float rotation = 0;
    public float speed;
    public ActorState state = ActorState.Idle;
    public List<ActorTask> taskQueue = new List<ActorTask>();
    public Transform transform;
    public Vector3 velocity;

    public Actor()
    {
        //gameObject = GlobalList.GetInstance().mainClass.Instantiate(Resources.Load("HumanPrefab")) as GameObject;

        jobList = new int[jobCount];

        jobList[0] = JobDef.getId("Lumberjack");
        jobList[1] = JobDef.getId("Carpenter");
        jobList[2] = JobDef.getId("Hauler");
        speed = 0.2f;

        gameObject = Object.Instantiate(Resources.Load("HumanPrefab")) as GameObject;
        gameObject.GetComponent<ActorGameObject>().actor = this;
        snapToGrid();
        World.getCell(loc).addActor(this);
        transform = gameObject.transform;
    }

//	
//
    public Point getLoc()
    {
        return loc;
    }

    public override int[] getType()
    {
        return jobList;
    }

    public event Action<Actor> nextTask;

//	
//	
//	public void init()
//	{
//		gameObject = GameObject.Instantiate(Resources.Load("HumanPrefab")) as GameObject;
//		gameObject.GetComponent<ActorGameObject>().actor = this;
//		snapToGrid();
//		World.getCell(loc).addActor(this);
//		//nextTask(this);
//		this.transform = gameObject.transform;
//	}
//	
    public void setLoc(Point loc)
    {
        //transform.position = new Vector3(loc.x, loc.y, loc.z);
        this.loc = loc;
        snapToGrid();
    }

//	
//	public void queueItemPickup(Item2 item)
//	{
//		//Hashtable table = iTween.Hash("time", 0, "onComplete", "pickupEntity", "onCompleteParams", item);
//	
//		//taskQueue.Add(table);
//		//taskQueue.Add(new CivTask(0, pickupItem));
//	}
//
//	public void nextTask2()
//	{
//		this.nextTask(this);
//	}
//	
//	
//	public void pickupItem (Item2 item)
//	{
//		//setState(ActorState.Interacting);
//
//        heldObject = item;
//        heldObjectMesh = GameObject.Instantiate(Resources.Load("EquipPrefab")) as GameObject;
//        heldObjectMesh.GetComponent<MeshFilter>().mesh = Mesh.Instantiate(item.getMesh().toMesh()) as Mesh;
//        heldObjectMesh.transform.position = gameObject.transform.position + new Vector3(0.59f, 1.78f, 0.27f);
//        heldObjectMesh.transform.parent = gameObject.transform;
//        //heldObjectMesh = item.getMesh().toMesh();
//
//		//GlobalList.GetInstance().mainClass.pickupItem(entity, this);
//		//
//		//heldObjectMesh = Instantiate(Resources.Load("EquipPrefab")) as GameObject;
//		//heldObjectMesh.GetComponent<MeshFilter>().mesh = Mesh.Instantiate(entity.getMesh()) as Mesh;
//		//
//		//heldObjectMesh.transform.position = gameObject.transform.position + new Vector3(0.8f, 1.42f, 0.71f);
//		//
//		//heldObjectMesh.transform.parent = gameObject.transform.Find("HumanModel").transform.Find("RightArm");
//		//heldObjectMesh.transform.localPosition = new Vector3(0.003705188f, 0.003034122f, -0.004560475f);
//		//heldObjectMesh.transform.localEulerAngles = new Vector3(0, 0, 0);
//		//print(gameObject.transform.Find("HumanModel"));
//	}
//
//    public void removeItem()
//    {
//        heldObject = null;
//        GameObject.Destroy(heldObjectMesh);
//    }
//	
//	public void enterJobQueue()
//	{
//		//setState(ActorState.WaitingForJob);
//		for(int i = 0; i < jobCount; i++)
//		{
//			//if(jobList[i])
//			//{
//				//GlobalList.GetInstance().addActor(this, jobList[i]);
//			//}
//		}
//	}
//	
//	public void leaveJobQueue()
//	{
//		for(int i = 0; i < jobCount; i++)
//		{
//			
//		}
//	}
//	
    public void Update()
    {
        if (state == ActorState.Idle)
            if (taskQueue.Count > 0)
            {
                currentTask = taskQueue[0];
                taskQueue.RemoveAt(0);
            }

        if (currentTask != null)
            if (path == null)
            {
            }
    }

//	
//	
    public void snapToGrid()
    {
        gameObject.transform.position = new Vector3(loc.x, loc.y, loc.z);
        nextTile = loc;
        Debug.Log(loc);
    }
//	
//	public void sendTo(Point destLoc)
//	{
//		//taskQueue.Add(iTween.Hash("time",0, 
//		//               "onComplete","determinePath",
//		//               "onCompleteParams",loc));
//		//taskQueue.Add(new CivTask(0, 13, 
//		taskQueue.Add(new ActorTask(ActorTaskType.Move, 0));
//		taskQueue[taskQueue.Count - 1].startLoc = loc;
//		taskQueue[taskQueue.Count - 1].destLoc = destLoc;
//		Debug.Log("sending actor to " + destLoc);
//		//taskQueue
//		//if(state == ActorState.WaitingForJob)
//		//{
//		//    leaveJobQueue();
//		//    nextTask(this);
//		//}
//	}
//
//    public void sendToAndPickup(Point destLoc, Item2 item)
//    {
//        sendTo(destLoc);
//        taskQueue.Add(new ActorTask(ActorTaskType.Pickup, 0));
//        taskQueue[taskQueue.Count - 1].item = item;

//    }
//
//    public void sendToAndBreak(Point accessLoc, Point blockLoc)
//    {
//        sendTo(accessLoc);
//        taskQueue.Add(new ActorTask(ActorTaskType.Break, 0));
//        taskQueue[taskQueue.Count - 1].target = blockLoc;
//    }
//
//    public void sendToPickupDropOff(Point itemLoc, Item2 item, Point destLoc)
//    {
//        sendToAndPickup(itemLoc, item);
//        sendTo(destLoc);
//        taskQueue.Add(new ActorTask(ActorTaskType.DropOff, 0));
//    }
//	
//	public void determinePath(Node destNode)
//	{
//		Debug.Log("determining path to " + loc);
//		Node start = currentNode;
//		Node destination = destNode;
//		
//		if(destination != null)
//		{
//			float start1 = Time.realtimeSinceStartup;
//			path = Pathfinder2.findPath(start, destination);
//			
//			if(path != null)
//			{
//				nextPath();
//			}
//			else
//			{
//                nextTask(this);
//			}
//		}
//		else
//		{
//			nextTask(this);
//		}
//		
//	}
//	
//	public void setState(ActorState newState)
//	{
//		if(allowedStateChange(newState) && newState != state)
//		{
//			switch (newState)
//			{
//				default:
//					break;
//			}
//			
//			state = newState;
//		}
//		
//	}
//	
//	private bool allowedStateChange(ActorState newState)
//	{
//		switch (newState)
//		{
//			default:
//				return true;
//		}
//	}
//	
//	
//	public void rotate(Vector3 shift)
//	{
//		if(shift.x > 0 && shift.z > 0)
//		{
//			rotation = 45;
//		}
//		else if(shift.x < 0 && shift.z < 0)
//		{
//			rotation = 225;
//		}
//		else if(shift.x < 0 && shift.z > 0)
//		{
//			rotation = 315;
//		}
//		else if(shift.x > 0 && shift.z < 0)
//		{
//			rotation = 135;
//		}
//		else if(shift.x > 0)
//		{
//			rotation = 90;
//		}
//		else if(shift.x < 0)
//		{
//			rotation = 270;
//		}
//		else if(shift.z > 0)
//		{
//			rotation = 0;
//		}
//		else if(shift.z < 0)
//		{
//			rotation = 180;
//		}
//		
//		foreach(Transform child in gameObject.transform)
//		{
//			child.transform.eulerAngles = new Vector3(0, rotation, 0);
//			gameObject.transform.Find("rick").transform.eulerAngles = new Vector3(0, rotation, 0);
//		}
//	}
//
//	public bool moveTo(Point destLoc)
//	{
//		Vector3 shift = new Vector3(destLoc.x - transform.position.x, destLoc.y - transform.position.y, destLoc.z - transform.position.z).normalized * speed;
//		rotate(shift);
//
//		Vector3 newLoc = shift + transform.position;
//
//		if(shift.x > 0 && newLoc.x > destLoc.x)
//		{
//			newLoc.x = destLoc.x;
//		}
//		else if(shift.x < 0 && newLoc.x < destLoc.x)
//		{
//			newLoc.x = destLoc.x;
//		}
//
//		if(shift.y > 0 && newLoc.y > destLoc.y)
//		{
//			newLoc.y = destLoc.y;
//		}
//		else if(shift.y < 0 && newLoc.y < destLoc.y)
//		{
//			newLoc.y = destLoc.y;
//		}
//
//		if(shift.z > 0 && newLoc.z > destLoc.z)
//		{
//			newLoc.z = destLoc.z;
//		}
//		else if(shift.z < 0 && newLoc.z < destLoc.z)
//		{
//			newLoc.z = destLoc.z;
//		}
//
//		gameObject.transform.position = newLoc;
//
//		if(newLoc == destLoc.ToVector3())
//		{
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
//	
//	public List<byte> getBytes()
//	{
//		return new List<byte>();
//	}
//	
//	public void nextPath()
//	{
//		World.getCell(nextTile).moveActorFromCell(this, World.getCell(loc));
//		loc = nextTile;
//		currentNode = path[0];
//		nextTile = path[0].getLoc();
//		path.RemoveAt(0);
//		Vector3 shift = new Vector3(nextTile.x - gameObject.transform.position.x, nextTile.y - gameObject.transform.position.y, nextTile.z - gameObject.transform.position.z).normalized;
//		
//		
//		float speed = 0.2f;
//		
//		if(Mathf.Abs(shift.x) + Mathf.Abs(shift.y) + Mathf.Abs(shift.z) >= 2)
//		{
//			speed = speed * 1.4f;
//		}
//		iTween.MoveTo(this.gameObject, iTween.Hash("x", nextTile.x,
//													"y", nextTile.y,
//													"z", nextTile.z,
//													"EaseType", "linear",
//													"time", speed,
//													"onComplete", "nextTask"));
//			
//		setState(ActorState.Moving);
//	}
}