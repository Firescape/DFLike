using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml;

public class JobDef
{
	private static JobDef Instance = new JobDef();

//	public static JobDef GetInstance()
//	{
//		return Instance;
//	}


	public Hashtable[] table = new Hashtable[256];
	public Dictionary<string, int> typeToId = new Dictionary<string, int>();

	public JobDef()
	{
		XMLParser parser = new XMLParser();
		
		XMLNode nodes = parser.Parse(File.ReadAllText("JobDefs.xml"));
		
		int jobCount = nodes.GetNodeList("job").Count;
		int propertyCount;
		int id;
		
		for(int i = 0; i < jobCount; i++)
		{
			
			propertyCount = nodes.GetNodeList("job>" + i + ">property").Count;
			id = int.Parse(nodes.GetValue("job>" + i + ">@id"));
			table[id] = new Hashtable();
			table[id].Add("Type", nodes.GetValue("job>" + i + ">@name"));
			typeToId.Add(nodes.GetValue("job>" + i + ">@name"), id);
			
			for(int i2 = 0; i2 < propertyCount; i2++) 
			{
				//nodes.GetValue("job>" + i + ">property" + i2 + ">@type")
				if(nodes.GetValue("job>" + i + ">property>" + i2 + ">@type") == null) 
				{
					
					table[id].Add(nodes.GetValue("job>" + i + ">property>" + i2 + ">@name"), true);
				}

				else {
					
					table[id].Add(nodes.GetValue("job>" + i + ">property>" + i2 + ">@name"), nodes.GetValue("job>" + i + ">property" + i2 + "_text"));
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
