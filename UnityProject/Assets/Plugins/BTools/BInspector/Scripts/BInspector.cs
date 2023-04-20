/*
 * @author	bwaynesu
 * @date	2017/08/03
 */

using System;
using UnityEngine;

namespace BTools.BInspector
{
    #region Array Element Title

    public class ArrayElementTitleAttribute : PropertyAttribute
    {
        public enum TitleType
        {
            Element,
            Enum,
        }

        private TitleType titleType = TitleType.Element;
        private string varName = null;
        private Type enumType;

        public TitleType UsingTitleType { get { return titleType; } }

        public string VarName { get { return varName; } }

        public Type EnumType { get { return enumType; } }

        public ArrayElementTitleAttribute(string _typeVarName)
        {
            titleType = TitleType.Element;
            varName = _typeVarName;
        }

        public ArrayElementTitleAttribute(Type _enumType)
        {
            titleType = TitleType.Enum;
            enumType = _enumType;
        }
    }

    #endregion Array Element Title

    #region Reorderable

    public class ReorderableAttribute : PropertyAttribute
    {
        public string varName = null;

        public ReorderableAttribute()
        {
        }

        public ReorderableAttribute(string _elementTitleVar)
        {
            varName = _elementTitleVar;
        }
    }

    #endregion Reorderable

    #region Descriptor

    public class DescriptorAttribute : PropertyAttribute
    {
        public string name = null;
        public string tips = null;
        public Color color = Color.clear;
        public bool isReadOnly = false;

        public DescriptorAttribute(string _name, string _tips = null, bool _isReadOnly = false)
        {
            name = _name;
            tips = _tips;
            isReadOnly = _isReadOnly;
        }

        public DescriptorAttribute(string _name, float _r, float _g, float _b, float _a, bool _isReadOnly = false)
        {
            name = _name;
            color = new Color(_r, _g, _b, _a);
            isReadOnly = _isReadOnly;
        }

        public DescriptorAttribute(string _name, string _tips, float _r, float _g, float _b, float _a, bool _isReadOnly = false)
        {
            name = _name;
            tips = _tips;
            color = new Color(_r, _g, _b, _a);
            isReadOnly = _isReadOnly;
        }
    }

    #endregion Descriptor

    #region Enum Mask

    public class EnumMaskAttribute : PropertyAttribute
    {
        public string displayName = null;

        public EnumMaskAttribute(string _displayName = null)
        {
            displayName = _displayName;
        }
    }

    #endregion Enum Mask

    #region Instead Show by Property

    public class InsteadShowByPropertyAttribute : PropertyAttribute
    {
        public string propertyName = null;
        public string displayName = null;

        public InsteadShowByPropertyAttribute(string _propertyName, string _displayName = null)
        {
            propertyName = _propertyName;
            displayName = _displayName;
        }
    }

    #endregion Instead Show by Property

    #region Help Message

    public enum HelpType
    {
        None,
        Info,
        Warning,
        Error
    }

    public enum CommonJudgeType
    {
        None,
        IntNegative,
        IntZero,
        FloatNegative,
        StringNullOrEmpty,
        ReferenceNull,
    }

    public class HelpMessageContent
    {
        public HelpType helpType = HelpType.None;
        public string msg = null;

        public HelpMessageContent()
        {
        }

        public HelpMessageContent(HelpType _helpType, string _msg)
        {
            SetMessage(_helpType, _msg);
        }

        public void SetMessage(HelpType _helpType, string _msg)
        {
            helpType = _helpType;
            msg = _msg;
        }
    }

    public class HelpMessageAttribute : PropertyAttribute
    {
        public string methodName = null;
        public CommonJudgeType commonJudgeType = CommonJudgeType.None;
        public HelpType commonHelpType = HelpType.Error;

        public HelpMessageAttribute(string _customMethodName)
        {
            SetValues(_customMethodName, CommonJudgeType.None, HelpType.Error);
        }

