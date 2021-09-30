using UnityEngine;
using Lightbug.GrabIt;

public class MouseInteraction : MonoBehaviour
{
    public enum PointingAt
    {
        nothing   = 0b_0000_0000,
        interactable     = 0b_0000_0001,
        grabbable   = 0b_0000_0010
    }

    public delegate void MousePointerHandler(PointingAt pointing);
    public static event MousePointerHandler PointedAt;

    private PointingAt pointingAt = PointingAt.nothing;

    void Update()
    {
        pointingAt = PointingAt.nothing;
        Interactable interactableObject = FindInteractable();
        if(interactableObject != null)
        {
            pointingAt = PointingAt.interactable;
            if(Input.GetMouseButtonDown(0))
            {
                interactableObject.interact();
            }
        }
        if(FindGrabbable() != null)
        {
            pointingAt = PointingAt.grabbable;
        }
        PointedAt?.Invoke(pointingAt);
    }

    Interactable FindInteractable()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position , transform.forward , out hitInfo , gameObject.GetComponentInParent<GrabIt>().GetGrabDistance()))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            return interactable;
        }
        return null;
    }

    GameObject FindGrabbable()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position , transform.forward , out hitInfo , gameObject.GetComponentInParent<GrabIt>().GetGrabDistance()))
        {
            Rigidbody grabbable = hitInfo.collider.GetComponent<Rigidbody>();
            if(grabbable != null)
                return grabbable.gameObject;
            else return null;
        }
        return null;
    }
}
