using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class ReadOnlyAttribute : PropertyAttribute
{

}

#if UNITY_EDITOR

/// <summary>
/// 游戏架构 - 编辑器工具
/// </summary>
namespace GameFrame.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}

#endif
