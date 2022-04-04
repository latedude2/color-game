using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Activatable
{
    bool activated = false;
    [SerializeField] RailNode railNode;
    [SerializeField] RailSystem railSystem;
    [SerializeField] GameObject[] gameObjectsToActivate;
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
            if(gameObjectsToActivate.Length > 0)
            {
                foreach (GameObject activatableGameobject in gameObjectsToActivate)
                {
                    activatableGameobject.GetComponent<Activatable>().Activate();
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
            if(gameObjectsToActivate.Length > 0)
            {
                foreach (GameObject activatableGameobject in gameObjectsToActivate)
                {
                    activatableGameobject.GetComponent<Activatable>().Deactivate();
                }
            }
            anim.Play("PressurePlateUpAnimation");
        }
        
    }
}
