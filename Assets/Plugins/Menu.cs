using UnityEngine;
using System.IO;

public class Menu:MonoBehaviour
{
	GUIStyle customGuiStyle;
	
	void  Start()
	{
		Resources.UnloadUnusedAssets();
	}
	
	private int selectionGridInt = 0;
	
	private static string[] files = new string[]{"map" + 0 + ".dat",
							"map" + 1 + ".dat",
							"map" + 2 + ".dat",
							"map" + 3 + ".dat"};
							
	private static string[] selectionStrings = new string[]{getMapInfo(0), getMapInfo(1), getMapInfo(2), getMapInfo(3)};
	
	
	static string getMapInfo(int index)
	{
		FileInfo file = new FileInfo(files[index]);
		
		float size;
		
		if(file.Exists)
		{
			size = (float)file.Length / 1024f / 1024f;
			return "Map " + index + " - " + (size) + "mb";
		}
		else
		{
			return "empty";
		}
	}
	
	void  OnGUI ()
	{
		// Make a background box
		//GUI.Box ( new Rect(10,10,100,300), "Loader Menu");
	
		selectionGridInt = GUI.SelectionGrid(new Rect(20, 140, 100, 100), selectionGridInt, Menu.selectionStrings, 1);
		
		G.savePath = files[selectionGridInt];
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if (GUI.Button ( new Rect(20,40,200,40), "New Game")) 
		{
			G.loadLevel = false;
			Application.LoadLevel (1);
		}
		
		if (GUI.Button ( new Rect(20,90,200,40), "Load Game")) 
		{
			G.loadLevel = true;
			Application.LoadLevel (1);
		}
	}
}