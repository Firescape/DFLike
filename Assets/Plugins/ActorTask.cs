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

public class ActorTask : object
{
    //time in frames
    public int delay;

    public int delayCounter = 0;
    public Point destLoc;
    public int? framesPerTile;
    public Item2 item;
    public Action onComplete;
    public List<Node> path;
    public Point startLoc;
    public ActorTaskState state = ActorTaskState.NotStarted;
    public Point target;
    public ActorTaskType type;

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