using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StarGeneration
{
    //generate a list of points for the stars
    public static List<Vector3> GeneratePoints(float radius, Vector3 sampleAreaSize, int attemptBefReject, float dispHeight, float heightVarience, int maxCount)
    {
        // each cell has a diagonal equal to the radius, so we have to calculate the side lengths
        float cellSize = radius / Mathf.Sqrt(2);

        // the grid will just be however many cells fit withing the size of the sample area
        int[,] grid = new int[Mathf.CeilToInt(sampleAreaSize.x / cellSize), Mathf.CeilToInt(sampleAreaSize.z / cellSize)];

        // list with all the generated points to track the index of the point in each cell
        // since 0 is used to denote an empty cell, the index of points starts from 1
        List<Vector3> points = new List<Vector3>();

        // list of spawn points where new points can be generated around
        List<Vector3> spawnPoints = new List<Vector3>();

        // adding a starting point to start generating around (added in the center for simplicity)
        Vector3 startPoint = new Vector3(sampleAreaSize.x / 2, dispHeight, sampleAreaSize.z / 2);
        spawnPoints.Add(startPoint);

        // looping through spawnpoints to try and generate new points
        while(spawnPoints.Count > 0 && points.Count < maxCount)
        {
            // attempting to generate a new point based on a random spawnpoint in the list
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnCenter = spawnPoints[spawnIndex];

            // after attemptBefReject tries to spawn a new point, if they all falied then the point will be removed as a spawnPoint
            bool candidateAccepted = false;
            for (int i = 0; i < attemptBefReject; i++)
            {
                // random point at any angle and a distance between the radius and double the radius
                float angle = Random.value * Mathf.PI * 2;
                float heightChange = dispHeight + Random.Range(-heightVarience, heightVarience);  // needs to be limited in some wya maybe??
                Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                Vector3 candidate = spawnCenter + dir * Random.Range(radius, 2 * radius);
                candidate.y = heightChange;
                
              
                // checking if the new point is within another point's radius
                if (isValid(candidate, sampleAreaSize, cellSize, points, grid, radius))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    // saving the grid position of the point
                    grid[(int)(candidate.x / cellSize), (int)(candidate.z / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            // if none of the new points were valid, then the point is removed from the spawnPoints list
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        
        return points;
    }

    static bool isValid(Vector3 candidate, Vector3 sampleAreaSize, float cellSize, List<Vector3> points, int[,] grid, float radius)
    {
        // checking tha the candidate point is within the sample area
        if (candidate.x >=0 && candidate.x < sampleAreaSize.x && candidate.z >=0 && candidate.z < sampleAreaSize.z)
        {
            //checking the cell of the candidate point
            int cellX = (int)(candidate.x / cellSize);
            int cellZ = (int)(candidate.z / cellSize);

            // checking any points in a 5x5 grid around the cell of the candidate
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(1) - 1);

            // classic double for loop going through the cells in  the grid
            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int z = searchStartZ; z <= searchEndZ; z++)
                {
                    // index of the point in the current grid cell (-1 because we count indexes from 1)
                    int pointIndex = grid[x, z] - 1;

                    // checking that the cell is not empty
                    if (pointIndex != -1)
                    {
                        // finding distance between the point in the cell and the candidate point (apparently squared magnitude is cheaper)
                        float sqrDist = (candidate - points[pointIndex]).sqrMagnitude;

                        // returning false if the candidate is within the point's radius
                        if (sqrDist < radius*radius)
                        {
                            return false;
                        }
                    }
                }
            }
            // if the candidate was not within the radius of anu grid points, return true
            return true;
        }
        return false;
    }
}
