using System.Collections.Generic;
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
    private Vector3[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;
    private GrabIt grabIt;
    private bool visible = false;

    [SerializeField] [Range(1, 5)] int shinePointMultiplier = 1;
    

    Vector3 velocity;

    void Start()
    {
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        grabIt = GameObject.Find("Main Camera").GetComponent<GrabIt>();
        _renderer = GetComponent<Renderer>();
        blackMat = Resources.Load<Material>("Materials/Black");
        _renderer.material = blackMat;
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    void FixedUpdate()
    {
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

    private void SetToVisible()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().velocity = velocity;
        }
        _collider.enabled = true;
        visible = true;
    }

    private void SetToInvisible()
    {
        if (grabIt.m_targetRB != null && gameObject == grabIt.m_targetRB.gameObject)
            grabIt.Drop();
        SetColor(ColorCode.Black);
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            velocity = gameObject.GetComponent<Rigidbody>().velocity;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        _collider.enabled = false;
        visible = false;
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
                if (pointReached(point, light))
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
                    if (!IsPointInMiddle(x, y, z, shinePointMultiplier))
                        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x * x / shinePointMultiplier, b.size.y * y / shinePointMultiplier, b.size.z * z / shinePointMultiplier) * 0.50f));
                }
            }
        }

        return shinepoints.ToArray();
    }

    bool IsPointInMiddle(int x, int y, int z, int shinePointMultiplier)
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

    bool pointReached(Vector3 point, GameObject pointingLight)
    {
        Vector3 lightPos = pointingLight.transform.position;
        if (ColorGame.Debug.debugMode)
        {
            UnityEngine.Debug.DrawLine(point, lightPos, Color.red);
        }
        return !Physics.Linecast(point, lightPos, 0);
    }

}
