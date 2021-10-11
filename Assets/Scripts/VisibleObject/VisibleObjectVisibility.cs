using System.Collections.Generic;
using System;
using UnityEngine;
using Lightbug.GrabIt;
using DimBoxes;

public class VisibleObjectVisibility : MonoBehaviour
{
    private ColorCode color = ColorCode.Black;
    [SerializeField] private ColorCode trueColor = ColorCode.Black;
    private Renderer _renderer;
    private Material colorMat;
    private Material blackMat;
    private BoundBox boundBox;
    private ShinePoint[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;
    private bool visible = false;

    [SerializeField] [Range(1, 5)] int shinePointMultiplier = 1;

    VisibleObjectPhysics _physics;
    
    void Start()
    {
        _physics = GetComponent<VisibleObjectPhysics>();
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
                SetColor(ColorCode.Black);
                SetToInvisible();
            }
        }
    }

    private void SetToVisible()
    {
        _physics.SetToVisible();
        visible = true;
    }

    private void SetToInvisible()
    {
        _physics.SetToInvisible();
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