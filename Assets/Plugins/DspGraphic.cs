using UnityEngine;
using System.Collections;

public class DspGraphic: object
{
    public GameObject gameObject;

    public DspGraphic(int[,,] map, Point loc)
    {
        //gameObject = GameObject.Instantiate(Resources.Load("Mask")) as GameObject;
        //Texture2D graphic = new Texture2D(map.GetLength(0) * 32, map.GetLength(2) * 32);
        //Texture2D terrainSheet = GameObject.Instantiate(Resources.Load("TerrainSheet")) as Texture2D;
        //gameObject.GetComponent<MeshRenderer>().material.mainTexture = graphic;
        //Color[] pixels = terrainSheet.GetPixels(128, 32, 32, 32, 0);
        //gameObject.transform.localScale = new Vector3(0.16666666f, 1, 0.16666666f);
        //gameObject.transform.position = new Vector3((float)loc.x, (float)loc.y + 0.01f, (float)loc.z);
        //gameObject.transform.position += new Vector3(1, 0, 1);
        //for (int i = 0; i < map.GetLength(0); i++)
        //{
        //    for (int i2 = 0; i2 < map.GetLength(1); i2++)
        //    {
        //        for (int i3 = 0; i3 < map.GetLength(2); i3++)
        //        {
        //            if (map[i, i2, i3] > 0)
        //            {
        //                graphic.SetPixels(i * 32, i3 * 32, 32, 32, pixels);
        //            }
        //        }
        //    }
        //}

        //graphic.Apply();

    }
}
