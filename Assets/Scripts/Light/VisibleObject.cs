using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] lightsInScene;


    void Start()
    {
        lightsInScene = GameObject.FindGameObjectsWithTag("Light");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
