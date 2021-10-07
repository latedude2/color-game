using System.Collections.Generic;
using System;
using UnityEngine;
using Lightbug.GrabIt;

public class VisibleObject : MonoBehaviour
{
    private ColorCode color = ColorCode.Black;
    [SerializeField] private ColorCode trueColor = ColorCode.Black;
    private Renderer _renderer;
    private Material colorMat;
    private Material blackMat;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private Vector3[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;
    private GrabIt grabIt;
    public List<ObjectConnection> objectConnections = new List<ObjectConnection>();
    private bool visible = false;

    [SerializeField] [Range(1, 5)] int shinePointMultiplier = 1;
    LayerMask blockingLayers;
    
    //Used for keeping the velocity of a non-visible object
    Vector3 velocity;

    bool justMadeVisible = false;

    void Start()
    {
        blockingLayers = 0b_0000_1001; //Block rays with default and static layers
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        grabIt = GameObject.Find("Main Camera").GetComponent<GrabIt>();
        _renderer = GetComponent<Renderer>();
        if(trueColor == ColorCode.Black)
        {
            trueColor = (ColorCode) Enum.Parse(typeof(ColorCode), _renderer.material.name.Replace("(Instance)",""));
        }
        blackMat = Resources.Load<Material>("Materials/Black");
        _renderer.material = blackMat;
        
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
        shinePoints = FindShinePoints();
        // When object becomes lit and interactable
        ColorCode objectFinalColor = FindShownColor();
        //If object should be visible
        if (objectFinalColor != ColorCode.Black)
        {
            if(objectFinalColor != color)
                SetColor(objectFinalColor);
            // If object is not visible, make visible
            if (!visible)
            {
                SetToVisible();
            }
        }
        else
        {
            // If object is visible, make invisible
            if (visible)
            {
                SetToInvisible();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        VisibleObject visibleObject = collision.collider.GetComponent<VisibleObject>();
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

    private void SetToVisible()
    {
        if (_rigidbody != null)
        {
            UnfreezeMotion();
        }
        _collider.enabled = true;
        visible = true;
        justMadeVisible = true;
    }

    private void SetToInvisible()
    {
        if (grabIt.m_targetRB != null && gameObject == grabIt.m_targetRB.gameObject)
            grabIt.Drop();
        SetColor(ColorCode.Black);
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

    public void AddObjectConnection(VisibleObject other)
    {
        objectConnections.Add(new ObjectConnection(this, other));
    }

    public void RemoveObjectConnection(VisibleObject other)
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



    private void SetColor(ColorCode color)
    {
        colorMat = Resources.Load<Material>("Materials/" + color.ToString());
        _renderer.material = colorMat;
    }

    private ColorCode FindShownColor()
    {
        ColorCode finalColor = ColorCode.Black;
        if(trueColor.HasFlag(ColorCode.Green) && isShinedOn(ColorCode.Green))
        {
            finalColor = finalColor | ColorCode.Green;
        }
        if(trueColor.HasFlag(ColorCode.Red) && isShinedOn(ColorCode.Red))
        {
            finalColor = finalColor | ColorCode.Red;
        }
        if(trueColor.HasFlag(ColorCode.Blue) && isShinedOn(ColorCode.Blue))
        {
            finalColor = finalColor | ColorCode.Blue;
        }
        return finalColor;
    }

    private bool isShinedOn(ColorCode color)
    {
        foreach (Vector3 point in shinePoints)
        {
            foreach (GameObject light in lightManager.GetPointingLights(point, color))
            {
                
                if (ShinePointReached(point, light))
                {
                    return true;
                }
            }
        }
        return false;
    }

    Vector3[] FindShinePoints()
    {
        //Create a list of points for the visible object that we will be checking for shine.
        List<Vector3> shinepoints = new List<Vector3>();

        BoxCollider b = GetComponent<BoxCollider>();
        
        // Add points on the box collider to the list
        for (int z = -shinePointMultiplier; z <= shinePointMultiplier; z++)
        {
            for (int y = -shinePointMultiplier; y <= shinePointMultiplier; y++)
            {
                for (int x = -shinePointMultiplier; x <= shinePointMultiplier; x++)
                {
                    // skip the middle of the box
                    if (!IsShinePointInMiddle(x, y, z, shinePointMultiplier))
                        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x * x / shinePointMultiplier, b.size.y * y / shinePointMultiplier, b.size.z * z / shinePointMultiplier) * 0.50f));
                }
            }
        }

        return shinepoints.ToArray();
    }

    bool IsShinePointInMiddle(int x, int y, int z, int shinePointMultiplier)
    {
        if(x == shinePointMultiplier || x == -shinePointMultiplier)
            return false;
        if(y == shinePointMultiplier || y == -shinePointMultiplier)
            return false;
        if(z == shinePointMultiplier || z == -shinePointMultiplier)
            return false;
        return true;
    }

    void OnDrawGizmos()
    {
        if (ColorGame.Debug.debugMode)
        {
            Gizmos.color = Color.yellow;
            try
            {
                // Draw spheres for ShinePoints for debugging
                foreach (Vector3 point in shinePoints)
                    Gizmos.DrawSphere(point, .05f);
            }
            catch
            {
                new UnityException();
            }
        }
    }

    bool ShinePointReached(Vector3 point, GameObject pointingLight)
    {
        Vector3 lightPos = pointingLight.transform.position;
        var gameObjectLayer = gameObject.layer;
        gameObject.layer = 0b_0000_0010;    //We ignore the object itself since we assume the visible object will always be concave
        if (ColorGame.Debug.debugMode)
        {
            UnityEngine.Debug.DrawLine(point, lightPos, Color.red);
        }
        bool nothingIsBlockingLight = !Physics.Linecast(point, lightPos, blockingLayers);
        gameObject.layer = gameObjectLayer; //set back the original layer
        return nothingIsBlockingLight;
    }

}
