using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InvisibleTrigger : MonoBehaviour
{
    public GameObject[] activatableObjects;
    Activatable[] _activatables;

    void Start()
    {
        List<Activatable> activatables = new List<Activatable>();

        activatables.AddRange(transform.GetComponents<Activatable>());

        foreach (GameObject obj in activatableObjects)
        {
            Debug.Log(obj.GetComponents<Activatable>().Length);
            activatables.AddRange(obj.GetComponents<Activatable>());
        }

        _activatables = activatables.ToArray();

        if(_activatables == null)
        {
            Debug.LogError("Button does not have activatable item connected");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(_activatables.Length > 0)
        {
            foreach (Activatable activatable in _activatables)
            {
                activatable.Activate();
            }
        }
    }
}
