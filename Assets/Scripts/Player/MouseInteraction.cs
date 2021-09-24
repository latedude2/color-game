using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.GrabIt;

public class MouseInteraction : MonoBehaviour
{
    void Start() {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position , transform.forward , out hitInfo , gameObject.GetComponentInParent<GrabIt>().GetGrabDistance()))
        {
            Debug.Log(hitInfo.collider);
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if(interactable != null){							
                interactable.interact();
            }
        }
    }
}
