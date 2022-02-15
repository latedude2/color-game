using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
