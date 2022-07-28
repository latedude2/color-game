using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnStars))]
public class StarParticleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnStars spawnStars = (SpawnStars)target;
        EditorGUILayout.LabelField("Use this button after you change particle system component to see changes.");   
        if(GUILayout.Button("Update Stars"))
        {
            MarkSceneAsDirty();
            spawnStars.RegenerateStars();
        }
    }

    //Force unity to save changes or Unity may not save when we have instantiated/removed prefabs despite pressing save button
    private void MarkSceneAsDirty()
    {
        UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
    }
    
}
