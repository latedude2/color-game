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
        GameObject target = FindTargetedGameObject();
        if(target != null)
        {
            if(target.GetComponent<Interactable>() != null)
            {
                pointingAt = PointingAt.interactable;
                if(Input.GetMouseButtonDown(0))
                {
                    target.GetComponent<Interactable>().Interact();
                    if (GetComponentInParent<Respawner>() != null)
                    {
                        GetComponentInParent<Respawner>().SaveSpawnPoint();
                    }
                }
            }
            else if(IsTargetGrabbable(target))
            {
                pointingAt = PointingAt.grabbable;
            } 
        }
        PointedAt?.Invoke(pointingAt);
    }

    bool IsTargetGrabbable(GameObject target)
    {   
        Rigidbody rb = GetRigidbodyInGameObjectObjectOrParent(target);
        if(rb == null || rb.isKinematic)
        {
            return false;
        }
        if (rb.GetComponent<BreakableObjectPhysics>() != null)
        {
            return true;
        }
        return false;
    }

    Rigidbody GetRigidbodyInGameObjectObjectOrParent(GameObject target)
    {
        Rigidbody rb = null;
        rb = target.GetComponent<Rigidbody>();
        if(rb == null && target.transform.parent != null)
        {
            rb = target.transform.parent.GetComponent<Rigidbody>();
        }
        return rb;
    }

    GameObject FindTargetedGameObject()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position , transform.forward , out hitInfo , gameObject.GetComponentInParent<GrabIt>().GetGrabDistance()))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
}
