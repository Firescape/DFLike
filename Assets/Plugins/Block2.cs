using System;

public struct Block2
{
	//10 bytes
	//uint data1;
	//uint data2;
	int type;
	int[] lol;
	//int type1;
	//int type2;
	//int type3;
	//int type4;
	//int type5;
	//int type6;
	//int type7;
	
	public Block2(int type)
	{
		this.type = type;
		lol = new int[128];
		lol.Initialize();
		//this.type1 = type;
		//this.type2 = type;
		//this.type3 = type;
		//this.type4 = type;
		//this.type5 = type;
		//this.type6 = type;
		
	}
	
	
	
	public void shift(byte val)
	{
		type = type | val;
	}
	
	public string ToString()
	{
		return "Type:" +  String.Format("{000:X}", type);
	}
}


