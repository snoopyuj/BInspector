/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Using Property (getter, setter) instead of displaying variables
    /// </summary>
    public class TestInsteadByProperty : MonoBehaviour
    {
        [Header("Try to modify the following values and observe the log")]
        [BInspector.InsteadShowByProperty("IntProperty", "Int Prop")]
        [SerializeField]
        private int i = 0;

        [BInspector.InsteadShowByProperty("BoolProperty", "Bool Prop")]
        [SerializeField]
        private bool b = false;

        [BInspector.InsteadShowByProperty("FloatProperty", "Float Prop")]
        [SerializeField]
        private float f = 0f;

        [BInspector.InsteadShowByProperty("StringProperty", "String Prop")]
        [SerializeField]
        private string s = null;

        [BInspector.InsteadShowByProperty("ColorProperty", "Color Prop")]
        [SerializeField]
        private Color c = Color.clear;

        [BInspector.InsteadShowByProperty("ObjectProperty", "Obj Prop")]
        [SerializeField]
        private Camera cObj = null;

        [BInspector.InsteadShowByProperty("LayerProperty", "Layer Prop")]
        [SerializeField]
        private LayerMask l = 0;

        [BInspector.InsteadShowByProperty("Vector2Property", "Vector2 Prop")]
        [SerializeField]
        private Vector2 v2 = Vector2.zero;

        [BInspector.InsteadShowByProperty("Vector3Property", "Vector3 Prop")]
        [SerializeField]
        private Vector3 v3 = Vector3.zero;

        [BInspector.InsteadShowByProperty("Vector4Property", "Vector4 Prop")]
        [SerializeField]
        private Vector4 v4 = Vector4.zero;

        [BInspector.InsteadShowByProperty("RectProperty", "Rect Prop")]
        [SerializeField]
        private Rect r;

        [BInspector.InsteadShowByProperty("CharProperty", "Char Prop")]
        [SerializeField]
        private char character;

        [BInspector.InsteadShowByProperty("AnimationCurveProperty", "Curve Prop")]
        [SerializeField]
        private AnimationCurve aniCurve = new AnimationCurve();

        [BInspector.InsteadShowByProperty("BoundsProperty", "Bounds Prop")]
        [SerializeField]
        private Bounds bounds;

        [BInspector.InsteadShowByProperty("QuaternionProperty", "Quaternion Prop")]
        [SerializeField]
        private Quaternion q;

        public int IntProperty
        {
            get { return i; }

            set
            {
                Debug.Log("Set i: " + value);
                i = value;
            }
        }

        public bool BoolProperty
        {
            get { return b; }

            set
            {
                Debug.Log("Set b: " + value);
                b = value;
            }
        }

        public float FloatProperty
        {
            get { return f; }

            set
            {
                Debug.Log("Set f: " + value);
                f = value;
            }
        }

        public string StringProperty
        {
            get { return s; }

            set
            {
                Debug.Log("Set s: " + value);
                s = value;
            }
        }

        public Color ColorProperty
        {
            get { return c; }

            set
            {
                Debug.Log("Set c: " + value);
                c = value;
            }
        }

        public Camera ObjectProperty
        {
            get { return cObj; }

            set
            {
                Debug.Log("Set cObj: " + value);
                cObj = value;
            }
        }

        public LayerMask LayerProperty
        {
            get { return l; }

            set
            {
                Debug.Log("Set l: " + value);
                l = value;
            }
        }

        public Vector2 Vector2Property
        {
            get { return v2; }

            set
            {
                Debug.Log("Set v2: " + value);
                v2 = value;
            }
        }

        public Vector3 Vector3Property
        {
            get { return v3; }

            set
            {
                Debug.Log("Set v3: " + value);
                v3 = value;
            }
        }

        public Vector4 Vector4Property
        {
            get { return v4; }

            set
            {
                Debug.Log("Set v4: " + value);
                v4 = value;
            }
        }

        public Rect RectProperty
        {
            get { return r; }

            set
            {
                Debug.Log("Set r: " + value);
                r = value;
            }
        }

        public char CharProperty
        {
            get { return character; }

            set
            {
                Debug.Log("Set character: " + value);
                character = value;
            }
        }

        public AnimationCurve AnimationCurveProperty
        {
            get { return aniCurve; }

            set
            {
                Debug.Log("Set aniCurve: " + value);
                aniCurve = value;
            }
        }

        public Bounds BoundsProperty
        {
            get { return bounds; }

            set
            {
                Debug.Log("Set bounds: " + value);
                bounds = value;
            }
        }

        public Quaternion QuaternionProperty
        {
            get { return q; }

            set
            {
                Debug.Log("Set q: " + value);
                q = value;
            }
        }
    }
}