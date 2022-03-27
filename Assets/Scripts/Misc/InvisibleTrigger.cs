using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InvisibleTrigger : MonoBehaviour, Interactable
{
    public Activatable[] activatableComponents;

    void Start()
    {
        activatableComponents = transform.GetComponents<Activatable>();
        if(activatableComponents == null)
        {
            Debug.LogError("Button does not have activatable item connected");
        }
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        if (!enabled)
            return;

        if(activatableComponents.Length > 0)
        {
            foreach (Activatable activatable in activatableComponents)
            {
                activatable.Activate();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(activatableComponents.Length > 0)
        {
            foreach (Activatable activatable in activatableComponents)
            {
                activatable.Activate();
            }
        }
    }
}
