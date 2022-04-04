using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour, Activatable
{
    bool activated = false;
    [SerializeField] RailNode railNode;
    [SerializeField] RailSystem railSystem;
    [SerializeField] GameObject[] activatableObjects;
    Activatable[] _activatables;


    Animation anim;
    public delegate void PressurePlateHandler();
    public static event PressurePlateHandler Pressed;

    void Start() {
        List<Activatable> activatables = new List<Activatable>(transform.GetComponents<Activatable>());

        activatables.Remove(this);

        foreach (GameObject obj in activatableObjects)
        {
            Debug.Log(obj.GetComponents<Activatable>().Length);
            activatables.AddRange(obj.GetComponents<Activatable>());
        }

        _activatables = activatables.ToArray();

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
            if(_activatables.Length > 0)
            {
                foreach (Activatable activatable in _activatables)
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
            if(_activatables.Length > 0)
            {
                foreach (Activatable activatable in _activatables)
                {
                    activatable.Deactivate();
                }
            }
            anim.Play("PressurePlateUpAnimation");
        }
        
    }
}
