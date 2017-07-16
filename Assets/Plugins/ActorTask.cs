using System;
using System.Collections.Generic;

public enum ActorTaskState
{
	NotStarted,
	InProgress,
	Complete
}

public enum ActorTaskType
{
	None, 
	Move,
    Pickup,
    DropOff,
    Break
}

public class ActorTask: object
{
	//time in frames
	public int delay;
	public int delayCounter = 0;
	public int? framesPerTile;
	public List<Node> path;
	public Action onComplete;
	public ActorTaskState state = ActorTaskState.NotStarted;
	public ActorTaskType type;
	public Point startLoc;
	public Point destLoc;
    public Item2 item;
    public Point target;

	public ActorTask(ActorTaskType type, int delay)
	{
		this.type = type;
		this.delay = delay;
	}

	public void Start()
	{
		state = ActorTaskState.InProgress;
	}

	public void Update()
	{

	}

}

