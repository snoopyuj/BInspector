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
    public class TestDescriptor : MonoBehaviour
    {
        [System.Serializable]
        public class TestClass
        {
            public int i = 0;
            public bool b = false;
        }

        [BInspector.Descriptor("Custom Class", "I'm Custom Class", _r: 1, _g: 0, _b: 1, _a: 1)]
        public TestClass testClass = new TestClass();

        [BInspector.Descriptor("Int", "I'm tooltip", _r: 0, _g: 1, _b: 1, _a: 1)]
        public int testInt = -1;

        [BInspector.Descriptor("Obj", "Please drag the object here", _r: 1, _g: 1, _b: 0, _a: 1)]
        public GameObject testGo = null;

        [BInspector.Descriptor("Read-Only", null, _isReadOnly: true)]
        public float testFloat = 123f;
    }
}