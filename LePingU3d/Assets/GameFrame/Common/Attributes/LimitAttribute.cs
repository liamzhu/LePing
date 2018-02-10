using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class LimitAttribute : PropertyAttribute
{
    public enum Mode { LimitLower, LimitUpper, LimitBoth }

    private readonly Mode _limitMode;

    private readonly int _lowerLimit;
    private readonly int _upperLimit;

    public LimitAttribute(int lowerLimit) : this(Mode.LimitLower, lowerLimit, int.MaxValue)
    {
    }

    public LimitAttribute(int lowerLimit, int upperLimit) : this(Mode.LimitLower, lowerLimit, upperLimit)
    {
    }

    private LimitAttribute(Mode mode, int lowerLimit, int upperLimit)
    {
        _limitMode = mode;
        _lowerLimit = lowerLimit;
        _upperLimit = upperLimit;
    }

    public int Limit(int value)
    {
        switch (_limitMode)
        {
            case Mode.LimitLower:
                return Mathf.Clamp(value, _lowerLimit, int.MaxValue);
            case Mode.LimitUpper:
                return Mathf.Clamp(value, int.MinValue, _upperLimit);
            case Mode.LimitBoth:
                return Mathf.Clamp(value, _lowerLimit, _upperLimit);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

#if UNITY_EDITOR

/// <summary>
/// 游戏架构 - 编辑器工具
/// </summary>
namespace GameFrame.Editor
{
    [CustomPropertyDrawer(typeof(LimitAttribute))]
    public class LimitPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                Debug.LogWarning("LimitAttribute can only be applied on integer properties/fields");
                return;
            }

            LimitAttribute limiter = attribute as LimitAttribute;
            property.intValue = limiter.Limit(EditorGUI.IntField(position, property.name, property.intValue));
        }
    }
}

#endif
