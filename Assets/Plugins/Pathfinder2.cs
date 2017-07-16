using UnityEngine;
using System.Collections;


using System.Collections.Generic;

public class Pathfinder2
{
	public static List<Node> findPath (Node firstNode, Node destinationNode)
	{
		List<int> openList = new List<int> (16384);
		List<Node> closedNodes = new List<Node> (16384);
		List<Node> nodeList = new List<Node> (16384);
		
		List<Node> closedNodesIndex = new List<Node> (16384);
		List<Node> openNodesIndex = new List<Node> (16384);
		
		nodeList.Add (new Node (new Point (0, 0, 0)));
		//dummy node
		openList.Add (0);
		
		int squaresChecked = 0;
		int numberOfOpenListItems = 0;
		
		Node currentNode = firstNode;
		
		Node testNode;
		
		int l;
		int i;
		int i2;
		
		Node[] connectedNodes;
		float travelCost = 1.0f;
		
		float g;
		float h;
		float f;
		
		int u;
		int v;
		
		int temp;
		
		//float searchTime;
		
		currentNode.g = 0;
		currentNode.h = manhattanHeuristic (currentNode, destinationNode, travelCost);
		currentNode.f = currentNode.g + currentNode.h;
		
		while (!(currentNode.x == destinationNode.x && currentNode.y == destinationNode.y && currentNode.z == destinationNode.z))
		{
			connectedNodes = currentNode.getConnected ();
			l = connectedNodes.Length;
			for (i = 0; i < l; i++)
			{
				if (connectedNodes [i] != null)
				{
					testNode = connectedNodes [i];
					
					if (testNode == currentNode || testNode.traversable == false)
						continue;
					
					
					if (Mathf.Abs (testNode.x - currentNode.x) + Mathf.Abs (testNode.y - currentNode.y) + Mathf.Abs (testNode.z - currentNode.z) >= 2)
					{
						travelCost = 1.4f;
					}
					else
					{
						travelCost = 1.0f;
					}
					
					g = currentNode.g + travelCost;
					h = euclidianHeuristic (testNode, destinationNode, travelCost);
					f = g + h;
					//FIXME_VAR_TYPE start= Time.realtimeSinceStartup;
					if (testNode.open || testNode.closed)
					{
						//
						if (testNode.f > f)
						{
							testNode.f = f;
							testNode.g = g;
							testNode.h = h;
							testNode.parentNode = currentNode;
						}
					}
					else
					{
						//Debug.Log("open++");
						testNode.f = f;
						testNode.g = g;
						testNode.h = h;
						testNode.parentNode = currentNode;
						
						openNodesIndex.Add (testNode);
						testNode.open = true;
						nodeList.Add (testNode);
						squaresChecked++;
						numberOfOpenListItems++;
						
						openList.Add (squaresChecked);
						
						//int i2 = numberOfOpenListItems;
						i2 = numberOfOpenListItems;
						//FIXME_VAR_TYPE start= Time.realtimeSinceStartup;
						while (i2 != 1)
						{
							if (Fcost (openList [i2], nodeList) <= Fcost (openList [(int)Mathf.Floor ((int)i2 / 2)], nodeList))
							{
								temp = openList [(int)Mathf.Floor ((int)i2 / 2)];
								openList [(int)Mathf.Floor ((int)i2 / 2)] = openList [i2];
								openList [i2] = temp;
								i2 = (int)Mathf.Floor ((int)i2 / 2);
								
							}
							else
							{
								break;
							}
						}
					}
				}
			}
			
			currentNode.closed = true;
			closedNodesIndex.Add (currentNode);
			if (openList.Count == 1)
			{
				for (i = 0; i < openNodesIndex.Count; i++)
				{
					openNodesIndex [i].open = false;
				}
				
				for (i = 0; i < closedNodesIndex.Count; i++)
				{
					closedNodesIndex [i].closed = false;
				}
				//Debug.Log(squaresChecked);
				return null;
				
			}
			
			currentNode = getNode (openList [1], nodeList);
			
			openList [1] = openList [numberOfOpenListItems];
			openList.RemoveAt (openList.Count - 1);
			numberOfOpenListItems--;
			
			v = 1;
			
			while (true)
			{
				u = v;
				
				if (2 * u + 1 <= numberOfOpenListItems)
				{
					if (Fcost (openList [u], nodeList) >= Fcost (openList [2 * u], nodeList))
					{
						v = 2 * u;
					}
					
					if (Fcost (openList [v], nodeList) >= Fcost (openList [2 * u + 1], nodeList))
					{
						v = 2 * u + 1;
					}
				}
				else if (2 * u <= numberOfOpenListItems)
				{
					if (Fcost (openList [u], nodeList) >= Fcost (openList [2 * u], nodeList))
					{
						v = 2 * u;
					}
				}
				
				if (u != v)
				{
					temp = openList [u];
					openList [u] = openList [v];
					openList [v] = temp;
				}
				else
				{
					break;
				}
				
			}
			
			
			//printArrayList(openList, nodeList);
			//Debug.Log("-----------------");
		}
		
		destinationNode = currentNode;
		//Debug.Log("open / closed list search time is " + searchTime);
		//Debug.Log("nodes checked: " + squaresChecked);
		
		for (i = 0; i < openNodesIndex.Count; i++)
		{
			openNodesIndex [i].open = false;
		}
		
		for (i = 0; i < closedNodesIndex.Count; i++)
		{
			closedNodesIndex [i].closed = false;
		}
		
		
		return Pathfinder2.buildPath (destinationNode, firstNode);
		
	}

