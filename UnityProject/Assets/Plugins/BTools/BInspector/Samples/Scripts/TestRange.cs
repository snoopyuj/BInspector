/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Automatically show min-max sliders.
    /// </summary>
    public class TestRange : MonoBehaviour
    {
        public BInspector.RangeInt rangeInt1 = new BInspector.RangeInt(0, 100);
        public BInspector.RangeInt rangeInt2 = new BInspector.RangeInt(0, 100, 25, 75, _isFixedRange: false);
        public BInspector.RangeFloat rangeFloat1 = new BInspector.RangeFloat(-10f, 10f, _isFixedRange: false);
        public BInspector.RangeFloat rangeFloat2 = new BInspector.RangeFloat(-10f, 10f, -5f, 5f);
    }
}