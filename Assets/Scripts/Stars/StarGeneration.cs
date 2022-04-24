using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGeneration : MonoBehaviour
{
    //generate a list of points for the stars
    public static List<Vector2> GeneratePoint(float radius, Vector2 sampleAreaSize, int attemptBefReject = 30)
    {
        // each cell has a diagonal equal to the radius, so we have to calculate the side lengths
        float cellSize = radius / Mathf.Sqrt(2);

        // the grid will just be however many cells fit withing the size of the sample area
        int[,] grid = new int[Mathf.CeilToInt(sampleAreaSize.x / cellSize), Mathf.CeilToInt(sampleAreaSize.y / cellSize)];

        // list with all the generated points to track the index of the point in each cell
        // since 0 is used to denote an empty cell, the index of points starts from 1
        List<Vector2> points = new List<Vector2>();

        // list of spawn points where new points can be generated around
        List<Vector2> spawnPoints = new List<Vector2>();

        // adding a starting point to start generating around (added in the center for simplicity)
        spawnPoints.Add(sampleAreaSize / 2);

        // looping through spawnpoints to try and generate new points
        while(spawnPoints.Count > 0)
        {
            // attempting to generate a new point based on a random spawnpoint in the list
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];

            // after attemptBefReject tries to spawn a new point, if they all falied then the point will be removed as a spawnPoint
            bool candidateAccepted = false;
            for (int i = 0; i < attemptBefReject; i++)
            {
                // random point at any angle and a distance between the radius and double the radius
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + dir * Random.Range(radius, 2 * radius);

                // checking if the new point is within another point's radius
                if (isValid(candidate, sampleAreaSize, cellSize, points, grid, radius))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    // saving the grid position of the point
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
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

    static bool isValid(Vector2 candidate, Vector2 sampleAreaSize, float cellSize, List<Vector2> points, int[,] grid, float radius)
    {
        // checking tha the candidate point is within the sample area
        if (candidate.x >=0 && candidate.x < sampleAreaSize.x && candidate.y >=0 && candidate.y < sampleAreaSize.y)
        {
            //checking the cell of the candidate point
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            // checking any points in a 5x5 grid around the cell of the candidate
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            // classic double for loop going through the cells in  the grid
            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    // index of the point in the current grid cell (-1 because we count indexes from 1)
                    int pointIndex = grid[x, y] - 1;

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
