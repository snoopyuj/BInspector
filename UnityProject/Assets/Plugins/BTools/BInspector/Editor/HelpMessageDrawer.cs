/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Display info/warning/error messages according to the conditions
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(HelpMessageAttribute))]
    public class HelpMessageDrawer : PropertyDrawer
    {
        private GUIContent curCalculateHeightContent = new GUIContent("");
        private Dictionary<string, float> propertyHeightBook = new Dictionary<string, float>();

        protected virtual HelpMessageAttribute Attribute
        {
            get { return (HelpMessageAttribute)attribute; }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            if (propertyHeightBook.ContainsKey(_property.propertyPath))
                return propertyHeightBook[_property.propertyPath];
            else
                return EditorGUI.GetPropertyHeight(_property, _label, true) + GetMsgBoxHeight(_property, curCalculateHeightContent) + 5f;
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.PropertyField(_position, _property, _label, true);

            curCalculateHeightContent.text = string.Empty;

            if (Attribute.commonJudgeType == CommonJudgeType.None)
                CheckMsg(Attribute.methodName, _position, _property, _label);
            else
                CheckMsg(Attribute.commonJudgeType, _position, _property, _label);

            propertyHeightBook[_property.propertyPath] = EditorGUI.GetPropertyHeight(_property, _label, true) + GetMsgBoxHeight(_property, curCalculateHeightContent) + 5f;
        }

        private void CheckMsg(string _methodName, Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (string.IsNullOrEmpty(_methodName))
                return;

            Type type = _property.serializedObject.targetObject.GetType();
            MethodInfo method = type.GetMethod(_methodName);

            if (method == null)
                return;

            HelpMessageContent resultMsgContent = (HelpMessageContent)method.Invoke(_property.serializedObject.targetObject, null);

            if (resultMsgContent == null)
                return;

            ShowMsgBox(resultMsgContent.msg, _position, _property, _label, resultMsgContent.helpType);
        }

        private void CheckMsg(CommonJudgeType _commonJudgeType, Rect _position, SerializedProperty _property, GUIContent _label)
        {
            switch (_commonJudgeType)
            {
                case CommonJudgeType.IntNegative:
                    {
                        if (_property.intValue >= 0)
                            return;

                        ShowMsgBox(_property.displayName + " is negative!", _position, _property, _label, Attribute.commonHelpType);
                        break;
                    }
                case CommonJudgeType.IntZero:
                    {
                        if (_property.intValue != 0)
                            return;

                        ShowMsgBox(_property.displayName + " = 0", _position, _property, _label, Attribute.commonHelpType);
                        break;
                    }
                case CommonJudgeType.FloatNegative:
                    {
                        if (_property.floatValue >= 0)
                            return;

                        ShowMsgBox(_property.displayName + " is negative!", _position, _property, _label, Attribute.commonHelpType);
                        break;
                    }
                case CommonJudgeType.StringNullOrEmpty:
                    {
                        if (!string.IsNullOrEmpty(_property.stringValue))
                            return;

                        ShowMsgBox(_property.displayName + " is null / empty!", _position, _property, _label, Attribute.commonHelpType);
                        break;
                    }
                case CommonJudgeType.ReferenceNull:
                    {
                        if (_property.objectReferenceValue != null)
                            return;

                        ShowMsgBox(_property.displayName + " can't be null!", _position, _property, _label, Attribute.commonHelpType);
                        break;
                    }
                default:
                    return;
            }
        }

        private float GetMsgBoxHeight(SerializedProperty _property, GUIContent _content)
        {
            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.UpperLeft;

            float height = style.CalcSize(_content).y;

            if (string.IsNullOrEmpty(_content.text))
                return 0f;
            else if (height < EditorGUIUtility.singleLineHeight * 3f)
                return 3f * EditorGUIUtility.singleLineHeight;
            else
                return height;
        }

        private void ShowMsgBox(string _text, Rect _position, SerializedProperty _property, GUIContent _label, HelpType _helpType)
        {
            curCalculateHeightContent.text = _text;
            _position.y += EditorGUIUtility.singleLineHeight * BInspectorUtility.GetPropertyExtraFixHeight(_property, _label);
            _position.height = GetMsgBoxHeight(_property, curCalculateHeightContent);

            EditorGUI.HelpBox(_position, _text, (MessageType)_helpType);
            EditorUtility.SetDirty(_property.serializedObject.targetObject);    // for Repaint Inspector
        }
    }
}