/*
 * @author	bwaynesu
 * @date	2017/08/22
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Add a preview button in the drag-and-drop field to preview the object's parameter settings.
    /// </summary>
    public class TestSubView : MonoBehaviour
    {
        [BInspector.SubView]
        public Camera cam = null;

        [BInspector.SubView]
        public Light l = null;
    }
}