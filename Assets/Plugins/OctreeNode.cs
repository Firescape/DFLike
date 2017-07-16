using UnityEngine;
using System.Collections.Generic;
using System;

public class OctreeNode: object
{
	public OctreeNode[] octants;
	private Bounds bounds;
	public List<IIndexable> values;
	
	//@System.NonSerialized
	public GameObject wireCube;
	
	public OctreeNode(Vector3 center, Vector3 size)
	{
		if(Mathf.IsPowerOfTwo((int)size.x) && Mathf.IsPowerOfTwo((int)size.y) && Mathf.IsPowerOfTwo((int)size.z) && size.x == size.y && size.x == size.z)
		{
			
		}
		else
		{
			Debug.Log("Octree node size must be unit size and power of two \n the actual center/size is: " + center + " | " + size + " changing size to .x ");
			size = new Vector3(size.x, size.x, size.x);
		}
		
		this.bounds = new Bounds(center, size);
		
		//Debug.Log("The size of the new node is " + bounds.size);
		//Debug.Log("The center of the new node is " + bounds.center);
		//
		wireCube = UnityEngine.Object.Instantiate(Resources.Load("WireCubePrefab")) as GameObject;
		//wireCube.GetComponent
		wireCube.GetComponent<WireCube>().size = bounds.size;
		wireCube.GetComponent<WireCube>().center = bounds.center;// + new Vector3(-5, -5, -5);
		wireCube.GetComponent<WireCube>().draw = true;
	}
	
	public bool isEmpty()
	{
		if(octants == null)
		{
			if(values == null)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public bool isLeaf()
	{
		if(values != null)
		{
			return true;
		}
		
		return false;
	}

    public String debug()
    {
        String str = "";

        if (values == null)
        {
            if (octants != null)
            {
                foreach (OctreeNode child in octants)
                {
                    str = str + child.debug();
                }
            }
        }
        else
        {
            str =  "\t" + "\t" + values[0].getLoc().ToString() + "x" + values.Count  + "\n";
        }

        return str;
    }
	
	public bool isInternal()
	{
		if(octants != null)
		{
			return true;
		}
		
		return false;
	}
	
	public IIndexable getValue(Vector3 loc)
	{
		if(values == null)
		{
			if(octants != null)
			{
				octants[getOctant(loc)].getValue(loc);
			}
			else
			{
				return null;
			}
		}

		return values[0];
	}
	
	public bool removeValue(IIndexable value)
	{
		if(values == null)
		{
			if(octants != null)
			{
				//if a sub octant succesfuly removes a value
				//attempt to consolidate the octant
                //Debug.Log("checking sub octants");
				if(octants[getOctant(value.getLoc().ToVector3())].removeValue(value))
				{
					int leafCount = 0;
					int lastEmpty = -1;
					//check every octant
					for(int i = 0; i < 8; i++)
					{
						//if that octant is a leaf
						//ie contains a value
						//increase leaf counter
						//mark down its index for later use
						if(octants[i].isLeaf())
						{
							lastEmpty = i;
							leafCount++;
						}
						//if that octant is a internal 
						//ie has suboctants of its own
						//then it's not possible to consolidate octant
						else if(octants[i].isInternal())
						{
							leafCount = -1;
							break;
						}
					}
					
					if(leafCount == 0 || leafCount == 1)
					{
						//Debug.Log("consolidated octants");
						for(int i = 0; i < 8; i++)
						{
							//Debug.Log(i);
							UnityEngine.Object.Destroy(octants[i].wireCube);
						}
					}
					
					if(leafCount == 1)
					{
						values = new List<IIndexable>(1);
						values.AddRange(octants[lastEmpty].values);
						octants[lastEmpty].values = null;
						octants = null;
						
						return true;
					}
					else if(leafCount == 0)
					{
						octants = null;
						
						return true;
					}
					
				}
				
				return false;
			}
			else
			{
				throw(new SystemException("Reached a dead end. Could not remove value at location " + value.getLoc()));
			}
		}
		else
		{
			//Debug.Log("removed item");
			values.Remove(value);
			if(values.Count == 0)
			{
				values = null;
				return true;
			}
			return false;
		}
		
		throw(new SystemException("this throw statement should not have been reached"));
	}
	
	
	
	public void insertValue (IIndexable value)
	{
		if(values == null && octants == null)
		{
			values = new List<IIndexable>(1);
			values.Add(value);
		}
		else if(values != null && value.getLoc() == values[0].getLoc())
		{
			values.Add(value);
		}
		else if(bounds.size.x > 1 && bounds.size.y > 1 && bounds.size.z > 1)
		{
			if(octants == null)
			{
				octants = new OctreeNode[8];
				
				for(int i = 0; i < 8; i++)
				{
					octants[i] = new OctreeNode(bounds.center + (OctreeNode.octantOffset[i] * bounds.size.x) / 4, bounds.size / 2); 
				}
			}
			
			if(values != null)
			{
				octants[getOctant(values[0].getLoc().ToVector3())].insertValue(values[0]);
				values = null;
			}
			
			octants[getOctant(value.getLoc().ToVector3())].insertValue(value);
		}
		else
		{
			values.Add(value);
		}
		
		
	}
	
	//0: -1, -1, -1
	//1: -1, -1,  1
	
	//2: -1,  1, -1
	//3: -1,  1,  1
	
	//4:  1, -1, -1
	//5:  1, -1,  1
	
	//6:  1,  1, -1
	//7:  1,  1,  1
	
	private static Vector3[] octantOffset = 
	new Vector3[]{new Vector3(-1, -1, -1),
	 new Vector3(-1, -1, 1),
	 
	 new Vector3(-1, 1, -1),
	 new Vector3(-1, 1, 1),
	 
	 new Vector3(1, -1, -1),
	 new Vector3(1, -1, 1),
	 
	 new Vector3(1, 1, -1),
	 new Vector3(1, 1, 1)};
	
	private int getOctant(Vector3 loc)
	{
		Vector3 center = bounds.center;
		
		if(loc.x < center.x)
		{
			if(loc.y < center.y)
			{
				if(loc.z < center.z)
				{
					return 0;
				}
				else
				{
					return 1;
				}
			}
			else
			{
				if(loc.z < center.z)
				{
					return 2;
				}
				else
				{
					return 3;
				}
			}
		}
		else
		{
			if(loc.y < center.y)
			{
				if(loc.z < center.z)
				{
					return 4;
				}
				else
				{
					return 5;
				}
			}
			else
			{
				if(loc.z < center.z)
				{
					return 6;
				}
				else
				{
					return 7;
				}
			}
		}
		
		throw(new SystemException("getoctant error, someone made a booboo because this error should not have even been reached"));
	}
	
	public IIndexable findClosest(Vector3 loc)
	{
		//return values[0];
		if(values != null)
		{
			return values[0];
		}
		else if(octants != null)
		{
            foreach (OctreeNode octant in octants)
            {
                if (octant.isEmpty() == false)
                {
                    return octant.findClosest(loc);
                }
            }

            //if(octants[getOctant
		}

		return null;
		
	}
}
