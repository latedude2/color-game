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
        lightBulb = transform.parent.GetComponentInChildren<EmittingSurface>();
        SetColorAtStart();
        if (Application.isPlaying)
        {
            GetComponent<Light>().enabled = true;
            Invoke(nameof(SetInitialEnabled), 0.1f);
        }
        #if UNITY_EDITOR
        toggleShowingLightsInEditor(EditorPrefs.GetBool("ToggleShowingDisabledLights"));
        #endif
    }

    public void SetColorAtStart()
    {
        _light = gameObject.GetComponent<Light>();
        SetLightColor(color);
        if (enabledAtStart || !IsColorSwitchable())
            SetSignifierColor(color);
        else
            SetSignifierColor(ColorCode.White);
        
    }

    public void SetInitialEnabled()
    {
        gameObject.SetActive(enabledAtStart);
        lightBulb.SetColor(color);
        lightBulb.SetActive(enabledAtStart);
    }

    //Editor tool - toggles the light if it is not enabled at start
    public void toggleShowingLightsInEditor(bool showing)
    {
        if(!enabledAtStart)
            GetComponent<Light>().enabled = showing;
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
