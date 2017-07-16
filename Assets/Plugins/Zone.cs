using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ZoneType
{
	None = 0,
	Stockpile = 1
}

public class Zone: object
{
	public Point loc;
	public ZoneType type;
	
	public Zone(Point loc)
	{
		this.loc = loc;
	}
}

