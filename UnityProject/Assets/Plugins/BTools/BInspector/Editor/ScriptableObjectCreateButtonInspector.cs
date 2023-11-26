/*
 * @author	bwaynesu
 * @date	2017/08/11
 */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace BTools.BInspector
{
    /// <summary>
    /// Displaying a "Create File" button directly on the Inspector of a .cs file.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoImporter))]
    public class ScriptableObjectCreateButtonInspector : Editor
    {
        private static readonly string[] labels = { "Data", "ScriptableObject", string.Empty };

        private MonoScript ms = null;

        private void OnEnable()
        {
            ms = (target as MonoImporter).GetScript();
        }

        public override void OnInspectorGUI()
        {
            var type = ms.GetClass();

            if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsSubclassOf(typeof(Editor)))
            {
                if (GUILayout.Button("Create File"))
                {
                    var asset = ScriptableObject.CreateInstance(type);
                    var path = AssetDatabase.GetAssetPath(Selection.activeObject);

                    if (path == null || path == "")
                        path = "Assets";
                    else if (Path.GetExtension(path) != "")
                        path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

                    Debug.Log("Create " + type.Name + ".asset at: " + path);

                    labels[2] = type.Name;

                    var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + type.Name + ".asset");

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