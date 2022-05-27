using System.Collections;
using System.Collections.Generic;
using Lightbug.GrabIt;
using UnityEngine;

public class BreakableObjectPhysics : VisibleObjectPhysics
{
    [SerializeField] bool shattered = false;

    void Awake()
    {
        grabIt = GameObject.Find("Main Camera").GetComponent<GrabIt>();
        _collider = GetComponent<Collider>();
        if (TryGetComponent(out Rigidbody rb))
        {
            _rigidbody = rb;
        }
    }

    override public void SetToVisible()
    {
        if (_rigidbody != null)
        {
            UnfreezeMotion();
        }
        _collider.enabled = true;
        visible = true;
        justMadeVisible = true;
        if(shattered) gameObject.layer = 0; //default
    }

    override public void SetToInvisible()
    {
        if (grabIt.m_targetRB != null && gameObject == grabIt.m_targetRB.gameObject)
            grabIt.Drop();
        if (_rigidbody != null)
            FreezeMotion();
        RemoveAllObjectConnections();
        visible = false;
        if(shattered) gameObject.layer = 9; //shatteredNotVisible
    }
}
