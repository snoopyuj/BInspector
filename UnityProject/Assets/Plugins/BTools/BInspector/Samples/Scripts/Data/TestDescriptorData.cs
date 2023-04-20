/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Change variable display name, tooltip, color, and read-only.
    /// </summary>
    public class TestDescriptorData : ScriptableObject
    {
        [System.Serializable]
        public class TestClass
        {
            public int i = 0;
            public bool b = false;
        }

        [BInspector.Descriptor("Custom Class", "I'm Custom Class", 1, 0, 1, 1)]
        public TestClass testClass = new TestClass();

        [BInspector.Descriptor("Int", "I'm tooltip", 0, 1, 1, 1)]
        public int testInt = -1;

        [BInspector.Descriptor("Obj", "Please drag the object here", 1, 1, 0, 1)]
        public GameObject testGo = null;

        [BInspector.Descriptor("Read-Only", null, true)]
        public float testFloat = 123f;
    }
}