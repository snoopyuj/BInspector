/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Automatically display min max slider
    /// </summary>
    public class TestRangeData : ScriptableObject
    {
        public BInspector.RangeInt rangeInt1 = new BInspector.RangeInt(0, 100);
        public BInspector.RangeInt rangeInt2 = new BInspector.RangeInt(0, 100, 25, 75, false);
        public BInspector.RangeFloat rangeFloat1 = new BInspector.RangeFloat(-10f, 10f, false);
        public BInspector.RangeFloat rangeFloat2 = new BInspector.RangeFloat(-10f, 10f, -5f, 5f);
    }
}