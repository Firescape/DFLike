using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameInitializer:MonoBehaviour
{
	private World world;
    //public Font font;
	public Font font;
	
	void Start()
	{
		World.world = new World();
		//font = (Font)Resources.GetBuiltinResource(typeof(Font), "cour.ttf");
	}
	
	void FixedUpdate()
	{
        World.world.Update();
        //Debug.Log(GC.GetTotalMemory(false));
	}
	
	void Update()
	{
		World.world.CheckInput();
	}

    List<String> output = null;

    void OnGUI()
    {
        // = world.entityManager.getIndexDebug();
        //GUI.Label(new Rect(10, 10, 100, 20), "Hello World!");
        //if (GUI.Button(new Rect(10, 10, 135, 20), "Show/Update IndexType"))
        //{
            //output = World.world.entityManager.getIndexDebug();
			//TerrainChunk tC = new TerrainChunk();
			//output[0] = tC.txt;
            //for (int i = 0; i < output.Count; i++)
            //{
                //Debug.Log(output[i]);
            //}
        //}//
		
        if(output != null)
        {
            for (int i = 0; i < output.Count; i++)
            {
                GUI.skin.font = font;
                GUI.Label(new Rect(20 + 123 * i, 30, 125, 800), output[i]);

            }
        }

    }


}

