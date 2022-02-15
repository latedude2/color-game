using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class WireBuilder : MonoBehaviour
{
    [System.NonSerialized] public Vector3 lineEnd = Vector3.right;
    [System.NonSerialized] public Vector3 lineStart = Vector3.right / 2;
    public GameObject wirePrefab;


    public void AddWire()
    {
        Vector3 spawnPoint = Vector3.Lerp(transform.TransformPoint(lineStart), transform.TransformPoint(lineEnd), 0.5f);
        GameObject newWire = Instantiate(wirePrefab, spawnPoint, Quaternion.identity);
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
        UnityEditor.Selection.objects = newEditorSelection;    
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