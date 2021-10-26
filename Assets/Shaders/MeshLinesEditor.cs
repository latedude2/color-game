using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

[CustomEditor(typeof(MeshLines))]
public class MeshLinesEditor : Editor {
//     SerializedProperty myVar;
//     SerializedProperty armorProp;
//     SerializedProperty gunProp;
//     void OnEnable()
//     {
//         // Setup the SerializedProperties.
//         myVar = serializedObject.FindProperty (nameof(myVar));
//         // armorProp = serializedObject.FindProperty ("armor");
//         // gunProp = serializedObject.FindProperty ("gun");
//     }
    bool updateRealTime = false;
    public override void OnInspectorGUI () {
        MeshLines meshLines = (MeshLines)target;
//         //Called whenever the inspector is drawn for this object.
        DrawDefaultInspector();
        //This draws the default screen.  You don't need this if you want
        //to start from scratch, but I use this when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.
        // meshLines.myVar = EditorGUILayout.FloatField ("Max Health", meshLines.myVar);
        // for (int i = 0; i < meshLines.lines.Count; i++) {
            
        //     // meshLines.lines[i].a = EditorGUILayout.Vector3Field("Point", meshLines.lines[i].a);
        // }
        // List<MeshLines.Line> lines = meshLines.lines;
        // for (int i = 0; i < lines.Count; i++) {
            
        //     // lines[i].a = EditorGUILayout.Vector3Field("Point", lines[i].a);
        // }
        // foreach (var line in meshLines.lines) {
        // }
        // updateRealTime = GUILayout.Toggle(updateRealTime, "Update Real Time");
        // meshLines.updateRealTime = updateRealTime;
        // updateRealTime = meshLines.updateRealTime;

        if(GUILayout.Button("Generate Mesh Lines")) {
            // Debug.Log(myVar.floatValue);
            // myVar.floatValue += 1;
            // meshLines.myVar += 1;
            meshLines.lines = meshLines.generateMeshLines();
            SceneView.RepaintAll();
            meshLines.refresh();
        }
        // serializedObject.Update();
        meshLines.refresh();
    }
}

// [CustomPropertyDrawer(typeof(MeshLines.Line))]
// public class LinePropertyDrawer : PropertyDrawer {
//     private const int fieldWidth = 24;
//     private const int fieldHeight = 18;
//     private const uint spacing = 2;
//     public override void OnGUI (Rect pos, SerializedProperty property, GUIContent label) {
//         // EditorGUI.PropertyField(position, property, label, false);
//         // EditorGUI.LabelField(position, label.text);
//         // Rect indentedRect = EditorGUI.IndentedRect (position);
//         // EditorGUI.LabelField(indentedRect, label.text);
//         // EditorGUI.PrefixLabel(position, label);
//         GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(10));
//         // var ass = EditorGUI.Vector3Field(pos, "Point A", property.vector3Value);
//         Vector3 a = (Vector3)property.FindPropertyRelative ("a").vector3Value;
//         Vector3 b = (Vector3)property.FindPropertyRelative ("b").vector3Value;
//         Rect newPos = new Rect(pos.x + pos.width - fieldWidth, pos.y, fieldWidth, fieldHeight);
//         a.x = EditorGUI.FloatField(newPos, a.x);
//         newPos.x -= fieldWidth + spacing;
//         a.y = EditorGUI.FloatField(newPos, a.y);
//         newPos.x -= fieldWidth + spacing;
//         a.z = EditorGUI.FloatField(newPos, a.z);
//         newPos.y += fieldHeight + spacing;
//         b.z = EditorGUI.FloatField(newPos, b.z);
//         newPos.x += fieldWidth + spacing;
//         b.y = EditorGUI.FloatField(newPos, b.y);
//         newPos.x += fieldWidth + spacing;
//         b.x = EditorGUI.FloatField(newPos, b.x);
        
//         // float fieldHeight = base.GetPropertyHeight (property, label) + 2;
//         // property.
//         // var baa = EditorGUI.Vector3Field(pos, "Point B", property.vector3Value);
//         // EditorGUI.Vector3Field();
//         // 1
//         // if (property.isExpanded) {
//         // // 2
//         // // 3
//         //     if (enemyPrefab != null) {
//         //         SpriteRenderer enemySprite = enemyPrefab.GetComponentInChildren<SpriteRenderer> (); 
//         //         // 4
//         //         int previousIndentLevel = EditorGUI.indentLevel;
//         //         // 5
//         //         float fieldHeight = base.GetPropertyHeight (property, label) + 2;
//         //         Vector3 enemySize = enemySprite.bounds.size;
//         //         Rect texturePosition = new Rect (indentedRect.x, indentedRect.y + fieldHeight * 4, enemySize.x / enemySize.y * spriteHeight, spriteHeight);
//         //         // 6
//         //         EditorGUI.DropShadowLabel(texturePosition, new GUIContent(enemySprite.sprite.texture));
//         //         // 7
//         //         EditorGUI.indentLevel = previousIndentLevel;
//         //     }
//         // }
//         // EditorGUI.EndProperty();
//     }
//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
//         return EditorGUI.GetPropertyHeight(property)*2;
//     }
// }