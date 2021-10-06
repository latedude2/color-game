using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectConnection
{
    FixedJoint fixedJoint;
    public VisibleObject connectedObject { get; }
    public VisibleObject connectorObject { get; }

    public ObjectConnection(VisibleObject connector, VisibleObject connected)
    {
        this.connectorObject = connector;
        this.connectedObject = connected;
        AddFixedJoint();
    }

    private void AddFixedJoint()
    {
        if (connectorObject.GetComponent<Rigidbody>() != null)
        {
            fixedJoint = connectorObject.gameObject.AddComponent<FixedJoint>();
            if (connectedObject.GetComponent<Rigidbody>() != null)
                fixedJoint.connectedBody = connectedObject.GetComponent<Rigidbody>();
            else
                connectorObject.FreezeMotion(true);
        }
    }

    public void RemoveFixedJoint()
    {
        if (fixedJoint != null)
        {
            Object.Destroy(fixedJoint);
            connectorObject.UnfreezeMotion();
        }
    }

}
