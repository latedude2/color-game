using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RailNode : MonoBehaviour, Activatable
{
    RailSystem railSystem;
    [System.NonSerialized] public int nodeID;

    public UnityEvent nodeDeactivated;

    private void Start() {
        railSystem = transform.parent.GetComponent<RailSystem>();
    }
    public void Activate()
    {
        railSystem.SetTargetID(nodeID);
    }

    public void Deactivate()
    {
        nodeDeactivated.Invoke();
    }
}
