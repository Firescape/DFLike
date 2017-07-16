using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml;

public class BlockDef
{	
	private static BlockDef Instance = new BlockDef();

	public static BlockDef GetInstance ()
	{
		return Instance;
	}

		
	public Hashtable[] table = new Hashtable[256];
	public Dictionary<string, int> typeToId = new Dictionary<string, int>();
	
	public BlockDef()
	{
		XMLParser parser =new XMLParser(); 

		XMLNode nodes = parser.Parse(File.ReadAllText("BlockDefs.xml")); 
		//System.IO.read
		int blockCount = nodes.GetNodeList("block").Count;
		int propertyCount;
		int id;

		for(int i = 0; i < blockCount; i++)
		{

			propertyCount = nodes.GetNodeList("block>" + i + ">property").Count;
			id = int.Parse(nodes.GetValue("block>" + i + ">@id"));
			table[id] = new Hashtable();
			table[id].Add("Type", nodes.GetValue("block>" + i + ">@name"));
			typeToId.Add(nodes.GetValue("block>" + i + ">@name"), id);

			for(int i2 = 0; i2 < propertyCount; i2++)
			{
				//nodes.GetValue("block>" + i + ">property" + i2 + ">@type")
				if(nodes.GetValue("block>" + i + ">property>" + i2 + ">@type") == null)
				{

					table[id].Add(nodes.GetValue("block>" + i + ">property>" + i2 + ">@name"), true);
				}
				else
				{

					table[id].Add(nodes.GetValue("block>" + i + ">property>" + i2 + ">@name"), nodes.GetValue("block>" + i + ">property" + i2 + "_text"));
				}

			}
		}
	}

	
	public static int getId(string type)
	{

		return Instance.typeToId[type];
	}

	

	public static string getType(int id)
	{

		return Instance.table[id]["Type"] as string;
	}

}
