/*
 * @author	bwaynesu
 * @date	2017/08/11
 */

using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Add a field to return the file location on the Scriptable Object.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectFileLocationInspector : Editor
    {
        private ScriptableObject targetObj = null;

        private void OnEnable()
        {
            targetObj = target as ScriptableObject;
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("File Location", targetObj, typeof(ScriptableObject), false);
            GUI.enabled = true;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            DrawDefaultInspector();
        }
    }
}