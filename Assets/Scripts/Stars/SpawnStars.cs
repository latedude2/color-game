using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStars : MonoBehaviour

{
    public float radius = 1;
    public float displayHeight = 20;
    public Vector3 areaSize = new Vector3(1,1,1);
    public int rejectionSamples = 10;
    public float dispRadius = 1;
    public float heightVariance = 1f;
    public int maxStarCount = 1500;

    List<Vector3> points;

    // method called when values are changed in inspector
    void OnValidate()
    {
        // calling the function from the other script to generate a list of points
        points = StarGeneration.GeneratePoints(radius, areaSize, rejectionSamples, displayHeight, heightVariance, maxStarCount);

    }

    // drawing spheres at the points
    // and drawing the generation area
    void OnDrawGizmos()
    {

        if (points != null)
        {
            
            foreach (Vector3 point in points)
            { 
                Gizmos.DrawSphere(point, dispRadius);
            }
        }
    }

    void OnStart()
    {
        if (points != null)
        {

            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(point, dispRadius);
            }
        }
    }
}
