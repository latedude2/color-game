using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Update colors in editor")]
    private static void NewMenuOption()
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
}