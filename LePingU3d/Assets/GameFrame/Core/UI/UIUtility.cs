using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIUtility
{
    public static int MaxPanelDepth()
    {
        NGUITools.NormalizePanelDepths();

        int depth = 0;
        UIPanel[] list = NGUITools.FindActive<UIPanel>();
        if (list.Length > 0)
        {
            Array.Sort(list, UIPanel.CompareFunc);
            depth = list[list.Length - 1].depth;
        }
        return depth;
    }

    public static int GetTargetMaxDepth(GameObject obj, bool includeInactive = false)
    {
        int maxDepth = 0;
        List<UIPanel> lsPanels = GetPanelSorted(obj, includeInactive);
        if (lsPanels != null)
            return lsPanels[lsPanels.Count - 1].depth;
        return maxDepth;
    }

    public static int GetTargetMinDepth(GameObject obj, bool includeInactive = false)
    {
        int minDepth = 0;
        List<UIPanel> lsPanels = GetPanelSorted(obj, includeInactive);
        if (lsPanels != null)
            return lsPanels[0].depth;
        return minDepth;
    }

    private class CompareSubPanels : IComparer<UIPanel>
    {
        public int Compare(UIPanel left, UIPanel right)
        {
            return left.depth - right.depth;
        }
    }

    private static List<UIPanel> GetPanelSorted(GameObject target, bool includeInactive = false)
    {
        UIPanel[] panels = target.transform.GetComponentsInChildren<UIPanel>(includeInactive);
        if (panels != null && panels.Length > 0)
        {
            List<UIPanel> lsPanels = new List<UIPanel>(panels);
            lsPanels.Sort(new CompareSubPanels());
            return lsPanels;
        }
        return null;
    }

    public static void SetTargetMinPanelDepth(GameObject obj, int depth)
    {
        List<UIPanel> lsPanels = GetPanelSorted(obj, true);
        if (lsPanels != null)
        {
            int i = 0;
            while (i < lsPanels.Count)
            {
                lsPanels[i].depth = depth + i;
                i++;
            }
        }
    }

    public static void ChangeChildLayer(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        for (int i = 0; i < trans.childCount; ++i)
        {
            Transform child = trans.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }
}
