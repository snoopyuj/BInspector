/*
 * @author	bwaynesu
 * @date	2017/08/11
 */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Displaying a "Create File" button directly on the Inspector of a .cs file.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoScript))]
    public class ScriptableObjectCreateButtonInspector : Editor
    {
        private static readonly string[] labels = { "Data", "ScriptableObject", string.Empty };

        private MonoScript ms = null;

        private void OnEnable()
        {
            ms = target as MonoScript;
        }

        public override void OnInspectorGUI()
        {
            Type type = ms.GetClass();

            if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsSubclassOf(typeof(Editor)))
            {
                if (GUILayout.Button("Create File"))
                {
                    ScriptableObject asset = ScriptableObject.CreateInstance(type);

                    string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                    if (path == null || path == "")
                        path = "Assets";
                    else if (Path.GetExtension(path) != "")
                        path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

                    Debug.Log("Create " + type.Name + ".asset at: " + path);

                    labels[2] = type.Name;

                    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + type.Name + ".asset");
                    AssetDatabase.CreateAsset(asset, assetPathAndName);
                    AssetDatabase.SetLabels(asset, labels);
                    AssetDatabase.SaveAssets();

                    EditorGUIUtility.PingObject(asset);
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            DrawDefaultInspector();
        }
    }
}