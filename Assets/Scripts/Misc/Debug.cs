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
            if(Input.GetKeyDown(KeyCode.F1))
            {
                debugMode = !debugMode;
            }
        }
    }
}
