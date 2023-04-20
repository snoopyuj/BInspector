/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Change variable display name, tooltip, color, and read-only.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(DescriptorAttribute))]
    public class DescriptorDrawer : PropertyDrawer
    {
        private Color oriColor = Color.clear;

        protected virtual DescriptorAttribute Attribute
        {
            get { return (DescriptorAttribute)attribute; }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight(_property, _label, true);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (!string.IsNullOrEmpty(Attribute.name))
                _label.text = Attribute.name;

            if (!string.IsNullOrEmpty(Attribute.tips))
                _label.tooltip = Attribute.tips;

            if (Attribute.color != Color.clear)
            {
                oriColor = GUI.backgroundColor;
                GUI.backgroundColor = Attribute.color;
            }

            GUI.enabled = !Attribute.isReadOnly;
            EditorGUI.PropertyField(_position, _property, _label, true);
            GUI.enabled = true;

            if (Attribute.color != Color.clear)
                GUI.backgroundColor = oriColor;

            // avoid to affect next property
            if (!string.IsNullOrEmpty(Attribute.tips))
                _label.tooltip = null;
        }
    }
}