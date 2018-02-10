using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class LayerAttribute : PropertyAttribute { }

#if UNITY_EDITOR

/// <summary>
/// 游戏架构 - 编辑器工具
/// </summary>
namespace GameFrame.Editor
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                Debug.LogWarning("LayerAttribute can only be applied on integer properties/fields");
                return;
            }

            property.intValue = EditorGUI.LayerField(position, property.name, property.intValue);
        }
    }
}

#endif
