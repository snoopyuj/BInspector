/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Use Property (getter, setter) to replace the display variable
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(InsteadShowByPropertyAttribute))]
    public class InsteadShowByPropertyDrawer : PropertyDrawer
    {
        private static class AnimationCurveUtility
        {
            public static void Copy(AnimationCurve _srcCurve, AnimationCurve _targetCurve)
            {
                _targetCurve.keys = _srcCurve.keys;
                _targetCurve.preWrapMode = _srcCurve.preWrapMode;
                _targetCurve.postWrapMode = _srcCurve.postWrapMode;
            }

            public static AnimationCurve CreateCopy(AnimationCurve _srcCurve)
            {
                AnimationCurve newCurve = new AnimationCurve();
                Copy(_srcCurve, newCurve);

                return newCurve;
            }

            public static bool? IsEquals(AnimationCurve _curve1, AnimationCurve _curve2)
            {
                if (_curve1 == null && _curve2 == null)
                    return null;

                if (_curve1 == null || _curve2 == null || _curve1.keys.Length != _curve2.keys.Length)
                    return false;

                if (_curve1.preWrapMode != _curve2.preWrapMode || _curve1.postWrapMode != _curve2.postWrapMode)
                    return false;

                for (var i = 0; i < _curve1.keys.Length; ++i)
                {
                    if (Keyframe.Equals(_curve1.keys[i], _curve2.keys[i]))
                        continue;

                    return false;
                }

                return true;
            }
        }

        private Action<Rect, SerializedProperty, GUIContent>[] differentTypePropActionAry = null;
        private PropertyInfo propInfo = null;
        private UnityEngine.Object[] selectedTargetObjAry = null;
        private GUIContent warningLabelContent = new GUIContent("Mixed!");
        private GUIStyle warningLabelStyle = new GUIStyle(GUI.skin.label);
        private Rect warningTextRect = new Rect(0f, 0f, 0f, 0f);

        public InsteadShowByPropertyDrawer()
        {
            differentTypePropActionAry = new Action<Rect, SerializedProperty, GUIContent>[Enum.GetNames(typeof(SerializedPropertyType)).Length];

            differentTypePropActionAry[(int)SerializedPropertyType.Integer] = ShowIntField;
            differentTypePropActionAry[(int)SerializedPropertyType.Boolean] = ShowBoolField;
            differentTypePropActionAry[(int)SerializedPropertyType.Float] = ShowFloatField;
            differentTypePropActionAry[(int)SerializedPropertyType.String] = ShowStringField;
            differentTypePropActionAry[(int)SerializedPropertyType.Color] = ShowColorField;
            differentTypePropActionAry[(int)SerializedPropertyType.ObjectReference] = ShowObjectField;
            differentTypePropActionAry[(int)SerializedPropertyType.LayerMask] = ShowLayerField;
            differentTypePropActionAry[(int)SerializedPropertyType.Vector2] = ShowVector2Field;
            differentTypePropActionAry[(int)SerializedPropertyType.Vector3] = ShowVector3Field;
            differentTypePropActionAry[(int)SerializedPropertyType.Vector4] = ShowVector4Field;
            differentTypePropActionAry[(int)SerializedPropertyType.Rect] = ShowRectField;
            differentTypePropActionAry[(int)SerializedPropertyType.Character] = ShowCharacterField;
            differentTypePropActionAry[(int)SerializedPropertyType.AnimationCurve] = ShowAnimationCurveField;
            differentTypePropActionAry[(int)SerializedPropertyType.Bounds] = ShowBoundsField;
            differentTypePropActionAry[(int)SerializedPropertyType.Quaternion] = ShowQuaternionField;

            warningLabelStyle.fontStyle = FontStyle.Italic;
            warningLabelStyle.normal.textColor = Color.red;
        }

        protected virtual InsteadShowByPropertyAttribute @Attribute
        {
            get { return (InsteadShowByPropertyAttribute)attribute; }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return base.GetPropertyHeight(_property, _label) * BInspectorUtility.GetPropertyExtraFixHeight(_property, _label);
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (propInfo == null)
            {
                MemberInfo[] memberInfoAry = _property.serializedObject.targetObject.GetType().GetMember(Attribute.propertyName);

                if (memberInfoAry.Length > 0)
                    propInfo = (PropertyInfo)memberInfoAry[0];
            }

            Action<Rect, SerializedProperty, GUIContent> action = differentTypePropActionAry[(int)_property.propertyType];

            if (action == null)
            {
                Debug.LogError("[InsteadShowByProperty] Not support Type");
                return;
            }
            else
            {
                selectedTargetObjAry = _property.serializedObject.targetObjects;
                _label.text = (string.IsNullOrEmpty(Attribute.displayName)) ? Attribute.propertyName : Attribute.displayName;

                action(_position, _property, _label);
            }
        }

        #region Each Type Action

        private void ShowIntField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            int value = (int)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if ((int)(propInfo.GetValue(selectedTargetObjAry[i], null)) == value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.IntField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowBoolField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            bool value = (bool)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if ((bool)(propInfo.GetValue(selectedTargetObjAry[i], null)) == value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.Toggle(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowFloatField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            float value = (float)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if ((float)(propInfo.GetValue(selectedTargetObjAry[i], null)) == value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.FloatField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowStringField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            string value = (string)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (string.Equals((string)(propInfo.GetValue(selectedTargetObjAry[i], null)), value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.TextField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowColorField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Color value = (Color)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if ((Color)(propInfo.GetValue(selectedTargetObjAry[i], null)) == value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.ColorField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowObjectField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            UnityEngine.Object value = (UnityEngine.Object)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if ((UnityEngine.Object)(propInfo.GetValue(selectedTargetObjAry[i], null)) == value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.ObjectField(_position, _label, value, propInfo.PropertyType, true);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowLayerField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            LayerMask layerMask = (LayerMask)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (((LayerMask)(propInfo.GetValue(selectedTargetObjAry[i], null))).value == layerMask.value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                {
                    // draw mixed value warning
                    warningTextRect.Set(_position.x + (EditorGUIUtility.labelWidth - 42f), _position.y, 42f, _position.height);
                    GUI.Label(warningTextRect, warningLabelContent, warningLabelStyle);
                }

                layerMask = EditorGUI.LayerField(_position, _label, layerMask);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], layerMask, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowVector2Field(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Vector2 value = (Vector2)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (Vector2.Equals((Vector2)(propInfo.GetValue(selectedTargetObjAry[i], null)), value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.Vector2Field(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowVector3Field(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Vector3 value = (Vector3)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (Vector3.Equals((Vector3)(propInfo.GetValue(selectedTargetObjAry[i], null)), value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.Vector3Field(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowVector4Field(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Vector4 value = (Vector4)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (Vector4.Equals((Vector4)(propInfo.GetValue(selectedTargetObjAry[i], null)), value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.Vector4Field(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowRectField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Rect value = (Rect)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (Rect.Equals((Rect)(propInfo.GetValue(selectedTargetObjAry[i], null)), value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.RectField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowCharacterField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            var oriValue = (propInfo.GetValue(targetObj, null));
            string value = (oriValue == null) ? string.Empty : oriValue.ToString();
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    var tmpOriValue = (propInfo.GetValue(selectedTargetObjAry[i], null));
                    string tmpValue = (tmpOriValue == null) ? string.Empty : tmpOriValue.ToString();

                    if (string.Equals(tmpValue, value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.TextField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                if (string.IsNullOrEmpty(value))
                {
                    for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                    {
                        propInfo.SetValue(selectedTargetObjAry[i], '\0', null);
                        EditorUtility.SetDirty(selectedTargetObjAry[i]);
                    }
                }
                else
                {
                    for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                    {
                        propInfo.SetValue(selectedTargetObjAry[i], value[0], null);
                        EditorUtility.SetDirty(selectedTargetObjAry[i]);
                    }
                }
            }
        }

        private void ShowAnimationCurveField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            /*視同一物件!*/

            object targetObj = _property.serializedObject.targetObject;
            AnimationCurve value = (AnimationCurve)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            if (value == null)
            {
                value = new AnimationCurve();
                propInfo.SetValue(targetObj, value, null);

                EditorUtility.SetDirty(_property.serializedObject.targetObject);
            }

            EditorGUI.BeginChangeCheck();
            {
                bool? isEqualCurve = null;

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    isEqualCurve = AnimationCurveUtility.IsEquals(value, (AnimationCurve)(propInfo.GetValue(selectedTargetObjAry[i], null)));

                    if (!isEqualCurve.HasValue || (bool)isEqualCurve)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                {
                    // draw mixed value warning
                    warningTextRect.Set(_position.x + (EditorGUIUtility.labelWidth - 42f), _position.y, 42f, _position.height);
                    GUI.Label(warningTextRect, warningLabelContent, warningLabelStyle);
                }

                value = EditorGUI.CurveField(_position, _label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], AnimationCurveUtility.CreateCopy(value), null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowBoundsField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Bounds value = (Bounds)(propInfo.GetValue(targetObj, null));
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if ((Bounds)(propInfo.GetValue(selectedTargetObjAry[i], null)) == value)
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                value = EditorGUI.BoundsField(_position, _label, value);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        private void ShowQuaternionField(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            object targetObj = _property.serializedObject.targetObject;
            Quaternion value = (Quaternion)(propInfo.GetValue(targetObj, null));
            Vector4 v4 = new Vector4(value.x, value.y, value.z, value.w);
            bool hasMultipleDifferentValues = false;

            EditorGUI.BeginChangeCheck();
            {
                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    if (Quaternion.Equals((Quaternion)(propInfo.GetValue(selectedTargetObjAry[i], null)), value))
                        continue;

                    hasMultipleDifferentValues = true;
                    break;
                }

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = true;

                v4 = EditorGUI.Vector4Field(_position, _label, v4);

                if (hasMultipleDifferentValues)
                    EditorGUI.showMixedValue = false;
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTargetObjAry, "Change");
                value.x = v4.x;
                value.y = v4.y;
                value.z = v4.z;
                value.w = v4.w;

                for (var i = 0; i < selectedTargetObjAry.Length; ++i)
                {
                    propInfo.SetValue(selectedTargetObjAry[i], value, null);
                    EditorUtility.SetDirty(selectedTargetObjAry[i]);
                }
            }
        }

        #endregion Each Type Action
    }
}