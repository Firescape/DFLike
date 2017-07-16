using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using UnityEngine;

public class G
{
	public static bool  loadLevel = false;
	public static string savePath = "default.dat";
	public static Point mapSize = new Point(64, 32, 64);
	public static Point chunkSize = new Point(16, 32, 16);
	public static Point bucketSize = new Point(16, 32, 16);
	public static string debugString = "";
	
	public static Point[] transList =
	new Point[]{
	new Point(0, 1, 0), //0
	new Point(0, 0, 1), //1
	new Point(1, 0, 0), //2
	new Point(-1, 0, 0), //3
	new Point(0, 0, -1), //4
	new Point(0, -1, 0), //5
	
	new Point(1, -1, 0), //6
	new Point(-1, -1, 0), //7
	new Point(1, 1, 0), //8
	new Point(-1, 1, 0), //9
	
	new Point(0, -1, 1), //10
	new Point(0, -1, -1), //11
	new Point(0, 1, 1), //12
	new Point(0, 1, -1), //13
	
	new Point(1, 0, 1), //14
	new Point(1, 0, -1), //15
	new Point(-1, 0, 1), //16
	new Point(-1, 0, -1) //17
	};
	
	public static Vector3[][] VertDef =
	new Vector3[][]{
	new Vector3[]{new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(1,0,1)},
	new Vector3[]{new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(0,-1,1), new Vector3(1,-1,1)},
	new Vector3[]{new Vector3(1,0,0), new Vector3(1,0,1), new Vector3(1,-1,0), new Vector3(1,-1,1)},
	new Vector3[]{new Vector3(0,0,0), new Vector3(0,0,1), new Vector3(0,-1,0), new Vector3(0,-1,1)},
	new Vector3[]{new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,-1,0), new Vector3(1,-1,0)},
	new Vector3[]{new Vector3(0,-1,0), new Vector3(1,-1,0), new Vector3(0,-1,1), new Vector3(1,-1,1)}};
	
	public static int To1D (Point loc, Point dimensions)
	{
		return (dimensions.x * dimensions.z * (loc.y - 0) + loc.x + dimensions.x * (loc.z - 0));
	}
	
	
	public static Vector3[] normalDef =
	new Vector3[]{
	new Vector3(0, 1, 0),
	new Vector3(0, 0, 1),
	new Vector3(1, 0, 0),
	new Vector3(-1, 0, 0),
	new Vector3(0, 0, -1),
	new Vector3(0, -1, 0)};
	
	
