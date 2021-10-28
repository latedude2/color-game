using UnityEngine;
using UnityEditor;
using System;
using CustomExtensions;

/////
// from https://gist.github.com/gatosyocora/9f506612a27a02e28e0be60d09535580
/////

namespace GatoMaterialPropertyDrawer {
    internal class LineDecorator : MaterialPropertyDrawer {
        private float spaceSize;

        public LineDecorator() {
            spaceSize = 0;
        }

        public LineDecorator(float spaceSize) {
            this.spaceSize = spaceSize;
        }

        public override void OnGUI(Rect pos, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            GUILayout.Space(spaceSize);
            GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Space(spaceSize);
        }
    }

    internal class SingleLineTexDrawer : MaterialPropertyDrawer
    {
        public SingleLineTexDrawer() { }

        public override void OnGUI(Rect pos, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            editor.TexturePropertySingleLine(label, prop);
        }
    }

    internal class FoldoutDecorator : MaterialPropertyDrawer {
        public bool isFoldout = false;
        private string groupKeyword;
        private string groupKeywordDisplay;
        private float space = 16;

        public FoldoutDecorator(string groupKeyword) {
            this.groupKeyword = groupKeyword;
            this.groupKeywordDisplay = groupKeyword.AddSpacesToSentence();
        }

        public FoldoutDecorator(string groupKeyword, float isFoldout) {
            this.groupKeyword = groupKeyword;
            this.isFoldout = Convert.ToBoolean(isFoldout);
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor) {
            return EditorGUIUtility.singleLineHeight+space;
        }

        public override void OnGUI(Rect pos, MaterialProperty prop,    GUIContent label, MaterialEditor editor) {
            var material = editor.target as Material;
            float toggleIndent = 47;
            Rect linePos = new Rect(pos.x, pos.y+space/2, pos.width, 1);
            GUI.Box(linePos, string.Empty);
            Rect foldPos = new Rect(pos.x, pos.y+space, pos.width - toggleIndent - pos.height+space, pos.height-space);
            isFoldout = EditorGUI.Foldout(foldPos, isFoldout, groupKeyword.AddSpacesToSentence(), true);
            bool enabled = Convert.ToBoolean(material.GetFloat("_Enabled" + groupKeyword));
            Rect togglePos = new Rect(pos.width-toggleIndent, pos.y+space, pos.height-space, pos.height-space);
            enabled = EditorGUI.Toggle(togglePos, enabled);
            var e = Event.current;
            material.SetFloat("_IsFoldout" + groupKeyword, Convert.ToSingle(isFoldout));
            material.SetFloat("_Enabled" + groupKeyword, Convert.ToSingle(enabled));
        }
    }

    internal class FoldoutGroupDrawer : MaterialPropertyDrawer {
        private string groupKeyword;

        public FoldoutGroupDrawer(string groupKeyword) {
            this.groupKeyword = groupKeyword;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor) {
            var material = editor.target as Material;
            if (!material.HasProperty("_IsFoldout" + groupKeyword)) return 0f;
            var isFoldout = Convert.ToBoolean(material.GetFloat("_IsFoldout" + groupKeyword));
            return -2f;
        }

        public override void OnGUI(Rect pos, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            var material = editor.target as Material;
            if (!material.HasProperty("_IsFoldout" + groupKeyword)) return;
            var isFoldout = Convert.ToBoolean(material.GetFloat("_IsFoldout" + groupKeyword));
            var enabled = Convert.ToBoolean(material.GetFloat("_Enabled" + groupKeyword));

            if (!isFoldout) {
                return;
            }

            if (!enabled) {
                GUI.enabled = false;
            }

            EditorGUI.indentLevel++;
            editor.DefaultShaderProperty(prop, label.text);
            EditorGUI.indentLevel--;
        }
    }

    internal class FoldoutGroupToggleDrawer : MaterialPropertyDrawer {
        private string groupKeyword;
        private string toggleKeyword;

