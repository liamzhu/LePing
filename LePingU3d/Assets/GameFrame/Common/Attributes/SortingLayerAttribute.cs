﻿using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditorInternal;
using UnityEditor;

#endif

public class SortingLayerAttribute : PropertyAttribute { }

#if UNITY_EDITOR

/// <summary>
/// 游戏架构 - 编辑器工具
/// </summary>
namespace GameFrame.Editor
{
    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                Debug.LogWarning("SortingLayerAttributes can only be applied on integer properties/fields");
                return;
            }
            EditorGUI.LabelField(position, label);

            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            string[] sortingLayerNames = GetSortingLayerNames();
            int[] sortingLayerIDs = GetSortingLayerIDs();

            int sortingLayerIndex = Mathf.Max(0, System.Array.IndexOf(sortingLayerIDs, property.intValue));
            sortingLayerIndex = EditorGUI.Popup(position, sortingLayerIndex, sortingLayerNames);
            property.intValue = sortingLayerIDs[sortingLayerIndex];
        }

        private string[] GetSortingLayerNames()
        {
            System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        private int[] GetSortingLayerIDs()
        {
            System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
            return (int[])sortingLayersProperty.GetValue(null, new object[0]);
        }

    }
}

#endif
