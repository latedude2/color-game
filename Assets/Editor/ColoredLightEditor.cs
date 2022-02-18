using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColoredLight))]
public class ColoredLightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ColoredLight ColoredLight = (ColoredLight)target;
        ColoredLight.SetColorAtStart();
    }
}
