using UnityEngine;
using System.Collections;
using System;

public class MouseResponseMgr
{

    public static void Register(GameObject go, Action clickCallback, Action doubleCallback)
    {
        if (go == null)
        {
            go = new GameObject();
            go.name = "MouseResponseItem";
        }
        MouseResponseItem mouseResponseItem = go.AddComponent<MouseResponseItem>();
        mouseResponseItem.Init(clickCallback, doubleCallback);
    }
}

public class MouseResponseItem : MonoBehaviour
{
    private bool mouseDownStatus;
    private int mouseDownCount;
    private float lastTime;
    private float currentTime;

    private Action clickCallback;
    private Action doubleCallback;

    public void Init(Action clickCallback, Action doubleCallback)
    {
        this.clickCallback = clickCallback;
        this.doubleCallback = doubleCallback;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!this.mouseDownStatus)
            {
                this.mouseDownStatus = true;
                //Debug.Log("Click !");
                if (this.clickCallback != null) this.clickCallback();

                // 如果按住数量为 0
                if (this.mouseDownCount == 0)
                {
                    // 记录最后时间
                    this.lastTime = Time.realtimeSinceStartup;
                }
                this.mouseDownCount++;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Up !");
            this.mouseDownStatus = false;
        }

        if (this.mouseDownStatus)
        {
            //Debug.Log("Hold !");
            if (this.mouseDownCount >= 2)
            {
                this.currentTime = Time.realtimeSinceStartup;
                if (this.currentTime - this.lastTime < 0.3f)
                {
                    this.lastTime = this.currentTime;
                    this.mouseDownCount = 0;
                    //Debug.Log("Double Click");
                    if (this.doubleCallback != null) this.doubleCallback();
                }
                else
                {
                    // 记录最后时间
                    this.lastTime = Time.realtimeSinceStartup;
                    this.mouseDownCount = 1;
                }
            }
        }
    }
}
