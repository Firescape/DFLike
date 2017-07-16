using System;
using System.Collections.Generic;
using UnityEngine;

class UIManager:MonoBehaviour
{
    String s;

    void OnGUI()
    {
        s = GUI.TextArea(new Rect(20, 20, 100, 20), GC.GetTotalMemory(false).ToString());
    }
}

