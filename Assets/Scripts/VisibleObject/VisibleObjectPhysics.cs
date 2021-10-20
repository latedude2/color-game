using System.Collections.Generic;
using System;
using UnityEngine;
using Lightbug.GrabIt;
using DimBoxes;

public class VisibleObjectPhysics : MonoBehaviour, Activatable
{
    private Collider _collider;
    private Rigidbody _rigidbody;
    private GrabIt grabIt;
    private List<ObjectConnection> objectConnections = new List<ObjectConnection>();
    
    //Used for keeping the velocity of a non-visible object
    Vector3 velocity;

    bool visible;
    bool justMadeVisible;


    void Start()
    {
        grabIt = GameObject.Find("Main Camera").GetComponent<GrabIt>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        if (TryGetComponent(out Rigidbody rb))
        {
            _rigidbody = rb;
            _rigidbody.isKinematic = true;
        }
    }

    void FixedUpdate()
    {
        if(visible)
            justMadeVisible = false;
        if (_rigidbody != null)
            _rigidbody.WakeUp();
    }

    void OnCollisionEnter(Collision collision)
    {
        VisibleObjectPhysics visibleObject = collision.collider.GetComponent<VisibleObjectPhysics>();
        if(visibleObject != null)
        {
            if (justMadeVisible)
            {
                AddObjectConnection(visibleObject);
                if (!visibleObject.justMadeVisible)
                    visibleObject.AddObjectConnection(this);
            }
        }
    }

    public void SetToVisible()
    {
        if (_rigidbody != null)
        {
            UnfreezeMotion();
        }
        _collider.enabled = true;
        visible = true;
        justMadeVisible = true;
    }

    public void SetToInvisible()
    {
        if (grabIt.m_targetRB != null && gameObject == grabIt.m_targetRB.gameObject)
            grabIt.Drop();
        if (_rigidbody != null)
            FreezeMotion();
        RemoveAllObjectConnections();
        _collider.enabled = false;
        visible = false;
    }

    public void FreezeMotion(bool resetVelocity = false)
    {
        velocity = resetVelocity ? Vector3.zero : _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    public void UnfreezeMotion()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = velocity;
    }

    public void AddObjectConnection(VisibleObjectPhysics other)
    {
        objectConnections.Add(new ObjectConnection(this, other));
    }

    public void RemoveObjectConnection(VisibleObjectPhysics other)
    {
        for (int i = objectConnections.Count - 1; i >= 0; i--)
        {
            if (objectConnections[i].connectedObject == other)
            {
                objectConnections[i].RemoveFixedJoint();
                objectConnections.RemoveAt(i);
            }
        }
    }

    public void RemoveAllObjectConnections()
    {
        for (int i = objectConnections.Count - 1; i >= 0; i--)
        {
            objectConnections[i].connectedObject.RemoveObjectConnection(this);
            objectConnections[i].RemoveFixedJoint();
            objectConnections.RemoveAt(i);
        }
    }

    public void activate()
    {
        SetToVisible();
    }

    public void deactivate()
    {
        SetToInvisible();
    }
}
