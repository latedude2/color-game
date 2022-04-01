using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Activatable
{
    bool activated = false;
    [SerializeField] RailNode railNode;
    [SerializeField] RailSystem railSystem;
    [SerializeField] List<Activatable> activatableComponents = new List<Activatable>();
    Animation anim;
    public delegate void PressurePlateHandler();
    public static event PressurePlateHandler Pressed;

    void Start() {
        anim = GetComponent<Animation>();
        railNode.nodeDeactivated.AddListener(Deactivate);
    }

    void OnTriggerEnter(Collider other) {
        Activate();
    }

    public void Activate()
    {
        if(!activated)
        {
            activated = true;
            Pressed?.Invoke();
            if(activatableComponents != null && activatableComponents.Count > 0)
            {
                foreach (Activatable activatable in activatableComponents)
                {
                    activatable.Activate();
                }
            }

            railNode.Activate();
            
            anim.Play("PressurePlateAnimation"); 
        }
    }
    public void Deactivate()
    {
        if(activated)
        {
            activated = false;
            if(activatableComponents != null && activatableComponents.Count > 0)
            {
                foreach (Activatable activatable in activatableComponents)
                {
                    activatable.Deactivate();
                }
            }
            anim.Play("PressurePlateUpAnimation");
        }
        
    }
}