	//static function printArrayList(arr, nodeList:List.<Node>):void
	//{
	//if(arr)
	//{
	//for (int i = 1; i < arr.Count; i++)
	//{
	//Debug.Log(Fcost(arr[i], nodeList));
	//}
	//}
	//else
	//{
	//print("the array to be printed is empty");
	//}
	//}



	public static float Fcost(int e, List<Node> nodeList)
	{
		//Debug.Log("the length of the array is " + nodeList.Count);
		//Debug.Log("element requested " + e);
		return nodeList [e].f;
	}

	public static Node getNode(int e, List<Node> nodeList)
	{
		return nodeList [e];
	}

	public static List<Node> buildPath(Node destinationNode, Node startNode)
	{
		List<Node> path = new List<Node>();
		Node node = destinationNode;
		path.Add(node);
		//Debug.Log("lol");
		while (node != startNode)
		{
			//Debug.Log(new Vector2(node.x, node.y));
			//Debug.Log(node.parentNode);
			node = node.parentNode;
			//Debug.Log(new Vector2(node.x, node.y));
			//print(new Vector2(node.x, node.y));
			path.Insert(0, node);
		}
		
		return path;
	}

	public static bool isOpen(Node node, ArrayList openList)
	{
		int l = openList.Count;
		for (int i = 0; i < l; ++i)
		{
			if (openList [i] == node)
			{
				//Debug.Log("is on open list");
				return true;
			}
		}
		
		return false;
	}

	public static float diagonalHeuristic(Node node, Node destinationNode, float cost)
	{
		float diagonalCost = cost;
		
		float dx = Mathf.Abs (node.x - destinationNode.x);
		float dy = Mathf.Abs (node.y - destinationNode.y);
		
		float diag = Mathf.Min (dx, dy);
		float straight = dx + dy;
		
		return diagonalCost * diag + cost * (straight - 2 * diag);
	}

	public static float manhattanHeuristic(Node node, Node destinationNode, float cost)
	{
		return (Mathf.Abs (node.x - destinationNode.x) * cost + Mathf.Abs (node.y - destinationNode.y) * cost + Mathf.Abs (node.z - destinationNode.z) * cost);
	}

	public static float euclidianHeuristic(Node node, Node destinationNode, float cost)
	{
		return new Vector3 (node.x - destinationNode.x, node.y - destinationNode.y, node.z - destinationNode.z).magnitude;
	}
}
