using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredLight : MonoBehaviour, Activatable
{
    [SerializeField] private bool enabledAtStart = false;
    [SerializeField] private ColorCode color = ColorCode.White;
    [SerializeField] private GameObject[] signifiers;
    Light _light;

    EmittingSurface lightBulb;

    // Start is called before the first frame update
    void Start()
    {
        _light = gameObject.GetComponent<Light>();
        lightBulb = transform.parent.GetComponentInChildren<EmittingSurface>();
        SetLightColor(color);
        if (enabledAtStart)
            SetSignifierColor(color);
        else
            SetSignifierColor(ColorCode.White);
        Invoke(nameof(SetInitialEnabled), 0.1f);
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
            signifier.GetComponent<Renderer>().material.SetColor("_Color", ColorHelper.GetColor(newColor));
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
        SetSignifierColor(ColorCode.White);
    }
}
