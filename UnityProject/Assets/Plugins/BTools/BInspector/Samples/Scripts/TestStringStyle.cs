/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using UnityEngine;

namespace BTools.BInspector.Samples
{
    /// <summary>
    /// Change string display format: file path / folder path / password / Tag
    /// </summary>
    public class TestStringStyle : MonoBehaviour
    {
        [Tooltip("unity original attribute")]
        [TextArea(4, int.MaxValue)]
        public string unityTextArea = "This \nis \nText \nArea";

        [Space]
        [StringStyle(StringStyleType.Default)]
        public string defaultString = "Default Style";

        [StringStyle(StringStyleType.FilePath)]
        public string filePath = "D:/test.sln";

        [StringStyle(StringStyleType.FolderPath)]
        public string folderPath = "D:/";

        [StringStyle(StringStyleType.Password)]
        public string password = "I'm Password.";

        [StringStyle(StringStyleType.Tag)]
        public string tagString = "Untagged";

        [StringStyle("Unit/kmh", "Unit/mph", "Speed/Low", "Speed/Mid", "Speed/High")]
        public string categoryString = "Speed/High";
    }
}