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

    List<Vector3> points;

    // method called when values are changed in inspector
    void OnValidate()
    {
        // calling the function from the other script to generate a list of points
        points = StarGeneration.GeneratePoints(radius, areaSize, rejectionSamples, displayHeight);

    }

    // drawing spheres at the points
    // and drawing the generation area
    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(areaSize / 2, areaSize);

        if (points != null)
        {
            /*Gizmos.DrawSphere(points[0], dispRadius);
            Gizmos.DrawSphere(points[1], dispRadius);
            Gizmos.DrawSphere(points[2], dispRadius);*/
            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(point, dispRadius);
            }
        }
    }
}
