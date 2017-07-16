using System;

public interface IIndexable:ILocatable
{
	
	int[] getType();
	event Action OnDeIndex;
	IIndexable Reserver {get; set;}
	event Action<IIndexable> OnDereserve;
}

public interface ILocatable
{
	Point getLoc();
}
