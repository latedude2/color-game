using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    private Vector3[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;

    void Start()
    {
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        shinePoints = FindShinePoints();
    }

    void FixedUpdate()
    {
        //TODO: Create list of points of the cube where we check if the light is shining. 

        //TODO: Raycast from the lights to the points and check if any of them reach the points without being occluded. 

        shinePoints = FindShinePoints();
    }

    Vector3[] FindShinePoints()
    {
        //Create a list of points for the visible object that we will be checking for shine.
        List<Vector3> shinepoints = new List<Vector3>();

        BoxCollider b = GetComponent<BoxCollider>();

        // Add points for each corner of the box collider to the list
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f));

        return shinepoints.ToArray();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        try
        {
            // Draw spheres for ShinePoints for debugging
            foreach (Vector3 point in shinePoints)
                Gizmos.DrawSphere(point, .05f);
        }
        catch
        {
            new UnityException();
        }
    }

    GameObject[] FindPointingLights()
    {
        //TODO: Create list of lights that are on and pointing towards the points, so we don't waste computations. 
        //Based on the normal vectors, only three sides of a box should be lit by the same light at once. 
        return lightManager.GetLights();
    }
}
