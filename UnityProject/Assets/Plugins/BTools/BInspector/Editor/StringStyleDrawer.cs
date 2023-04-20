/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Change string display format: file path / folder path / password / Tag
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(StringStyleAttribute))]
    public class StringStyleDrawer : PropertyDrawer
    {
        private const float ButtonSize = 25f;

        private readonly GUIContent buttonContent = new GUIContent("...");

        private Dictionary<string, int> categoryIdxBook = new Dictionary<string, int>();

        protected virtual StringStyleAttribute Attribute
        {
            get { return (StringStyleAttribute)attribute; }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight(_property, _label, true);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            switch (Attribute.stringStyleType)
            {
                case StringStyleType.FilePath:
                    {
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
                            string path = EditorUtility.OpenFilePanel("File Path", "", "");

                            Undo.RecordObject(_property.serializedObject.targetObject, "Change");

                            _property.stringValue = path;
                        }

                        // draw path label
                        _position.x = oriX + EditorGUIUtility.labelWidth;
                        _position.width = oriWidth - EditorGUIUtility.labelWidth;
                        _label.text = _property.stringValue;
                        EditorGUI.PropertyField(_position, _property, GUIContent.none);

                        break;
                    }

                case StringStyleType.FolderPath:
                    {
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
                            string path = EditorUtility.OpenFolderPanel("Folder Path", "", "");

                            Undo.RecordObject(_property.serializedObject.targetObject, "Change");

                            _property.stringValue = path;
                        }

                        // draw path label
                        _position.x = oriX + EditorGUIUtility.labelWidth;
                        _position.width = oriWidth - EditorGUIUtility.labelWidth;
                        _label.text = _property.stringValue;
                        EditorGUI.PropertyField(_position, _property, GUIContent.none);

                        break;
                    }

                case StringStyleType.Password:
                    {
                        string value = _property.stringValue;

                        EditorGUI.BeginChangeCheck();
                        {
                            value = EditorGUI.PasswordField(_position, _label, value);
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(_property.serializedObject.targetObject, "Change");

                            _property.stringValue = value;
                        }
                        break;
                    }

                case StringStyleType.Tag:
                    {
                        _property.stringValue = EditorGUI.TagField(_position, _label, _property.stringValue);
                        break;
                    }

                case StringStyleType.Category:
                    {
                        if (Attribute.categoryAry == null)
                        {
                            Debug.LogError("Empty category for: " + _label.text);
                            break;
                        }

                        string propPath = _property.propertyPath;

                        if (!categoryIdxBook.ContainsKey(propPath))
                        {
                            categoryIdxBook[propPath] = FindCategoryIdx(_property.stringValue);

                            if (categoryIdxBook[propPath] < 0)
                                categoryIdxBook[propPath] = 0;
                        }

                        categoryIdxBook[propPath] = EditorGUI.Popup(_position, _label.text, categoryIdxBook[propPath], Attribute.categoryAry);
                        _property.stringValue = Attribute.categoryAry[categoryIdxBook[propPath]];

                        break;
                    }

                default:
                    {
                        EditorGUI.PropertyField(_position, _property, _label, true);
                        break;
                    }
            }
        }

        private int FindCategoryIdx(string _curCategory)
        {
            string[] categoryAry = Attribute.categoryAry;

            for (var i = 0; i < categoryAry.Length; ++i)
            {
                if (string.Equals(_curCategory, categoryAry[i]))
                    return i;
            }

            return -1;
        }
    }
}