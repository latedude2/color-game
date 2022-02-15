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
            wireBuilder.AddWire();
        }

        if(GUILayout.Button("Rotate left"))
        {
            wireBuilder.RotateLeft();
        }
    }
    
}
