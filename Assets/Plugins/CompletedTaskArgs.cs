using System;
using System.Collections;

public class CompletedTaskArgs : EventArgs
{
    public Entity source;

    public CompletedTaskArgs(Entity source)
    {
        this.source = source;
    }
}