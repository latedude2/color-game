using UnityEngine;
using UnityEditor;

public class MenuItems
{
    static bool showDisabledLights = true;
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
    [MenuItem("Enlit Games/Toggle showing disabled lights")]
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
}