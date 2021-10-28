using System.Collections.Generic;
using System;
using UnityEngine;
using DimBoxes;

public class VisibleObjectVisibility : MonoBehaviour
{
    protected ColorCode color = ColorCode.Black;
    public ColorCode trueColor = ColorCode.Black;
    private Renderer _renderer;
    private Material colorMat;
    private Material blackMat;
    private BoundBox boundBox;
    private ShinePoint[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;
    protected bool visible = false;
    [SerializeField] [Range(1, 5)] int shinePointMultiplier = 1;
    Activatable[] activatableComponents;
    
    
    void Start()
    {
        activatableComponents = GetComponents<Activatable>();
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        _renderer = GetComponent<Renderer>();
        if(trueColor == ColorCode.Black)
        {
            trueColor = (ColorCode) Enum.Parse(typeof(ColorCode), _renderer.material.name.Replace("(Instance)",""));
        }
        blackMat = Resources.Load<Material>("Materials/Black");
        _renderer.material = blackMat;
        boundBox = GetComponent<BoundBox>();
        boundBox.lineColor = ColorHelper.GetColor(trueColor);
        boundBox.SetLineRenderers();
        
    }

    void FixedUpdate()
    {
        shinePoints = FindShinePoints();
        // When object becomes lit and interactable
        ColorCode objectFinalColor = FindShownColor();
        //Check object should be visible
        SetVisibility(objectFinalColor);
    }

    protected virtual void SetVisibility(ColorCode objectFinalColor)
    {
        if (objectFinalColor != ColorCode.Black)
        {
            if (objectFinalColor != color)
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
                SetColor(ColorCode.Black);
                SetToInvisible();
            }
        }
    }

    protected void SetToVisible()
    {
        foreach (Activatable activatable in activatableComponents)
        {
            activatable.Activate();
        }
        visible = true;
    }

    protected void SetToInvisible()
    {
        foreach (Activatable activatable in activatableComponents)
        {
            activatable.Deactivate();
        }
        visible = false;
    }

    protected void SetColor(ColorCode color)
    {
        colorMat = Resources.Load<Material>("Materials/" + color.ToString());
        _renderer.material = colorMat;

        if (color == trueColor)
        {
            boundBox.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            boundBox.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Outline");
        }
    }

    private ColorCode FindShownColor()
    {
        ColorCode finalColor = ColorCode.Black;
        var gameObjectLayer = gameObject.layer;
        gameObject.layer = 0b_0000_0010;    //We ignore the object itself since we assume the visible object will always be concave
        if(trueColor.HasFlag(ColorCode.Green) && IsShinedByColor(ColorCode.Green))
        {
            finalColor = finalColor | ColorCode.Green;
        }
        if(trueColor.HasFlag(ColorCode.Red) && IsShinedByColor(ColorCode.Red))
        {
            finalColor = finalColor | ColorCode.Red;
        }
        if(trueColor.HasFlag(ColorCode.Blue) && IsShinedByColor(ColorCode.Blue))
        {
            finalColor = finalColor | ColorCode.Blue;
        }
        gameObject.layer = gameObjectLayer; //set back the original layer
        return finalColor;
    }

    private bool IsShinedByColor(ColorCode color)
    {
        foreach (ShinePoint shinePoint in shinePoints)
        {
            foreach (GameObject light in LightManager.GetPointingLights(shinePoint.GetPosition(), color))
            {
                if (shinePoint.Reached(light))
                {
                    return true;
                }
            }
        }
        return false;
    }

    ShinePoint[] FindShinePoints()
    {
        //Create a list of points for the visible object that we will be checking for shine.
        List<ShinePoint> shinepoints = new List<ShinePoint>();
        if (TryGetComponent<MeshCollider>(out MeshCollider collider)) {
            Mesh mesh = collider.sharedMesh;
            foreach (var vertice in mesh.vertices) {
                shinepoints.Add(new ShinePoint(transform.TransformPoint(vertice)));
            }
        } else {

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
                            shinepoints.Add(new ShinePoint(transform.TransformPoint(b.center + new Vector3(b.size.x * x / shinePointMultiplier, b.size.y * y / shinePointMultiplier, b.size.z * z / shinePointMultiplier) * 0.50f)));
                    }
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
                foreach (ShinePoint point in shinePoints)
                {
                    Gizmos.DrawSphere(point.GetPosition(), .05f);
                }
            }
            catch
            {
                new UnityException();
            }
        }
    }
}