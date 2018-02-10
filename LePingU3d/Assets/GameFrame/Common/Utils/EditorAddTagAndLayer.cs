#if UNITY_EDITOR

using UnityEditor;

public static class EditorAddTagAndLayer
{

    public static void AddTag(string tag)
    {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "tags")
                {
                    for (int i = 0; i < it.arraySize; i++)
                    {
                        SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(dataPoint.stringValue))
                        {
                            dataPoint.stringValue = tag;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    public static void AddLayer(string layer)
    {
        if (!isHasLayer(layer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name.StartsWith("User Layer"))
                {
                    if (it.type == "string")
                    {
                        if (string.IsNullOrEmpty(it.stringValue))
                        {
                            it.stringValue = layer;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    private static bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                return true;
        }
        return false;
    }

    private static bool isHasLayer(string layer)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                return true;
        }
        return false;
    }
}

#endif
