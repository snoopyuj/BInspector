/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Reorderable list
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(ReorderableAttribute))]
    public class ReorderableDrawer : PropertyDrawer
    {
        private readonly string[] SplitArrayKeyWords = new string[] { "." };

        private ReorderableListExtend reorderList = null;
        private string propPath = null;
        private string subPath = null;

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            propPath = _property.propertyPath;
            subPath = propPath.Substring(propPath.Length - 3);

            if (reorderList != null && string.Equals(subPath, "[0]"))
                return reorderList.GetHeight();
            else
                return -2f;
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            propPath = _property.propertyPath;
            subPath = propPath.Substring(propPath.Length - 3);

            if (!string.Equals(subPath, "[0]"))
                return;

            if (reorderList == null)
            {
                string[] paramNames = propPath.Split(SplitArrayKeyWords, 2, System.StringSplitOptions.None);

                reorderList = new ReorderableListExtend(_property.serializedObject, paramNames[0], ((ReorderableAttribute)attribute).varName);
            }

            reorderList.DoList(_position);
        }
    }
}