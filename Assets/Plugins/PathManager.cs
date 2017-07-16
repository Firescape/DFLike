using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathManager: object
{
	private Node[] pathMap;
	private Point mapSize;
	private Func<Point, Block> getBlock;
    

	public PathManager(Point mapSize, Func<Point, Block> getBlock)
	{
		this.mapSize = mapSize;
		this.getBlock = getBlock;
		
		setupMap();
	}
	
	public void submitUpdateList(IEnumerable<Cell> invalidatedBlockList)
	{
		foreach(Cell invBlock in invalidatedBlockList)
		{
			checkTerrain(invBlock.loc, true);
		}
	}

	private void checkTerrain(Point loc, bool checkNearby)
	{
		Point newLoc = loc + G.transList[0];
		
		if(getBlock(loc).getType() > 0) 
		{
			if(G.withinLim(newLoc + G.transList[0], mapSize))
			{
				if(getNode(newLoc) == null && getBlock(newLoc).getType() == 0)
				{
					Node n = new Node(newLoc);
					n.group = getNewGroup();
					setNode(newLoc, n);
					linkUpNodes(newLoc, true);
				}
				else if(getNode(newLoc) != null && getBlock(newLoc).getType() == 0)
				{
					linkUpNodes(newLoc, true);
				}
				
				if(getNode(loc) != null)
				{
					removeNode(loc, true);
				}
				
			}
		}
		else if(getBlock(loc).getType() == 0)
		{
			if(G.withinLim(newLoc, mapSize))
			{
				if(getNode(newLoc) != null)
				{
					removeNode(newLoc, true);
				}
			}
		}
		
		if(checkNearby)
		{
			newLoc = loc + G.transList[5];
			if(G.withinLim(newLoc, mapSize)) 
			{
				checkTerrain(newLoc, false);
			}
			newLoc = newLoc + G.transList[5];
			
			if(G.withinLim(newLoc, mapSize)) 
			{
				checkTerrain(newLoc, false);
			}
		}

	}

	private void setNode(Point loc, Node node)
	{
		pathMap[G.To1D(loc, mapSize)] = node;
	}

	private void linkUpNodes(Point loc, bool checkNearby)
	{
		Point newLoc;
		Point newLoc2;
		
		Point newTrans;
		Point newTrans2;
		
		Node n = getNode(loc);

        bool noPath = false;
        bool assignedGroup = false;
		
		n.clearAllLinkSizes();
		
		
		
		for(int i = 0; i < 18; i++) 
		{
			newLoc = loc + G.transList[i];
			if(!G.withinLim(newLoc, mapSize))
				continue;
			
			if(getBlock(newLoc + G.transList[0]).getType() > 0) 
			{
                noPath = true;
			}
			
			if(i >= 14)
			{
				newTrans = new Point(G.transList[i].x, 0, 0);
				newTrans2 = new Point(0, 0, G.transList[i].z);
				
				if(!IsSolid(loc + newTrans + G.transList[5]) && !IsSolid(loc + newTrans2 + G.transList[5])) 
				{
                    noPath = true;
				}
				
				if(IsSolid(loc + newTrans) || IsSolid(loc + newTrans2) || IsSolid(loc + newTrans + G.transList[0]) || IsSolid(loc + newTrans2 + G.transList[0])) 
				{
                    noPath = true;
				}
				
			}
			
			if(i == 8 || i == 9 || i == 12 || i == 13) 
			{
				newTrans = new Point(G.transList[i].x, 0, 0);
				newTrans2 = new Point(0, G.transList[i].y, 0);
				
				if(IsSolid(loc + newTrans2))
				{
                    noPath = true;
				}
			}
			
			if(i == 6 || i == 7 || i == 10 || i == 11) 
			{
				newTrans = new Point(G.transList[i].x, 0, 0);
				newTrans2 = new Point(0, 0, G.transList[i].z);
				
				if(IsSolid(loc + newTrans2) || IsSolid(loc + newTrans)) 
				{
                    noPath = true;
				}
			}

			if(getNode(newLoc) != null)
			{
				n.linkedNodes[i] = getNode(newLoc);
				if(n.group >= 0)
				{
					n.showDebugCube(n.group.ToString());
				}
				if(checkNearby) 
				{
					linkUpNodes(newLoc, false);
				}
			}
			else
			{
				if(n.linkedNodes[i] != null) 
				{
					n.linkedNodes[i] = null;
				}
			}


            if (noPath)
            {
                n.setLinkSize(i, -1, true);
				
            }
            else
            {
                n.setLinkSize(i, 1, true);
            }

            noPath = false;
		}


        

        checkGroup(n);
		
		
	}

    int stackCheck = 0;

    private void checkGroup(Node u)
    {
        Queue<Node> nodes = new Queue<Node>();
        Node n;
        Node w;
        nodes.Enqueue(u);
		int c = 0;

        while (nodes.Count != 0)
        {
            n = nodes.Dequeue();
            //stackCheck++;
            for(int i = 0; i < n.linkedNodes.Length; i++)
            {
                w = n.linkedNodes[i];
                if (w != null && n.linkSizes[i] > 0 && n.group != w.group)
                {
					//G.debugString = G.debugString + c + ": change group " + w.group + " to " + u.group + " | ";
                    w.group = u.group;
					w.showDebugCube(w.group.ToString());
                    nodes.Enqueue(w);
					//c++;
                }
            }
        }
    }
	
	private void setupMap()
	{
		pathMap = new Node[mapSize.x * mapSize.y * mapSize.z];
		
		for(int i = 0; i < mapSize.x; i++) 
		{
			for(int i2 = 0; i2 < mapSize.y; i2++) 
			{
				for(int i3 = 0; i3 < mapSize.z; i3++)
				{
					checkTerrain(new Point(i, i2, i3), false);
				}
			}
		}
	}

    private int biggestGroup = 0;

    private int getNewGroup()
    {
        //Debug.Log("created new group " + biggestGroup);
        return biggestGroup++;
    }

	private bool IsSolid(Point loc)
	{
		return (getBlock(loc).hasProperty("Solid"));
	}

	public Node getNode(Point loc)
	{
		return pathMap[G.To1D(loc, mapSize)];
	}
	
	private void removeNode(Point loc, bool clearNearby)
	{
		Point newLoc;
		
		List<Node> arr = new List<Node>(18);
		getNode(loc).hideDebugCube();
		if(getNode(loc) != null)
		{
			for(int i = 0; i < 18; i++) 
			{
				if(getNode(loc).linkedNodes[i] != null) 
				{
					arr.Add(getNode(loc).linkedNodes[i]);
					arr[arr.Count -1].group = getNewGroup();
					getNode(loc).linkedNodes[i] = null;
					
				}
			}
			setNode(loc, null);
			
		}
		
		for(int i2 = 0; i2 < arr.Count; i2++) 
		{
			linkUpNodes(arr[i2].getLoc(), false);
			
		}
	}
}