        public FoldoutGroupToggleDrawer(string groupKeyword, string toggleKeyword) {
            this.groupKeyword = groupKeyword;
            this.toggleKeyword = toggleKeyword;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor) {
            var material = editor.target as Material;
            if (!material.HasProperty("_IsFoldout" + groupKeyword)) return 0f;
            var isFoldout = Convert.ToBoolean(material.GetFloat("_IsFoldout" + groupKeyword));
            var enabled = Convert.ToBoolean(material.GetFloat("_Enabled" + groupKeyword));
            if (!isFoldout) {
                return -2f;
            } else {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public override void OnGUI(Rect pos, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            var material = editor.target as Material;
            if (!material.HasProperty("_IsFoldout" + groupKeyword)) return;
            var isFoldout = Convert.ToBoolean(material.GetFloat("_IsFoldout" + groupKeyword));
            var groupEnabled = Convert.ToBoolean(material.GetFloat("_Enabled" + groupKeyword));

            if (!isFoldout) {
                return;
            }

            if (!groupEnabled) {
                GUI.enabled = false;
            }

            float toggleIndent = 47;
            bool toggleEnabled = Convert.ToBoolean(material.GetFloat("_Enabled" + toggleKeyword));
            EditorGUI.indentLevel++;
            EditorGUI.LabelField(pos, toggleKeyword.AddSpacesToSentence());
            EditorGUI.indentLevel--;
            Rect togglePos = new Rect(pos.width-toggleIndent, pos.y, pos.height, pos.height);
            toggleEnabled = EditorGUI.Toggle(togglePos, toggleEnabled);
            material.SetFloat("_Enabled" + toggleKeyword, Convert.ToSingle(toggleEnabled));
        }
    }

    internal class FoldoutGroupTextureDrawer : MaterialPropertyDrawer {
        private string groupKeyword;
        private string textureKeyword;

        public FoldoutGroupTextureDrawer(string groupKeyword, string textureKeyword) {
            this.groupKeyword = groupKeyword;
            this.textureKeyword = textureKeyword;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor) {
            var material = editor.target as Material;
            if (!material.HasProperty("_IsFoldout" + groupKeyword)) return 0f;
            var isFoldout = Convert.ToBoolean(material.GetFloat("_IsFoldout" + groupKeyword));
            var enabled = Convert.ToBoolean(material.GetFloat("_Enabled" + groupKeyword));
            if (!isFoldout) {
                return -2f;
            } else {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public override void OnGUI(Rect pos, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            var material = editor.target as Material;
            if (!material.HasProperty("_IsFoldout" + groupKeyword)) return;
            var isFoldout = Convert.ToBoolean(material.GetFloat("_IsFoldout" + groupKeyword));
            var enabled = Convert.ToBoolean(material.GetFloat("_Enabled" + groupKeyword));

            if (!isFoldout) {
                return;
            }

            if (!enabled) {
                GUI.enabled = false;
            }

            if (prop.type != MaterialProperty.PropType.Texture)
                EditorGUI.LabelField(pos, "Works only for Texture property.");
            EditorGUI.indentLevel++;
            var curVal = editor.TexturePropertyMiniThumbnail(pos, prop, textureKeyword, "Periodic mapping of " + groupKeyword);
            EditorGUI.indentLevel--;
            prop.textureValue = curVal;
        }
    }


    internal class RenderBackfacesToggleDrawer : MaterialPropertyDrawer {

        public RenderBackfacesToggleDrawer() {
        }

        public override void OnGUI(Rect pos, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
            var material = editor.target as Material;
            float toggleIndent = 47;
            bool toggleEnabled = Convert.ToBoolean(material.GetFloat("_RenderBackfaces"));
            EditorGUI.LabelField(pos, "Render Backfaces");
            Rect togglePos = new Rect(pos.width-toggleIndent, pos.y, pos.height, pos.height);
            toggleEnabled = EditorGUI.Toggle(togglePos, toggleEnabled);
            material.SetFloat("_RenderBackfaces", Convert.ToSingle(toggleEnabled));
            if (toggleEnabled) {
                material.SetFloat("_ColorMask", 1);
            } else {
                material.SetFloat("_ColorMask", 0);
            }
        }
    }
}