using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailNode : MonoBehaviour, Interactable
{
    RailSystem railSystem;
    [System.NonSerialized] public int nodeID;

    private void Start() {
        railSystem = transform.parent.GetComponent<RailSystem>();
    }
    public void Interact()
    {
        railSystem.SetTargetID(nodeID);
    }
}
