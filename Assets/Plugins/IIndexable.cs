using System;

public interface IIndexable : ILocatable
{
    IIndexable Reserver { get; set; }

    int[] getType();
    event Action OnDeIndex;
    event Action<IIndexable> OnDereserve;
}

public interface ILocatable
{
    Point getLoc();
}