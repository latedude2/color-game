using UnityEngine;
using UnityEditor;

public class MenuItems
{
    private const string SettingPrefKey = "ToggleShowingDisabledLights";
    private static bool showDisabledLights {
        get => EditorPrefs.GetBool(SettingPrefKey);
        set => EditorPrefs.SetBool(SettingPrefKey, value);
    }

    [MenuItem("Enlit Games/Update colors in editor")]
    private static void UpdateColors()
    {
        WireSurface[] wires = GameObject.FindObjectsOfType<WireSurface>();
        foreach(WireSurface comp in wires)
        {
            comp.SetLoadedColor();
        }

        ColoredLight[] lights = GameObject.FindObjectsOfType<ColoredLight>();
        foreach(ColoredLight comp in lights)
        {
            comp.SetColorAtStart();
        }
        UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
    }
    [MenuItem("Enlit Games/Show disabled light cones")]
    private static void ToggleShowingDisabledLights()
    {
        showDisabledLights = !showDisabledLights;
        ColoredLight[] lights = GameObject.FindObjectsOfType<ColoredLight>();
        foreach(ColoredLight comp in lights)
        {
            comp.toggleShowingLightsInEditor(showDisabledLights);
        }
        UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
    }

    [MenuItem("Enlit Games/Show disabled light cones", true)]
    private static bool SettingValidate() {
        Menu.SetChecked("Enlit Games/Show disabled light cones", showDisabledLights);
        return true;
    }
}