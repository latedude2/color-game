using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] RailNode railNode;
    [SerializeField] RailSystem railSystem;
    [SerializeField] Activatable[] activatableComponents;
    Animation anim;
    public delegate void PressurePlateHandler();
    public static event PressurePlateHandler Pressed;

    void Start() {
        anim = GetComponent<Animation>();
        activatableComponents = transform.GetComponents<Activatable>();
        if(activatableComponents == null)
        {
            Debug.LogError("Button does not have activatable item connected");
        }
    }

    void OnTriggerEnter(Collider other) {
        Pressed?.Invoke();
        if(activatableComponents.Length > 0)
        {
            foreach (Activatable activatable in activatableComponents)
            {
                activatable.Activate();
            }
        }

        railNode.Interact();
        
        anim.Play("PressurePlateAnimation"); 
    }

    void OnTriggerExit(Collider other) {
        
        anim.Play("PressurePlateUpAnimation");
    }
}
