public class CellEventArgs : System.EventArgs
{
	public enum CellEventType
	{
		BlockRemoval,
		BlockAddition,
		BlockSwap,
		ItemRemoval,
		ItemAddition
	}
	
	public CellEventType cellEventType;
	
	public CellEventArgs(CellEventType cellEventType)
	{
		this.cellEventType = cellEventType;
	}
}
