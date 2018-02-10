using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class TagAttribute : PropertyAttribute { }

#if UNITY_EDITOR

/// <summary>
/// 游戏架构 - 编辑器工具
/// </summary>
namespace GameFrame.Editor
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                Debug.LogWarning("TagAttribute can only be applied on string properties/fields");
                return;
            }

            property.stringValue = EditorGUI.TagField(position, property.name, property.stringValue);
        }
    }
}

#endif
