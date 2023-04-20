/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Reorderable list
    /// </summary>
    public class TestReorderableList : MonoBehaviour
    {
        [Serializable]
        public class CustomClass
        {
            public int i = -1;
            public bool b = false;
            public float f = 0f;

            public CustomClass(int _i)
            {
                i = _i;
            }
        }

        [BInspector.Reorderable]
        public int[] normalIntAry = new int[] { 1, 2, 3 };

        /// <summary>
        /// Display name will use "i" value
        /// </summary>
        [BInspector.Reorderable("i")]
        public CustomClass[] customClassAry = new CustomClass[] { new CustomClass(10), new CustomClass(20), new CustomClass(30) };
    }
}