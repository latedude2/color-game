using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorGame
{
    public class Debug : MonoBehaviour
    {
        public static bool debugMode = false;

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F1))
            {
                debugMode = !debugMode;
            }
        }
        void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = Color.white;
            guiStyle.fontSize = 20;
            if (debugMode)
                GUI.Label(new Rect(50, 50, 100, 20), "Debug Mode, Press F2 for NoClip",guiStyle);
        }
    }
}
