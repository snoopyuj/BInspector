/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Display the enum using a mask (allowing multiple selections).
    /// </summary>
    public class TestEnumMask : MonoBehaviour
    {
        // Masked enum - or bitfield - must follow a bitwise values. (1, 2, 4, 8, 16, 32, 64, etc)
        // Otherwise the bitfield cannot be save properly.
        // Unity has issue with a masked value of an enum having a 0 value.
        // None = 0,
        public enum MyMask
        {
            One = 1,
            Two = 2,
            Three = 4,
            Four = 8,
        }

        [Header("Multiple Selection Enum")]
        [BInspector.EnumMask("Custom Mask Name")]
        public MyMask myMask;

        private void Start()
        {
            foreach (var m in Enum.GetValues(typeof(MyMask)))
                if ((myMask & (MyMask)m) != 0)
                    Debug.Log("Enum Mask: " + m);
        }
    }
}