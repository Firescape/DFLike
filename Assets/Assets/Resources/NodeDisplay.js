
public var tMap:Node[,,];
public var loc:Vector3;

function OnDrawGizmosSelected ()
{
	//if(target != null)
	//{
		// Draws a blue line from this transform to the target
		//Gizmos.color = Color.blue;
		//Gizmos.DrawLine (new Vector3(0, 0, 0), new Vector3(1, 0, 1));
	//}
	
	Gizmos.color = Color.blue;
	
	var locNode:Node = tMap[loc.x, loc.y, loc.z];
	
	//Debug.Log(tMap[15, 17, 15]);
	
	var vertexList:Vector3[] = new Vector3[2];
	
	vertexList[0] = new Vector3(locNode.x + 0.5, locNode.y + 0.05, locNode.z + 0.5);
	//clearVectors();
	for(var i:int = 0; i < 18; i++)
	{
		if(locNode.linkedNodes[i])
		{
			vertexList[1] = new Vector3(locNode.linkedNodes[i].x + 0.5, locNode.linkedNodes[i].y + 0.05, locNode.linkedNodes[i].z + 0.5);
			
			if(locNode.linkSizes[i]== -1)
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = Color.blue;
			}
			
			Gizmos.DrawLine (vertexList[0], vertexList[1]);
			//Vector.DrawLine3D (vectors[i]);
		}
	}
}

//public function newDebugText(loc:Point, text:String):void
//{
	//if(text == "")
	//{
		//
		//if(gLayout[loc.x, loc.y, loc.z])
		//{
			//Destroy(gLayout[loc.x, loc.y, loc.z]);
		//}
	//}
	//else if(gLayout[loc.x, loc.y, loc.z])
	//{
	//}
	//else
	//{
		//gLayout[loc.x, loc.y, loc.z] = Instantiate(Resources.Load("FacePrefab2"));
		//
		//gLayout[loc.x, loc.y, loc.z].GetComponent.<NodeDisplay>().tMap = terrainMap;
		//gLayout[loc.x, loc.y, loc.z].GetComponent.<NodeDisplay>().loc = loc;
		//gLayout[loc.x, loc.y, loc.z].transform.position = Point.add(loc + new Point(0.5, 0.01, 0.5));
	//}
//}