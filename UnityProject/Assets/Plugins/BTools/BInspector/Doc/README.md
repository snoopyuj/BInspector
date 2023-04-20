# BInspector

Unity Interface Optimization: Making the Inspector More User-Friendly.

> _Author: bwaynesu_  
> _Created: August 03, 2017_  
> _Tags: C#, Unity3D_  
> _Support: Unity 2019 or Lower_

## Contents

- [BInspector](#binspector)
  - [Contents](#contents)
  - [Introduction](#introduction)
  - [Array Element Title](#array-element-title)
  - [Conditional Hide](#conditional-hide)
  - [Descriptor](#descriptor)
  - [Enum Mask](#enum-mask)
  - [Help Message](#help-message)
  - [Instead By Property](#instead-by-property)
  - [Range](#range)
  - [Reorderable List](#reorderable-list)
  - [String Style](#string-style)
  - [Scriptable Object Create Button](#scriptable-object-create-button)
  - [Scriptable Object File Location Inspector](#scriptable-object-file-location-inspector)
  - [Sub View](#sub-view)

## Introduction

The purpose of this project is to optimize Unity Inspector and improve game development efficiency. As shown in Fig.1, the current variable does not indicate whether leaving it empty would cause a game error.

<img src="./Pics/Fig.1. Original interface.png" height="40"></img>

Developers can only check it during runtime through code. If we can notify developers of this error while editing data, we can avoid unnecessary debugging time and improve project development efficiency (Fig.2).

<img src="./Pics/Fig.2. Warning message appears when there is a null value.png" height="80"></img>

In addition, this project improves the display format of some variables for easier editing and reading by developers. The project also includes a sample scene for practical exercises.

<p>
<img src="./Pics/Fig.3. Original display method for string.png" height="80"></img>
<img src="./Pics/Fig.4. Improved display method for string according to different patterns.png" height="80"></img>
</p>

## Array Element Title

Use a specific variable in a class as the display name instead of "Element 0", "Element 1", "Element 2", etc.

<p>
<img src="./Pics/Fig.5. Original display method for List.png" height="100"></img>
<img src="./Pics/Fig.6. Use a variable value as the display name.png" height="100"></img>
</p>

```CSharp
// Method 1: Display name will use field value

// Parameters:
//   typeVarName: The name of the variable to be used as the display name.

[BInspector.ArrayElementTitle("i")]
public CustomClass[] customClassAry = new CustomClass[0];

[Serializable]
public class CustomClass
{
  public int i = -1;
  public bool b = false;
  public float f = 0f;
}
```

```CSharp
// Method 2: Display name will use enum value

// Parameters:
//   typeVarName: The Enum to be used as the display name.

[BInspector.ArrayElementTitle(typeof(TestEnumTitle))]
public CustomClass[] customClassAry2 = new CustomClass[0];

public enum TestEnumTitle
{
  Enum1,
  Enum2,
  Enum3,
}
```

## Conditional Hide

Display the variable based on conditions

<p>
<img src="./Pics/Fig.25 Use other variable conditions to display the variable.png" height="40"></img>
<img src="./Pics/Fig.26 Use other variable conditions to display the variable.png" height="70"></img>
</p>

```CSharp
// Parameters:
//   VariableName: the name of the Boolean field, property, or method to be evaluated
//   IsInverse: whether to invert the triggering condition (only trigger if False)

public bool isShow = false;

[BInspector.ConditionalHide("isShow", _isInverse: false)]
public int byVar = 0;
```

## Descriptor

Change the display name, tooltip, color, and read-only status of a variable.

<p>
<img src="./Pics/Fig.7. Original variable display method.png" height="60"></img>
<img src="./Pics/Fig.8. Changeable name, background color, read-only mode.png" height="60"></img>
</p>

```CSharp
// Parameters:
//   name: Display name.
//   tips: Tooltip (optional).
//   (r, g, b, a): Background color (optional).
//   isReadOnly: Whether the variable is read-only (default: false).

[BInspector.Descriptor("Integer", "I'm a tooltip.", 0, 1, 1, 1)]
public int testInt = -1;

[BInspector.Descriptor("GameObject", "Please drag an object here.", 1, 1, 0, 1)]
public GameObject testGo = null;

[BInspector.Descriptor("Read-only", null, true)]
public float testFloat = 123f;
```

## Enum Mask

Display an enum using a mask (multiple selections are possible).

<img src="./Pics/Fig.9. Enum becomes mask display method.png" height="140"></img>

```CSharp
// Parameters:
//   displayName: Display name (optional).

[BInspector.EnumMask("Mask")]
public MyMask myMask;

// The enum values must follow this sequence: 1, 2, 4, 8, 16, 32, 64, etc.
// Otherwise, an error will occur when the value is retrieved.
// Value of 0 do not need to be set.
public enum MyMask
{
  One = 1,
  Two = 2,
  Three = 4,
  Four = 8,
}
```

## Help Message

Display a message (info/warning/error) based on a condition.

<img src="./Pics/Fig.10. Conditions for warning messages based on developer settings.png" height="70"></img>

```CSharp
// Method 1: Use the built-in Common Judge Type to evaluate the condition.

// CommonJudgeType contains frequently used conditions.
// If the condition is met, a message is displayed:
//   IntNegative: The integer is negative.
//   IntZero: The integer is zero.
//   FloatNegative: The float is negative.
//   StringNullOrEmpty: The string is null or empty.
//   ReferenceNull: The object is null.

[BInspector.HelpMessage(CommonJudgeType.ReferenceNull)]
public Camera c = null;
```

<p>
<img src="./Pics/Fig.11. Developer-defined conditions.png" height="70"></img>
<img src="./Pics/Fig.12. Developer-defined conditions.png" height="70"></img>
</p>

```CSharp
// Method 2: Use a custom function to evaluate the condition.

// Parameters:
//   CustomMethodName: The name of the custom function to use.

[BInspector.HelpMessage("Show")]
public int i = -1;

HelpMessageContent helpMsgContent = new HelpMessageContent();
public HelpMessageContent Show()
{
  if (i < 0)
  {
    helpMsgContent.SetMessage(HelpType.Error, i + " is smaller than 0.");
    return helpMsgContent;
  }

  if (i >= 0 && i < 10)
  {
    helpMsgContent.SetMessage(HelpType.Warning, "i = " + i);
    return helpMsgContent;
  }

  return null;
}
```

## Instead By Property

This feature allows using a Property (getter, setter) to display a variable in the Inspector, instead of displaying the variable directly.

<p>
<img src="./Pics/Fig.13. Original variable b changed to display BoolProperty.png" height=""></img>
<img src="./Pics/Fig.14. When editing variables, execute Property directly.png" height=""></img>
</p>

```CSharp
// Note: this feature must be used with the SerializeField attribute.

// Parameters:
//   propertyName: The name of the Property to display.
//   displayName: The display name (if not provided, the original Property name will be used).

[BInspector.InsteadShowByProperty("BoolProperty", "Bool Prop"), SerializeField]
bool b = false;

public bool BoolProperty
{
  get { return b; }
  set
  {
    Debug.Log("Set b: " + value);
    b = value;
  }
}
```

## Range

Display Min Max Slider

<p>
<img src="./Pics/Fig.15. Left and right extreme values cannot be edited.png" height="40"></img>
<img src="./Pics/Fig.16. Editable left and right extreme values.png" height="40"></img>
</p>

```CSharp
// Divided into RangeInt and RangeFloat

// Parameters:
//   minLimit: the minimum value
//   maxLimit: the maximum value
//   defaultMinVal: the default smaller value
//   defaultMaxVal: the default larger value
//   isFixedRange: whether to limit the modification of the minimum and maximum values (default is True)

public RangeInt rangeInt1 = new RangeInt(0, 100);
public RangeInt rangeInt2 = new RangeInt(0, 100, 25, 75, false);
public RangeFloat rangeFloat1 = new RangeFloat(-10f, 10f, false);
public RangeFloat rangeFloat2 = new RangeFloat(-10f, 10f, -5f, 5f);
```

## Reorderable List

Adjustable order list

<p>
<img src="./Pics/Fig.17. List original display method.png" height="120"></img>
<img src="./Pics/Fig.18. List interface that can change order and display name.png" height="150"></img>
</p>

```CSharp
// Parameters:
//   elementTitleVar: the name of the variable to be displayed as the title

/// <summary>
/// Display name will use "i" value
/// </summary>
[BInspector.Reorderable("i")]
public CustomClass[] customClassAry = new CustomClass[0];

[Serializable]
public class CustomClass
{
  public int i = -1;
  public bool b = false;
  public float f = 0f;
}
```

## String Style

Change string display style

<p>
<img src="./Pics/Fig.19. String original display method.png" height=""></img>
<img src="./Pics/Fig.20. String display method improved according to different modes.png" height=""></img>
</p>

```CSharp
// The String Style Type determines the display mode of the string:
//   Default: Default style. No difference from the original interface.
//   FilePath: File search mode. Will open the file search window.
//   FolderPath: Folder search mode. Will open the folder search window.
//   Password: Cipher mode.
//   Tag and Category: Tag and custom drop-down menu mode.

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

[StringStyle("Speed/Low", "Speed/High")]
public string categoryString = "Speed/High";
```

## Scriptable Object Create Button

Create the ScriptableObject directly on the Inspector of the code file, without the need to write an additional function to add the ScriptableObject.

<img src="./Pics/Fig.21. No need to write additional functions to add ScriptableObject.png" height=""></img>

## Scriptable Object File Location Inspector

Display the file location on the ScriptableObject for quick access to the file path.

<img src="./Pics/Fig.22 Display file location and click to return to that path.png" height=""></img>

## Sub View

Add a preview button to the drag-and-drop field to preview the object parameter settings.

<p>
<img src="./Pics/Fig.23 Add preview button.png" height=""></img>
<img src="./Pics/Fig.24 Open preview window.png" height="200"></img>
</p>
