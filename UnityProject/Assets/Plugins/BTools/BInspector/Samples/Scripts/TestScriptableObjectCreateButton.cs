/*
 * @author	bwaynesu
 * @date	2017/08/11
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Displaying a "Create File" button directly on the Inspector of a .cs file.
    /// </summary>
    public class TestScriptableObjectCreateButton : MonoBehaviour
    {
        [Header("Please watch the Inspector of the file below")]
        public TextAsset csFile = null;
    }
}