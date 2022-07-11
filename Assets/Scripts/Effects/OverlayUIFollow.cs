using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUIFollow : MonoBehaviour
{
    public Vector3 target;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = cam.WorldToScreenPoint(target);

        if(transform.position != position)
        {
            transform.position = position;
        }
    }
}

