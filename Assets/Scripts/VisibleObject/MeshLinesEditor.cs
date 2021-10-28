using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

[CustomEditor(typeof(MeshLines))]
public class MeshLinesEditor : Editor {

    public override void OnInspectorGUI () {
        MeshLines meshLines = (MeshLines)target;

        DrawDefaultInspector();

        if (meshLines.mesh.vertices.Length > 1000) {
            EditorGUILayout.HelpBox("Lines for " + meshLines.mesh.vertices.Length + " vertices might take a long time to calculate. Consider using a simplified mesh.", MessageType.Warning);
        }
        if(GUILayout.Button("Generate Mesh Lines")) {
            meshLines.GenerateMeshLines();
            SceneView.RepaintAll();
            meshLines.refresh();
        }
        meshLines.refresh();
    }
}