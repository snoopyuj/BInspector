/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    public class BInspectorUtility
    {
        public static float GetPropertyExtraFixHeight(SerializedProperty _property, GUIContent _label)
        {
            switch (_property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.Quaternion:
                    return ((EditorGUIUtility.currentViewWidth >= 332.1f) ? 1f : 2f);

                case SerializedPropertyType.Rect:
                    return ((EditorGUIUtility.currentViewWidth >= 332.1f) ? 2f : 3f);

                case SerializedPropertyType.Bounds:
                    return 3f;

                default:
                    return 1f;
            }
        }

        public static string GetPropertyValueToString(ref SerializedProperty _titleNameProp)
        {
            if (_titleNameProp == null)
                return "";

            switch (_titleNameProp.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return _titleNameProp.intValue.ToString();

                case SerializedPropertyType.Boolean:
                    return _titleNameProp.boolValue.ToString();

                case SerializedPropertyType.Float:
                    return _titleNameProp.floatValue.ToString();

                case SerializedPropertyType.String:
                    return _titleNameProp.stringValue;

                case SerializedPropertyType.Color:
                    return _titleNameProp.colorValue.ToString();

                case SerializedPropertyType.ObjectReference:
                    return _titleNameProp.objectReferenceValue.ToString();

                case SerializedPropertyType.Enum:
                    return _titleNameProp.enumNames[_titleNameProp.enumValueIndex];

                case SerializedPropertyType.Vector2:
                    return _titleNameProp.vector2Value.ToString();

                case SerializedPropertyType.Vector3:
                    return _titleNameProp.vector3Value.ToString();

                case SerializedPropertyType.Vector4:
                    return _titleNameProp.vector4Value.ToString();

                case SerializedPropertyType.Vector2Int:
                    return _titleNameProp.vector2IntValue.ToString();

                case SerializedPropertyType.Vector3Int:
                    return _titleNameProp.vector3IntValue.ToString();

                default:
                    break;
            }

            return "";
        }

        public static object GetPropertyValue(SerializedProperty _prop)
        {
            if (_prop == null)
                throw new System.ArgumentNullException("_prop");

            switch (_prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return _prop.intValue;

                case SerializedPropertyType.Boolean:
                    return _prop.boolValue;

                case SerializedPropertyType.Float:
                    return _prop.floatValue;

                case SerializedPropertyType.String:
                    return _prop.stringValue;

                case SerializedPropertyType.Color:
                    return _prop.colorValue;

                case SerializedPropertyType.ObjectReference:
                    return _prop.objectReferenceValue;

                case SerializedPropertyType.LayerMask:
                    return (LayerMask)_prop.intValue;

                case SerializedPropertyType.Enum:
                    return _prop.enumValueIndex;

                case SerializedPropertyType.Vector2:
                    return _prop.vector2Value;

                case SerializedPropertyType.Vector3:
                    return _prop.vector3Value;

                case SerializedPropertyType.Vector4:
                    return _prop.vector4Value;

                case SerializedPropertyType.Vector2Int:
                    return _prop.vector2IntValue;

                case SerializedPropertyType.Vector3Int:
                    return _prop.vector3IntValue;

                case SerializedPropertyType.Rect:
                    return _prop.rectValue;

                case SerializedPropertyType.ArraySize:
                    return _prop.arraySize;

                case SerializedPropertyType.Character:
                    return (char)_prop.intValue;

                case SerializedPropertyType.AnimationCurve:
                    return _prop.animationCurveValue;

                case SerializedPropertyType.Bounds:
                    return _prop.boundsValue;

                case SerializedPropertyType.Gradient:
                    throw new System.InvalidOperationException("Can not handle Gradient types.");
            }

            return null;
        }

        public static void SetPropertyValue(SerializedProperty _prop, object _value)
        {
            if (_prop == null)
                throw new System.ArgumentNullException("prop");

            switch (_prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    _prop.intValue = (int)_value;
                    break;

                case SerializedPropertyType.Boolean:
                    _prop.boolValue = (bool)_value;
                    break;

                case SerializedPropertyType.Float:
                    _prop.floatValue = (float)_value;
                    break;

                case SerializedPropertyType.String:
                    _prop.stringValue = (string)_value;
                    break;

                case SerializedPropertyType.Color:
                    _prop.colorValue = (Color)_value;
                    break;

                case SerializedPropertyType.ObjectReference:
                    _prop.objectReferenceValue = (Object)_value;
                    break;

                case SerializedPropertyType.LayerMask:
                    _prop.intValue = (int)_value;
                    break;

                case SerializedPropertyType.Enum:
                    _prop.enumValueIndex = (int)_value;
                    break;

                case SerializedPropertyType.Vector2:
                    _prop.vector2Value = (Vector2)_value;
                    break;

                case SerializedPropertyType.Vector3:
                    _prop.vector3Value = (Vector3)_value;
                    break;

                case SerializedPropertyType.Vector4:
                    _prop.vector4Value = (Vector4)_value;
                    break;

                case SerializedPropertyType.Vector2Int:
                    _prop.vector2IntValue = (Vector2Int)_value;
                    break;

                case SerializedPropertyType.Vector3Int:
                    _prop.vector3IntValue = (Vector3Int)_value;
                    break;

                case SerializedPropertyType.Rect:
                    _prop.rectValue = (Rect)_value;
                    break;

                case SerializedPropertyType.ArraySize:
                    _prop.arraySize = (int)_value;
                    break;

                case SerializedPropertyType.Character:
                    _prop.intValue = (int)_value;
                    break;

                case SerializedPropertyType.AnimationCurve:
                    _prop.animationCurveValue = (AnimationCurve)_value;
                    break;

                case SerializedPropertyType.Bounds:
                    _prop.boundsValue = (Bounds)_value;
                    break;

                case SerializedPropertyType.Gradient:
                    throw new System.InvalidOperationException("Can not handle Gradient types.");
            }
        }
    }
}