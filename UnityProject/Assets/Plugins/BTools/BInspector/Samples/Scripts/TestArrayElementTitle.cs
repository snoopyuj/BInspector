/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Use the class variable name (or enum) as the display name instead of Element 0, 1, 2...
    /// </summary>
    public class TestArrayElementTitle : MonoBehaviour
    {
        public enum TestEnumTitle
        {
            Enum1,
            Enum2,
            Enum3,
            Enum4,
            Enum5,
        }

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

        /// <summary>
        /// Display name will use "i" value
        /// </summary>
        [BInspector.ArrayElementTitle("i")]
        public CustomClass[] customClassAry = new CustomClass[] { new CustomClass(10), new CustomClass(20), new CustomClass(30) };

        /// <summary>
        /// Custom Enum
        /// </summary>
        [Space]
        public TestEnumTitle customEnum = TestEnumTitle.Enum1;

        /// <summary>
        /// Display name will use enum value
        /// </summary>
        [BInspector.ArrayElementTitle(typeof(TestEnumTitle))]
        public CustomClass[] customClassAry2 = new CustomClass[] { new CustomClass(10), new CustomClass(20), new CustomClass(30) };
    }
}