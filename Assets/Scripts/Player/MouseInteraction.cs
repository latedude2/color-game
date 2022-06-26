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
        Rigidbody rb = null;
        if(target.TryGetComponent<Rigidbody>(out rb) || (target.transform.parent != null && target.transform.parent.TryGetComponent<Rigidbody>(out rb)))
        {
            if(rb.GetComponent<BreakableObjectPhysics>() != null && rb.isKinematic)
            {
                return false;
            }
            return true;
        }
        
        return false;
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