        public HelpMessageAttribute(CommonJudgeType _commonJugeType, HelpType _commonHelpType = HelpType.Error)
        {
            SetValues(null, _commonJugeType, _commonHelpType);
        }

        private void SetValues(string _customMethodName, CommonJudgeType _commonJugeType, HelpType _commonHelpType)
        {
            methodName = _customMethodName;
            commonJudgeType = _commonJugeType;
            commonHelpType = _commonHelpType;
        }
    }

    #endregion Help Message

    #region String Style

    public enum StringStyleType
    {
        Default,
        FilePath,
        FolderPath,
        Password,
        Tag,
        Category,
    }

    public class StringStyleAttribute : PropertyAttribute
    {
        public StringStyleType stringStyleType = StringStyleType.Default;
        public string[] categoryAry = null;

        public StringStyleAttribute(StringStyleType _type)
        {
            stringStyleType = _type;
        }

        public StringStyleAttribute(params string[] _categoryAry)
        {
            stringStyleType = StringStyleType.Category;
            categoryAry = (_categoryAry == null) ? new string[0] : _categoryAry;
        }
    }

    #endregion String Style

    #region Min Max Slider

    [Serializable]
    public class RangeInt
    {
        public int minLimit = 0;
        public int maxLimit = 0;
        public int curMinVal = 0;
        public int curMaxVal = 0;
        public bool isFixedRange = true;

        public RangeInt(int _minLimit, int _maxLimit, bool _isFixedRange = true)
        {
            SetValues(_minLimit, _maxLimit, _minLimit, _maxLimit, _isFixedRange);
        }

        public RangeInt(int _minLimit, int _maxLimit, int _defaultMinVal, int _defaultMaxVal, bool _isFixedRange = true)
        {
            SetValues(_minLimit, _maxLimit, _defaultMinVal, _defaultMaxVal, _isFixedRange);
        }

        public void SetValues(int _minLimit, int _maxLimit, int _defaultMinVal, int _defaultMaxVal, bool _isFixedRange = true)
        {
            minLimit = _minLimit;
            maxLimit = _maxLimit;

            curMinVal = _defaultMinVal;
            curMaxVal = _defaultMaxVal;

            isFixedRange = _isFixedRange;
        }
    }

    [Serializable]
    public class RangeFloat
    {
        public float minLimit = 0f;
        public float maxLimit = 0f;
        public float curMinVal = 0f;
        public float curMaxVal = 0f;
        public bool isFixedRange = true;

        public RangeFloat(float _minLimit, float _maxLimit, bool _isFixedRange = true)
        {
            SetValues(_minLimit, _maxLimit, _minLimit, _maxLimit, _isFixedRange);
        }

        public RangeFloat(float _minLimit, float _maxLimit, float _defaultMinVal, float _defaultMaxVal, bool _isFixedRange = true)
        {
            SetValues(_minLimit, _maxLimit, _defaultMinVal, _defaultMaxVal, _isFixedRange);
        }

        public void SetValues(float _minLimit, float _maxLimit, float _defaultMinVal, float _defaultMaxVal, bool _isFixedRange = true)
        {
            minLimit = _minLimit;
            maxLimit = _maxLimit;

            curMinVal = _defaultMinVal;
            curMaxVal = _defaultMaxVal;

            isFixedRange = _isFixedRange;
        }
    }

    #endregion Min Max Slider

    #region Sub View

    public class SubViewAttribute : PropertyAttribute
    {
        public SubViewAttribute()
        {
        }
    }

    #endregion Sub View

    #region Conditional Hide

    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string variableName = null;
        public bool isInverse = false;

        /// <summary>
        /// Show the variable based on certain conditions.
        /// </summary>
        /// <param name="_variableName"> The name of the Boolean Field or Property or Method to be referenced </param>
        /// <param name="_isInverse"> Whether to invert the triggering condition (False triggers) </param>
        public ConditionalHideAttribute(string _variableName, bool _isInverse = false)
        {
            variableName = _variableName;
            isInverse = _isInverse;
        }
    }

    #endregion Conditional Hide
}