//	public static function bytesToHashtableList( byte[] bytes):List.<Hashtable>
//	{
//		byte[] buffer = new byte[4096];
//		
//		MemoryStream byteStream = new MemoryStream(bytes, 4, bytes.Length - 4);
//		
//		byteStream.Read(buffer, 0, 4); //number of hashtables
//		List. hashtableList<Hashtable> = new List.<Hashtable>(BitConverter.ToInt32(buffer, 0));
//		
//		int pairCount;
//		string key;
//		byte typeCode;
//		FIXME_VAR_TYPE value;
//		int length = 0;
//		
//		UnicodeEncoding encoder = new UnicodeEncoding(false, false);
//
//		for(int i = 0; i < hashtableList.Capacity; i++)
//		{	
//			hashtableList.Add(new Hashtable());
//			
//			byteStream.Read(buffer, 0, 4);
//			pairCount = BitConverter.ToInt32(buffer, 0);
//			
//			for(int i2 = 0; i2 < pairCount; i2++)
//			{
//				byteStream.Read(buffer, 0, 4); //length of the key string
//				length = BitConverter.ToInt32(buffer, 0);
//				//Debug.Log(length);
//				byteStream.Read(buffer, 0, length);
//				key = encoder.GetString(buffer, 0, length);
//				//Debug.Log("Key is " + key);
//				byteStream.Read(buffer, 0, 1);
//				typeCode = buffer[0];
//				//Debug.Log("Type code is " + typeCode);
//				
//				switch(typeCode)
//				{
//					case TypeCode.Single:
//						byteStream.Read(buffer, 0, 4);
//						value = BitConverter.ToSingle(buffer, 0);
//						break;
//					case TypeCode.boolean:
//						byteStream.Read(buffer, 0, 1);
//						value = BitConverter.ToBoolean(buffer, 0);
//						break;
//					case TypeCode.String:
//						byteStream.Read(buffer, 0, 4);
//						length = BitConverter.ToInt32(buffer, 0);
//						byteStream.Read(buffer, 0, length);
//						value = encoder.GetString(buffer, 0, length);
//						break;
//					case TypeCode.Int32:
//						byteStream.Read(buffer, 0, 4);
//						value = BitConverter.ToInt32(buffer, 0);
//						break;
//					case TypeCode.Point:
//						byteStream.Read(buffer, 0, 12);
//						value = new Point(buffer);
//						break;
//					case TypeCode.uid:
//						byteStream.Read(buffer, 0, 4);
//						value = BitConverter.ToInt32(buffer, 0);
//						break;
//					default:
//						throw("Unhandled type code " + typeCode + " cannot be converted by bytes to object converter");
//				}
//				//Debug.Log(value);
//				
//				hashtableList[i].Add(key, value);
//			}
//		}
//	}
//
//	//1:length of entire byte array as int - how many elements are in the table as int - 
//
//	//2:length of the key as int- the key as string - 
//
//	//3:length of type of value as int - type of value as an enumarated byte - 
//	//4:length of value if it's a string as int - value as bytes; goto 2
//	
//	public static function hashtableListToBytes(tableList:List.<Hashtable>):List.<byte>
//	{
//		List. bytes<byte> = new List.<byte>();
//		
//		List. bytes2<byte> = new List.<byte>();
//		List. bytes3<byte> = new List.<byte>();
//		
//		bytes2.AddRange(BitConverter.GetBytes(tableList.Count));
//		
//		foreach(Hashtable table in tableList)
//		{
//			bytes2.AddRange(BitConverter.GetBytes(table.Count));
//			foreach(string key in table.Keys)
//			{
//				bytes3.Clear();
//				bytes3.AddRange(System.Text.Encoding.Unicode.GetBytes(key)); // the key itself
//				
//				bytes2.AddRange(BitConverter.GetBytes(bytes3.Count)); // the length of the string
//				bytes2.AddRange(bytes3); // the key is now inserted into the second level temparray
//				
//				bytes2.AddRange(convertToBytes(table[key])); //the convert to bytes handles the header and type of the value
//			}
//		}
//		//Debug.Log(bytes2.Count);
//		//Debug.Log(BitConverter.ToString(BitConverter.GetBytes(260)));
//		bytes.AddRange(BitConverter.GetBytes(bytes2.Count));
//		
//		
//		bytes.AddRange(bytes2);
//		
//		return bytes;
//	}
//	
//	public enum TypeCode
//	{
//		Single = 1,
//		bool =  2,
//		string = 3,
//		Point = 4,
//		uid = 5,
//		Int32 = 6
//	}
//	
//	public static function convertToBytes(object):List.<byte>
//	{
//		List. bytes<byte> = new List.<byte>();
//		byte[] bytes2;
//		
//		Type type = typeof(object);
//		byte typeCode;
//		
//		switch(type)
//		{
//			case Single:
//				typeCode = TypeCode.Single;
//				bytes2 = BitConverter.GetBytes(object);
//				break;
//			case boolean:
//				typeCode = TypeCode.boolean;
//				bytes2 = BitConverter.GetBytes(object);
//				break;
//			case String:
//				typeCode = TypeCode.String;
//				bytes2 = System.Text.Encoding.Unicode.GetBytes(object);
//				break;
//			case Int32:
//				typeCode = TypeCode.Int32;
//				bytes2 = BitConverter.GetBytes(object);
//				break;
//			case Point:
//				typeCode = TypeCode.Point;
//				bytes2 = object.GetBytes();
//				break;
//			default:
//				if(type.GetField("uid"))
//				{
//					typeCode = TypeCode.uid;
//					bytes2 = BitConverter.GetBytes(object.uid);
//				}
//				else
//				{
//					throw("Unhandled type " + type + " cannot be converted by the object to bytes converter.");
//				}
//				break;
//				
//		}
//		bytes.Add(typeCode);
//		if(type == string)
//		{
//			bytes.AddRange(BitConverter.GetBytes(bytes2.Length));
//		}
//		bytes.AddRange(bytes2);
//		
//		return bytes;
//	}
	
	
	
	
	public static int invTransIndex (int index)
	{
		for(int i = 0; i < 18; i++)
		{
			if(transList[index] * -1 == transList[i])
			{
				//Debug.Log(new Vector2(index, i));
				return i;
			}
		}
		throw(new System.Exception("Unhandled index provided"));
	}
	
	public static int transIndex (Point dir)
	{
		for(int i = 0; i < 18; i++)
		{
			if(transList[i] == dir)
			{
				return i;
			}
		}
		throw(new System.Exception("Unhandled direction provided"));
	}
	
	
	
	public static Point vectorClamp(Point loc,  Point lim)
	{
		Point newLoc = loc;
		
		newLoc.x = Mathf.Clamp(newLoc.x, 0, lim.x - 1);
		newLoc.y = Mathf.Clamp(newLoc.y, 0, lim.y - 1);
		newLoc.z = Mathf.Clamp(newLoc.z, 0, lim.z - 1);
		
		return newLoc;
	}
	
	public static bool withinLim ( Point loc ,   Point trans ,   Point lim  ){
		 Point transLoc = new Point(loc.x + trans.x, loc.y + trans.y, loc.z + trans.z);
		
		if(transLoc.x >= 0 && transLoc.y >= 0 && transLoc.z >= 0
		&& transLoc.x < lim.x && transLoc.y < lim.y && transLoc.z < lim.z)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public static bool withinLim ( Point loc ,   Point lim  ){
		 if(loc.x >= 0 && loc.y >= 0 && loc.z >= 0
		&& loc.x < lim.x && loc.y < lim.y && loc.z < lim.z)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
