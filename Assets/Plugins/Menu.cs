using UnityEngine;
using System.IO;

public class Menu : MonoBehaviour
{
    private static string[] files = new string[]
    {
        "map" + 0 + ".dat",
        "map" + 1 + ".dat",
        "map" + 2 + ".dat",
        "map" + 3 + ".dat"
    };

    private static string[] selectionStrings = new string[]
        {getMapInfo(0), getMapInfo(1), getMapInfo(2), getMapInfo(3)};

    private GUIStyle customGuiStyle;

    private int selectionGridInt = 0;

    private void Start()
    {
        Resources.UnloadUnusedAssets();
    }


    private static string getMapInfo(int index)
    {
        var file = new FileInfo(files[index]);

        float size;

        if (file.Exists)
        {
            size = (float) file.Length / 1024f / 1024f;
            return "Map " + index + " - " + size + "mb";
        }
        else
        {
            return "empty";
        }
    }

    private void OnGUI()
    {
        // Make a background box
        //GUI.Box ( new Rect(10,10,100,300), "Loader Menu");

        selectionGridInt = GUI.SelectionGrid(new Rect(20, 140, 100, 100), selectionGridInt, selectionStrings, 1);

        G.savePath = files[selectionGridInt];
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 40, 200, 40), "New Game"))
        {
            G.loadLevel = false;
            Application.LoadLevel(1);
        }

        if (GUI.Button(new Rect(20, 90, 200, 40), "Load Game"))
        {
            G.loadLevel = true;
            Application.LoadLevel(1);
        }
    }
}