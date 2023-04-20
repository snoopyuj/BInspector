/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Display info/warning/error messages based on conditions.
    /// </summary>
    public class TestHelpMessage : MonoBehaviour
    {
        [Header("Try changing the value")]
        [HelpMessage("Show")]
        public int i = -1;

        [HelpMessage(CommonJudgeType.IntNegative)]
        public int i2 = -1;

        [HelpMessage(CommonJudgeType.IntZero, HelpType.Warning)]
        public int i3 = 0;

        [HelpMessage(CommonJudgeType.IntNegative, HelpType.Warning)]
        public int[] iAry = new int[3] { -3, -2, -1 };

        [HelpMessage(CommonJudgeType.FloatNegative, HelpType.Info)]
        public float f = -1f;

        [HelpMessage(CommonJudgeType.StringNullOrEmpty, HelpType.None)]
        public string s = null;

        [HelpMessage(CommonJudgeType.ReferenceNull)]
        public Camera c = null;

        private HelpMessageContent helpMsgContent = new HelpMessageContent();

        public HelpMessageContent Show()
        {
            if (i < 0)
            {
                helpMsgContent.SetMessage(HelpType.Error, i + " is smaller than 0.");
                return helpMsgContent;
            }
            else if (i >= 0 && i < 10)
            {
                helpMsgContent.SetMessage(HelpType.Warning, "i = " + i);
                return helpMsgContent;
            }
            else if (i >= 10 && i <= 20)
            {
                helpMsgContent.SetMessage(HelpType.Info, "i = " + i);
                return helpMsgContent;
            }
            else if (i >= 20 && i <= 50)
            {
                helpMsgContent.SetMessage(HelpType.None, "i = " + i);
                return helpMsgContent;
            }
            else
                return null;
        }
    }
}