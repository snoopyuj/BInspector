/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Use the class variable name (or enum) as the display name instead of Element 0, 1, 2...
    /// </summary>
    [CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
    public class ArrayElementTitleDrawer : PropertyDrawer
    {
        private StringBuilder tmpIndexSb = new StringBuilder();
        private StringBuilder indexSb = new StringBuilder();

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight(_property, _label, true) + ((_property.isExpanded) ? EditorGUIUtility.singleLineHeight * 0.5f : 0f);
        }

        protected virtual ArrayElementTitleAttribute Attribute
        {
            get { return (ArrayElementTitleAttribute)attribute; }
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            string newLabel = null;

            if (Attribute.UsingTitleType == ArrayElementTitleAttribute.TitleType.Element)
            {
                string fullPathName = _property.propertyPath + "." + Attribute.VarName;
                SerializedProperty titleNameProp = _property.serializedObject.FindProperty(fullPathName);
                newLabel = BInspectorUtility.GetPropertyValueToString(ref titleNameProp);

                if (string.IsNullOrEmpty(newLabel))
                    newLabel = _label.text;
                else
                {
                    indexSb.Length = 0;
                    indexSb.Append("[");
                    indexSb.Append(GetPropertyIndex(_property));
                    indexSb.Append("] ");
                    indexSb.Append(newLabel);

                    newLabel = indexSb.ToString();
                }
            }
            else
            {
                int propIdx = GetPropertyIndex(_property);

                newLabel = Enum.GetName(Attribute.EnumType, propIdx);

                indexSb.Length = 0;
                indexSb.Append("[");
                indexSb.Append(propIdx);
                indexSb.Append("] ");
                indexSb.Append(newLabel);

                newLabel = indexSb.ToString();
            }

            EditorGUI.PropertyField(_position, _property, new GUIContent(newLabel, _label.tooltip), true);
        }

        private int GetPropertyIndex(SerializedProperty _property)
        {
            string path = _property.propertyPath;

            tmpIndexSb.Length = 0;

            for (var i = path.Length - 1; i >= 0; --i)
            {
                if (path[i] == ']')
                    continue;

                if (path[i] == '[')
                    break;

                tmpIndexSb.Insert(0, path[i]);
            }

            int value;
            bool isParse = Int32.TryParse(tmpIndexSb.ToString(), out value);

            return (isParse) ? value : -1;
        }
    }
}