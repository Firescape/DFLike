using System.Collections.Generic;
using System.Collections;
using System;

public abstract class Entity: object, IIndexable
{
	public Point loc;
	
	
	//public static int currId = 0;
	//public int uid = ++currId;
	Guid id = Guid.NewGuid();
	public static Func<Point, Node> getNode;
	public static Func<Point, Cell> getCell;
	public static List<BlockInput> blockInputList;
	public static Func<IIndexable, IndexType, bool> indexEntity;
	public static Action<IIndexable, IndexType, int> deIndexEntity;
	//public static Func<Dispatcher, bool> dispatch;
	public static Func<int, IndexType, IIndexable, Action<IIndexable>, Item2> findItem;
	//public static EntityManager entityManager;
	public static Func<int, Point, Actor> findWorker;
	
	
	private IIndexable reserver;
	public IIndexable Reserver
	{
		get
		{
			return reserver;
		}
		set 
		{
			IIndexable prev = reserver;
			reserver = value;
			if(prev != null && value == null)
			{
				//UnityEngine.Debug.Log("freed reserve");
				OnDereserve(this);
			}
		}
	}
	public event Action<IIndexable> OnDereserve = delegate {};
	public event Action OnDeIndex = delegate {};
	
	public int notSaved = 5;
	
	
	
	
	public Entity()
	{ 
		
	}
	
	public void deIndex()
	{
		OnDeIndex();
		OnDeIndex = delegate {};
	}
	
	public Mesh2 getMesh()
	{
		return new Mesh2();
	}
	
	public virtual List<byte> getBytes()
	{
		return new List<byte>();
	}
	
	private int generateId()
	{
		//return GlobalList.GetInstance().generateId();
		return 1;
	}

	public Point getLoc()
	{
		return loc;
	}
	
	public abstract int[] getType();
	
}
