/*
 * @author	bwaynesu
 * @date	2017/08/11
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Add a field to return the file location on the Scriptable Object.
    /// </summary>
    public class TestScriptableObjectFileLocation : MonoBehaviour
    {
        [Header("Please see the Inspector of the file below")]
        public TestDescriptorData data = null;
    }
}