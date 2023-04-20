/*
 * @author	bwaynesu
 * @date	2017/08/22
 */

using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Add a preview button in the drag-and-drop field to preview the object's parameter settings.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(SubViewAttribute))]
    public class SubViewDrawer : PropertyDrawer
    {
        private const float ButtonSize = 40f;

        private GUIContent buttonContent = new GUIContent("view");
        private SubViewWindow subViewWindow = null;

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight(_property, _label, true);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (_property.propertyType != SerializedPropertyType.ObjectReference || _property.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(_position, _property, _label, true);
                return;
            }

            float oriWidth = _position.width;
            float oriX = _position.x;

            // draw label
            _position.width = EditorGUIUtility.labelWidth - ButtonSize;
            EditorGUI.LabelField(_position, _label);

            // draw button
            _position.x += _position.width;
            _position.width = ButtonSize;
            if (GUI.Button(_position, buttonContent))
            {
                if (subViewWindow == null)
                    subViewWindow = ScriptableObject.CreateInstance<SubViewWindow>();

                subViewWindow.ShowWindow(_property.objectReferenceValue);
            }

            // draw obj label
            _position.x = oriX + EditorGUIUtility.labelWidth;
            _position.width = oriWidth - EditorGUIUtility.labelWidth;
            EditorGUI.PropertyField(_position, _property, GUIContent.none);
        }
    }

    public class SubViewWindow : EditorWindow
    {
        private const string TitleName = "Sub View";

        private Object Obj = null;
        private Editor CachedEditor = null;
        private Vector2 scrollPos;

        public void ShowWindow(Object _obj)
        {
            Obj = _obj;
            CachedEditor = Editor.CreateEditor(Obj);

            GetWindow<SubViewWindow>();
            titleContent.text = TitleName;
            Focus();
        }

        private void OnGUI()
        {
            if (CachedEditor == null || Obj == null)
                return;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(Obj.name, EditorStyles.boldLabel);

                    GUI.enabled = false;
                    EditorGUILayout.ObjectField(Obj, typeof(object), true);
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    CachedEditor.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}