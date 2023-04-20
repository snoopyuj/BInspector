/*
 * @author  bwaynesu
 * @date    2018/03/15
 */

using System;
using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Show the variable based on certain conditions.
    /// </summary>
    public class TestConditionalHideData : ScriptableObject
    {
        [Serializable]
        public class TestCustomClass
        {
            public int i = 0;
            public float f = 0f;
            public string s = "string";
        }

        public bool isShow = false;

        [ConditionalHide("isShow")]
        public int byVar = 0;

        [InsteadShowByProperty("IsShowByPropInverse")]
        [Space]
        public bool isShowByPropInverse = false;

        [ConditionalHide("IsShowByPropInverse", _isInverse: true)]
        public string byPropInverse = "Oh! Hi!";

        [ConditionalHide("IsShowByMethod")]
        [Space, Header("[Try: IsShow && IsShowByProp]")]
        public TestCustomClass byMethod = new TestCustomClass();

        public bool IsShowByPropInverse
        {
            get
            {
                return isShowByPropInverse;
            }

            set
            {
                isShowByPropInverse = value;
            }
        }

        public bool IsShowByMethod()
        {
            return isShow && IsShowByPropInverse;
        }
    }
}