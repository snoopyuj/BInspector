/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Automatically display min max slider
    /// </summary>
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(RangeFloat))]
    public class RangeFloatDrawer : PropertyDrawer
    {
        private RangeFloat targetRangeFloat = null;
        private Vector4 v4;

        private float MinLimit
        {
            get { return v4.x; }
            set { v4.x = value; }
        }

        private float MaxLimit
        {
            get { return v4.y; }
            set { v4.y = value; }
        }

        private float CurMinVal
        {
            get { return v4.z; }
            set { v4.z = value; }
        }

        private float CurMaxVal
        {
            get { return v4.w; }
            set { v4.w = value; }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2f + 5f;
        }

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (targetRangeFloat == null)
                targetRangeFloat = (RangeFloat)fieldInfo.GetValue(_property.serializedObject.targetObject);

            if (targetRangeFloat == null)
                return;

            _position.height = GetPropertyHeight(_property, _label);
            EditorGUI.HelpBox(_position, null, MessageType.None);

            _position.x += 5f;
            _position.width -= 10f;

            _position.height = EditorGUIUtility.singleLineHeight;
            CacheRangeValues();

            EditorGUI.BeginChangeCheck();
            {
                EditorGUI.MinMaxSlider(_label, _position, ref v4.z, ref v4.w, MinLimit, MaxLimit);
            }
            if (EditorGUI.EndChangeCheck())
            {
                SaveValues(_property);
            }

            _position.y += EditorGUIUtility.singleLineHeight;

            ShowValueFields(_position, _property);
        }

        private void CacheRangeValues()
        {
            MinLimit = targetRangeFloat.minLimit;
            MaxLimit = targetRangeFloat.maxLimit;
            CurMinVal = targetRangeFloat.curMinVal;
            CurMaxVal = targetRangeFloat.curMaxVal;
        }

        private void SaveValues(SerializedProperty _property)
        {
            Undo.RecordObject(_property.serializedObject.targetObject, "Change");

            targetRangeFloat.SetValues(MinLimit, MaxLimit, CurMinVal, CurMaxVal, targetRangeFloat.isFixedRange);

            EditorUtility.SetDirty(_property.serializedObject.targetObject);
        }

        private void ShowValueFields(Rect _position, SerializedProperty _property)
        {
            float oriWidth = _position.width;

            _position.width = oriWidth * 0.2f;
            CacheRangeValues();

            EditorGUI.BeginChangeCheck();
            {
                GUI.enabled = !targetRangeFloat.isFixedRange;
                {
                    _position.x += oriWidth * 0.05f;
                    MinLimit = EditorGUI.FloatField(_position, MinLimit);
                }
                GUI.enabled = true;

                _position.x += oriWidth * 0.25f;
                CurMinVal = EditorGUI.FloatField(_position, CurMinVal);

                _position.x += oriWidth * 0.25f;
                CurMaxVal = EditorGUI.FloatField(_position, CurMaxVal);

                GUI.enabled = !targetRangeFloat.isFixedRange;
                {
                    _position.x += oriWidth * 0.25f;
                    MaxLimit = EditorGUI.FloatField(_position, MaxLimit);
                }
                GUI.enabled = true;
            }
            if (EditorGUI.EndChangeCheck())
            {
                CurMinVal = Mathf.Clamp(CurMinVal, MinLimit, MaxLimit);
                CurMaxVal = Mathf.Clamp(CurMaxVal, MinLimit, MaxLimit);

                SaveValues(_property);
            }
        }
    }
}