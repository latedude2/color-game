using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteAlways]
public class WireBuilder : MonoBehaviour
{
    [System.NonSerialized] public Vector3 lineEnd = Vector3.right / 2;
    [System.NonSerialized] public Vector3 lineStart = Vector3.right / 2;
    public GameObject wirePrefab;

    [SerializeField] [Range(1, 15)] public int treeLength = 4;
    [SerializeField] [Range(1, 3)] public int branchCount = 2;
    public bool randomizeBranchLength = true;
    
    public bool followWalls = true;
    public float maxWallDistance = 0.2f;
    public float maxWireLength = 5f;
    List<Vector3> possibleLineEndPositions;

    [SerializeField] float minimumLength = 0.2f;

    private Activater activater;

    void OnDestroy() {
        if(gameObject.scene.isLoaded) //Gameobject was Deleted
        {
            WireBuilder wireBuilder= GetComponent<WireBuilder>();
            if(wireBuilder.activater != null)
                wireBuilder.activater.RemoveActivatable(gameObject);
        }
        
    }

    public void FindPosition(bool randomLength = true)
    {
        possibleLineEndPositions = new List<Vector3>(); 
        float lengthMultiplier = 1f;
        if(randomLength)
        {
            lengthMultiplier = UnityEngine.Random.Range(0.3f, 1f);
        }
        checkDirection(-Vector3.forward * lengthMultiplier, maxWireLength  / transform.localScale.z, followWalls);
        checkDirection(Vector3.forward * lengthMultiplier, maxWireLength  / transform.localScale.z, followWalls);
        checkDirection(Vector3.right * lengthMultiplier, maxWireLength / transform.localScale.x, followWalls);
        checkDirection(Vector3.up * lengthMultiplier, maxWireLength / transform.localScale.y , followWalls);
        checkDirection(-Vector3.up * lengthMultiplier, maxWireLength / transform.localScale.y , followWalls);
        Debug.Log("Found " + possibleLineEndPositions.Count + " possible positions"); 
    }

    public GameObject SpawnRandomSegment()
    {
        FindPosition(randomizeBranchLength);
        GameObject newWire = null;
        if(possibleLineEndPositions.Count > 0)
        {
            Vector3 endPosition = possibleLineEndPositions[UnityEngine.Random.Range(0, possibleLineEndPositions.Count)]; 
            lineEnd = endPosition;
            newWire = AddWire();
            possibleLineEndPositions.Remove(endPosition);
        }
        return newWire;
    }

    public void checkDirection(Vector3 direction, float multiplier = 1, bool checkForWalls = true)
    {
        RaycastHit hit;
        LayerMask layerMask = 0b_0001_0000_1011; //Block rays with default, static and ignore outline layers
        float distance = maxWireLength;
        
        bool hitSomething = Physics.Raycast(transform.TransformPoint(lineStart), transform.TransformDirection(direction), out hit, distance, layerMask);
        if (hitSomething)
        {
            if(hit.distance < minimumLength)
                return;
            distance = hit.distance;
        }

        //Debug.DrawRay(transform.TransformPoint(lineStart), transform.TransformDirection(direction * distance), Color.magenta, 2f);
        Vector3 potentialLineEnd = lineStart + direction * multiplier * (distance/maxWireLength);

        if(checkForWalls && (!checkWallExists(lineStart, distance) | !checkWallExists(potentialLineEnd, distance)))
        {
            Debug.Log("Missing wall to attach to");
            return;
        }
        possibleLineEndPositions.Add(potentialLineEnd); 
    }

    private bool checkWallExists(Vector3 position, float distance){
        if(wallCheck(transform.TransformPoint(position), transform.up, maxWallDistance) | wallCheck(transform.TransformPoint(position), -transform.up, maxWallDistance))
        {
            return true;
        }
        
        if(wallCheck(transform.TransformPoint(position), transform.right, maxWallDistance) | wallCheck(transform.TransformPoint(position), -transform.right, maxWallDistance))
        {
            return true;
        }
        
        return false;
    }

    

    public void iterateGeneration(int iteration = 3, int branchCount = 2, bool randomLength = true, GameObject wireSystem = null) 
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
                    newWire.transform.SetParent(wireSystem.transform);
                    newWire.GetComponent<WireBuilder>().iterateGeneration(iteration, branchCount, randomLength, wireSystem);
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
        newWire.transform.Rotate(new Vector3(0, -90, 0));

        GetComponent<WireSurface>().gameObjectsToActivate.Add(newWire);  
        SaveChangeHack(newWire);
        newWire.GetComponent<WireSurface>().activater = GetComponent<WireSurface>();
        newWire.GetComponent<WireBuilder>().activater = GetComponent<WireSurface>();
        
        RotateWireToStickToWall(Vector3.Distance(transform.TransformPoint(lineStart), transform.TransformPoint(lineEnd)), newWire.transform);  

        GameObject[] newEditorSelection = new GameObject[1];
        newEditorSelection[0] = newWire;

        newWire.GetComponent<WireSurface>().gameObjectsToActivate = new List<GameObject>();
        return newWire;
    }

    private void SaveChangeHack(GameObject newWire)
    //HACK: The addition of element does not get saved for some reason and reading the component works as a workaround
    {
        List<GameObject> activateGameObjectList = GetComponent<WireSurface>().gameObjectsToActivate;
        ColorCode savedColor = GetComponent<WireSurface>()._color;
        DestroyImmediate(GetComponent<WireSurface>());
        gameObject.AddComponent<WireSurface>();
        GetComponent<WireSurface>().gameObjectsToActivate = activateGameObjectList;
        GetComponent<WireSurface>()._color = savedColor;

    }

    private void RotateWireToStickToWall(float newWireLength, Transform newWire)
    {
        float distance = maxWallDistance;
        if(wallCheck(newWire.position, newWire.forward, distance) || wallCheck(newWire.position, -newWire.forward, distance))
            newWire.transform.Rotate(new Vector3(90,0,0));
    }

    public bool wallCheck(Vector3 position, Vector3 direction, float distance)
    {
        RaycastHit hit;
        LayerMask layerMask = 0b_0001_0000_1011; //Block rays with default, transparentFX and static layers
        //Debug.DrawRay(position, direction * distance, Color.green, 2f);
        return Physics.Raycast(position, direction, out hit, distance, layerMask);
    }

    

    public void RotateLeft()
    {
        transform.Rotate(new Vector3(90,0,0));
    }
}
#endif