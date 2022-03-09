using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailButton : MonoBehaviour, Interactable
{
    [SerializeField] RailNode railNode;
    [SerializeField] RailSystem railSystem;
    [SerializeField] Activatable[] activatableComponents;
    Animation anim;

    void Start() {
        anim = GetComponent<Animation>();
        activatableComponents = transform.GetComponents<Activatable>();
        if(activatableComponents == null)
        {
            Debug.LogError("Button does not have activatable item connected");
        }
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

        railNode.Interact();
        
        anim.Play("ButtonAnimation");
    }
}
