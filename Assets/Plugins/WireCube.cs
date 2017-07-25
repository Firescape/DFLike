using UnityEngine;
using System;


public class WireCube : MonoBehaviour
{
    public Vector3 center;
    public bool draw = false;
    public Vector3 size;
    private bool start = false;
    private string text;
    private GameObject textObject;


    private void Start()
    {
        transform.position = center;

        //text = Instantiate(TextGizmo, center, Quaternion.identity);
        textObject = new GameObject("bla");
        textObject.AddComponent<GUIText>();
        textObject.GetComponent<GUIText>().text = text;
        start = true;
        //Vector2 scorePosition = Camera.main.WorldToViewportPoint(center) + Vector2(-.025, .01);

        //GUIText pointsClone = Instantiate(TextGizmo, scorePosition, Quaternion.identity);
    }

    public void setLabel(string text)
    {
        this.text = text;
        if (start == true)
            textObject.GetComponent<GUIText>().text = text;

        //textObject.guiText.text = text;
    }

    private void OnDrawGizmos()
    {
        if (draw)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(center, size);
            //Vector3 pos = new Vector3(0, 0.75, 0);

            //Vector2 scorePosition = Camera.main.WorldToViewportPoint(center) + Vector2(-.025, .01);


            //TextGizmo.Instance.DrawText(Camera.main, center + new Vector3(0f, 0.75f, 0f), "hey");
            //Gizmos.
        }
        textObject.transform.position = Camera.main.WorldToViewportPoint(center + new Vector3(0f, 0.10f, 0f));
    }

    private void OnDestroy()
    {
        Destroy(textObject);
    }
}