/*
 * @author	bwaynesu
 * @date	2018/03/15
 */

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Show the variable based on certain conditions.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHideDrawer : PropertyDrawer
    {
        private MethodInfo method = null;
        private PropertyInfo propInfo = null;
        private new FieldInfo fieldInfo = null;

        protected virtual ConditionalHideAttribute Attribute
        {
            get { return (ConditionalHideAttribute)attribute; }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = CheckCondition(_property, Attribute.variableName, Attribute.isInverse);

            if (enabled)
                return EditorGUI.GetPropertyHeight(_property, _label, true);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            bool enabled = CheckCondition(_property, Attribute.variableName, Attribute.isInverse);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;

            if (enabled)
                EditorGUI.PropertyField(_position, _property, _label, true);

            GUI.enabled = wasEnabled;
        }

        private bool CheckCondition(SerializedProperty _property, string _varName, bool _isInverse)
        {
            if (string.IsNullOrEmpty(_varName))
                return (_isInverse) ? false : true;

            if (method == null && propInfo == null && fieldInfo == null)
                CacheVariable(_property, _varName);

            if (method != null)
            {
                bool isConditionPass = (bool)method.Invoke(_property.serializedObject.targetObject, null);

                return (_isInverse) ? (!isConditionPass) : isConditionPass;
            }
            else if (propInfo != null)
            {
                bool isConditionPass = (bool)propInfo.GetValue(_property.serializedObject.targetObject, null);

                return (_isInverse) ? (!isConditionPass) : isConditionPass;
            }
            else if (fieldInfo != null)
            {
                bool isConditionPass = (bool)fieldInfo.GetValue(_property.serializedObject.targetObject);

                return (_isInverse) ? (!isConditionPass) : isConditionPass;
            }

            return (_isInverse) ? false : true;
        }

        private void CacheVariable(SerializedProperty _property, string _varName)
        {
            Type type = _property.serializedObject.targetObject.GetType();

            if (method == null)
                method = type.GetMethod(_varName);

            if (propInfo == null)
                propInfo = type.GetProperty(_varName, typeof(bool));

            if (fieldInfo == null)
                fieldInfo = type.GetField(_varName);
        }
    }
}