using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
public class WireBuilder : MonoBehaviour
{
    [System.NonSerialized] public Vector3 lineEnd = Vector3.right;
    [System.NonSerialized] public Vector3 lineStart = Vector3.right / 2;
    public GameObject wirePrefab;

    [SerializeField] [Range(1, 20)] public int treeLength = 5;
    [SerializeField] [Range(1, 3)] public int branchCount = 3;
    public bool randomizeBranchLength = true;

    List<Vector3> possibleLineEndPositions;


    public void FindPosition(bool randomLength = true)
    {
        possibleLineEndPositions = new List<Vector3>(); 
        float lengthMultiplier = 1f;
        if(randomLength)
        {
            lengthMultiplier = UnityEngine.Random.Range(0.3f, 1f);
        }
        checkDirection(-Vector3.forward * lengthMultiplier, 50);
        checkDirection(Vector3.forward * lengthMultiplier, 50);
        checkDirection(Vector3.right * lengthMultiplier);
        Debug.Log("Found " + possibleLineEndPositions.Count + " possible positions"); 
    }

    public GameObject SpawnRandomSegment()
    {
        GameObject newWire;
        if(possibleLineEndPositions.Count > 0)
        {
            Vector3 endPosition = possibleLineEndPositions[UnityEngine.Random.Range(0, possibleLineEndPositions.Count)]; 
            lineEnd = endPosition;
            newWire = AddWire();
            possibleLineEndPositions.Remove(endPosition);
            return newWire;
        }
        else 
            return null;
    }

    public void checkDirection(Vector3 direction, float multiplier = 1)
    {
        RaycastHit hit;
        LayerMask layerMask = 0b_0001_0000_1001; //Block rays with default, static and ignore outline layers
        var distance = 5;
        Debug.DrawRay(transform.TransformPoint(lineStart), transform.TransformDirection(direction * distance), Color.magenta, 2f);
        if (!Physics.Raycast(transform.TransformPoint(lineStart), transform.TransformDirection(direction), out hit, distance, layerMask)) 
        {
            possibleLineEndPositions.Add(lineStart + direction * multiplier); 
        }
    }

    public void iterateGeneration(int iteration = 3, int branchCount = 2, bool randomLength = true) 
    {   //Bug: the wires will tangle with each other because they all spawn at once, so the raycasts fail
        possibleLineEndPositions = new List<Vector3>();
        if(iteration > 0)
        {
            iteration--;
            FindPosition(randomLength);
            for(int i = 0; i < branchCount; i++)
            {
                GameObject newWire = SpawnRandomSegment();
                if(newWire != null)
                {
                    newWire.GetComponent<WireBuilder>().iterateGeneration(iteration, branchCount, randomLength);
                }
            }
        }
    }

    public GameObject AddWire()
    {
        Vector3 spawnPoint = Vector3.Lerp(transform.TransformPoint(lineStart), transform.TransformPoint(lineEnd), 0.5001f); //the wire is spawned slightly in front so that it is later detected by raycasting
        GameObject newWire = Instantiate(wirePrefab, spawnPoint, Quaternion.identity);
        Undo.RegisterCreatedObjectUndo(newWire, "Created new wire");
        newWire.name = "Wire";
        newWire.transform.localScale = new Vector3(
            Vector3.Distance(transform.TransformPoint(lineStart), transform.TransformPoint(lineEnd)), 
            newWire.transform.localScale.y, 
            newWire.transform.localScale.z);
        
        newWire.GetComponent<WireSurface>()._color = gameObject.GetComponent<WireSurface>()._color;

        newWire.transform.LookAt(transform.TransformPoint(lineEnd));
        newWire.transform.Rotate(new Vector3(0,-90,0));

        GetComponent<WireSurface>().gameObjectsToActivate.Add(newWire);  
        RotateWireToStickToWall(Vector3.Distance(transform.TransformPoint(lineStart), transform.TransformPoint(lineEnd)), newWire.transform);  

        GameObject[] newEditorSelection = new GameObject[1];
        newEditorSelection[0] = newWire;

        newWire.GetComponent<WireSurface>().gameObjectsToActivate = new List<GameObject>();
        return newWire;
    }

    private void RotateWireToStickToWall(float newWireLength, Transform newWire)
    {
        RaycastHit hit;
        LayerMask layerMask = 0b_0000_1001; //Block rays with default and static layers
        float distance = newWireLength / 2;

        if (Physics.Raycast(newWire.position, newWire.forward, out hit, distance, layerMask))
        {
            //Debug.Log("Hit forward, rotating");
            newWire.transform.Rotate(new Vector3(90,0,0));
        }
        if (Physics.Raycast(newWire.position, -newWire.forward, out hit, distance, layerMask))
        {
            //Debug.Log("Hit back, rotating");
            newWire.transform.Rotate(new Vector3(90,0,0));
            
        }
    }

    public void RotateLeft()
    {
        transform.Rotate(new Vector3(90,0,0));
    }
}
#endif