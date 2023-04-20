/* http://www.clonefactor.com/wordpress/program/c/1497/ */

/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BTools.BInspector
{
    public class ReorderableListExtend
    {
        #region variable

        private readonly Rect FirstElementBGRectShift = new Rect(0f, -2f, 0f, 3f);
        private readonly Rect ElementBGRectShift = new Rect(0f, 0f, 0f, 1f);
        private readonly Rect LastElementBGRectShift = new Rect(0f, 0f, 0f, 5f);

        private string propertyName = null;
        private SerializedObject serializedObject = null;
        private SerializedProperty property = null;
        private List<float> elementHeights = null;
        private ReorderableList orderList = null;
        private string elementTitleName = null;
        private Rect curElementBGRec;

        #endregion variable

        #region System

        public ReorderableListExtend(SerializedObject _serializedObject, string _propertyName, string _elementTitleName = null,
            bool _dragable = true, bool _displayHeader = true, bool _displayAddButton = true, bool _displayRemoveButton = true)
        {
            propertyName = _propertyName;
            serializedObject = _serializedObject;
            property = _serializedObject.FindProperty(propertyName);
            elementHeights = new List<float>(property.arraySize);
            elementTitleName = _elementTitleName;

            orderList = new ReorderableList(serializedObject: _serializedObject, elements: property,
                draggable: _dragable, displayHeader: _displayHeader, displayAddButton: _displayAddButton, displayRemoveButton: _displayRemoveButton);

            orderList.onAddCallback += OnAdd;
            orderList.onRemoveCallback += OnRemove;
            orderList.drawElementCallback += OnDrawElement;
            orderList.elementHeightCallback += OnCalculateItemHeight;
            orderList.drawElementBackgroundCallback += OnDrawElementBackground;

            orderList.headerHeight = 0f;
        }

        ~ReorderableListExtend()
        {
            orderList.onAddCallback -= OnAdd;
            orderList.onRemoveCallback -= OnRemove;
            orderList.drawElementCallback -= OnDrawElement;
            orderList.elementHeightCallback -= OnCalculateItemHeight;
            orderList.drawElementBackgroundCallback -= OnDrawElementBackground;
        }

        #endregion System

        #region API

        public void DoLayoutList()
        {
            orderList.DoLayoutList();
        }

        public void DoList(Rect _rect)
        {
            orderList.DoList(_rect);
        }

        public float GetHeight()
        {
            return orderList.GetHeight();
        }

        #endregion API

        #region listener

        private void OnAdd(ReorderableList _list)
        {
            if (_list.index < 0 || _list.index >= _list.count)
                _list.index = _list.count - 1;

            property.InsertArrayElementAtIndex(_list.index);
            ++_list.index;
        }

        private void OnRemove(ReorderableList _list)
        {
            property.DeleteArrayElementAtIndex(_list.index);
        }

        private void OnDrawElement(Rect _rect, int _index, bool _active, bool _focused)
        {
            if (IsPropFocus(_rect, _index) && orderList.index != _index)
                orderList.index = _index;

            SerializedProperty prop = property.GetArrayElementAtIndex(_index);

            if (prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                _rect.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.ObjectField(_rect, prop);
            }
            else
            {
                _rect.height = EditorGUI.GetPropertyHeight(prop);

                if (string.IsNullOrEmpty(elementTitleName) || !prop.hasVisibleChildren)
                    EditorGUI.PropertyField(_rect, prop, includeChildren: prop.hasVisibleChildren);
                else
                {
                    string FullPathName = prop.propertyPath + "." + elementTitleName;
                    SerializedProperty titleNameProp = prop.serializedObject.FindProperty(FullPathName);
                    string newlabel = "[" + _index + "] " + BInspectorUtility.GetPropertyValueToString(ref titleNameProp);

                    EditorGUI.PropertyField(_rect, prop, new GUIContent(newlabel), includeChildren: true);
                }
            }
        }

        private float OnCalculateItemHeight(int _index)
        {
            SerializedProperty prop = property.GetArrayElementAtIndex(_index);

            if (prop.propertyType == SerializedPropertyType.ObjectReference)
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
        }

        private void OnDrawElementBackground(Rect _rect, int _index, bool _isActive, bool _isFocused)
        {
            SerializedProperty prop = property.GetArrayElementAtIndex(_index);

            Color oriBG = GUI.backgroundColor;
            GUI.backgroundColor = (orderList.index == _index) ? Color.cyan : Color.gray;

            _rect.height = (prop.propertyType == SerializedPropertyType.ObjectReference) ? EditorGUIUtility.singleLineHeight : EditorGUI.GetPropertyHeight(prop);

            if (_index == 0)
                curElementBGRec = new Rect(_rect.x + FirstElementBGRectShift.x, _rect.y + FirstElementBGRectShift.y, _rect.width + FirstElementBGRectShift.width, _rect.height + FirstElementBGRectShift.height);
            else if (_index == orderList.count - 1)
                curElementBGRec = new Rect(_rect.x + LastElementBGRectShift.x, _rect.y + LastElementBGRectShift.y, _rect.width + LastElementBGRectShift.width, _rect.height + LastElementBGRectShift.height);
            else
                curElementBGRec = new Rect(_rect.x + ElementBGRectShift.x, _rect.y + ElementBGRectShift.y, _rect.width + ElementBGRectShift.width, _rect.height + ElementBGRectShift.height);

            EditorGUI.HelpBox(curElementBGRec, null, MessageType.None);

            GUI.backgroundColor = oriBG;
        }

        #endregion listener

        #region private method

        private bool IsPropFocus(Rect _rect, int _index)
        {
            _rect.height = OnCalculateItemHeight(_index);

            return Event.current.type == EventType.MouseDown && _rect.Contains(Event.current.mousePosition);
        }

        #endregion private method
    }
}