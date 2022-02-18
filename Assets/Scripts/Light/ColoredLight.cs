using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]

public class ColoredLight : MonoBehaviour, Activatable
{
    [SerializeField] private bool enabledAtStart = false;
    public ColorCode color = ColorCode.White;
    [SerializeField] private GameObject[] signifiers;
    Light _light;

    EmittingSurface lightBulb;

    // Start is called before the first frame update
    void Start()
    {
        _light = gameObject.GetComponent<Light>();
        lightBulb = transform.parent.GetComponentInChildren<EmittingSurface>();
        SetColorAtStart();
        Invoke(nameof(SetInitialEnabled), 0.1f);
    }
    
    public void SetColorAtStart()
    {
        SetLightColor(color);
        if (enabledAtStart || !IsColorSwitchable())
            SetSignifierColor(color);
        else
            SetSignifierColor(ColorCode.White);
        
    }

    void SetInitialEnabled()
    {
        gameObject.SetActive(enabledAtStart);
        lightBulb.SetColor(color);
        lightBulb.SetActive(enabledAtStart);
    }

    public void SetColor(ColorCode newColor)
    {
        SetLightColor(newColor);
        SetSignifierColor(newColor);
    }

    private void SetSignifierColor(ColorCode newColor)
    {
        foreach (var signifier in signifiers) {
            var tempMaterial = new Material(signifier.GetComponent<Renderer>().sharedMaterial);
            tempMaterial.SetColor("_Color", ColorHelper.GetColor(newColor));
            signifier.GetComponent<Renderer>().sharedMaterial = tempMaterial;
        }
    }

    private void SetLightColor(ColorCode newColor)
    {
        color = newColor;
        _light.color = ColorHelper.GetColor(newColor);
    }

    public ColorCode GetColorCode()
    {
        return color;
    }

    public void Activate()
    {
        lightBulb.SetActive(true);
        gameObject.SetActive(true);
        SetSignifierColor(color);
    }

    public void Deactivate()
    {
        lightBulb.SetActive(false);
        gameObject.SetActive(false);
    }

    private bool IsColorSwitchable()
    {
        GameObject parent = transform.parent.transform.parent.transform.parent.gameObject;
        SetLightColor[] setLightColors = parent.GetComponentsInChildren<SetLightColor>();
        bool IsColorSwitchable = setLightColors.Length > 0 ? true : false;

        return IsColorSwitchable;
    }
}
