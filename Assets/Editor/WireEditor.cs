using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WireBuilder))]
public class WireEditor : Editor
{
    void OnSceneGUI()
    {
        var wire = target as WireBuilder;
        var transform = wire.transform;        

        wire.lineEnd = transform.InverseTransformPoint(
            Handles.PositionHandle(
                transform.TransformPoint(wire.lineEnd), 
                transform.rotation));

        Transform handleTransform = wire.transform;
		Vector3 p0 = handleTransform.TransformPoint(wire.lineStart);
		Vector3 p1 = handleTransform.TransformPoint(wire.lineEnd);

		Handles.color = Color.white;
		Handles.DrawLine(p0, p1);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WireBuilder wireBuilder = (WireBuilder)target;
        if(GUILayout.Button("Add wire"))
        {
            MarkSceneAsDirty();
            wireBuilder.AddWire();
        }

        if(GUILayout.Button("Rotate left"))
        {
            MarkSceneAsDirty();
            wireBuilder.RotateLeft();
        }

        if(GUILayout.Button("Spawn random segment"))
        {
            MarkSceneAsDirty();
            wireBuilder.SpawnRandomSegment();
        }

        if(GUILayout.Button("Autogenerate branches"))
        {
            MarkSceneAsDirty();
            GameObject newWireSystem = new GameObject("New Wire System");
            Undo.RegisterCreatedObjectUndo(newWireSystem, "Created new wire system");
            wireBuilder.iterateGeneration(wireBuilder.treeLength, wireBuilder.branchCount, wireBuilder.randomizeBranchLength, newWireSystem);
        }

        //wireBuilder.GetComponent<WireSurface>().SetLoadedColor();

    }

    //Force unity to save changes or Unity may not save when we have instantiated/removed prefabs despite pressing save button
    private void MarkSceneAsDirty()
    {
        UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
    }
    
}
