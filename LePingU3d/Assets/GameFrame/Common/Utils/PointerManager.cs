using UnityEngine;
using System.Collections;

public class PointerManager : MonoBehaviour
{

    //定义一个鼠标状态的枚举
    public enum PointerState
    {
        stateInvisible,
        stateVisible,
        stateSetInvisible,
        stateSetVisible
    }
    public PointerState pointerState = PointerState.stateSetInvisible;
    public float timeOutReset = 2f;
    private bool hideOverride = false;
    private float timeOutCounter;
    private Vector3 currentMousePosition;
    void Start()
    {
        //获得当前鼠标位置，以便判断是否移动了鼠标(文章出自狗刨学习网)
        currentMousePosition = Input.mousePosition;
    }

    void Update()
    {
        switch (pointerState)
        {
            case PointerState.stateInvisible:
                if (!hideOverride)
                    //移动了鼠标，保持指针可见状态
                    if (Input.mousePosition != currentMousePosition) pointerState = PointerState.stateSetVisible; break;
            case PointerState.stateVisible:
                //开始计时，如果鼠标长时间不动，则隐藏鼠标指针
                if (timeOutCounter > 0f)
                {
                    timeOutCounter -= Time.deltaTime;
                    //Debug.Log(timeOutCounter);
                }
                else pointerState = PointerState.stateSetInvisible;
                break;
            case PointerState.stateSetInvisible:
                //隐藏鼠标指针
                Cursor.visible = false;
                currentMousePosition = Input.mousePosition; //转换状态
                pointerState = PointerState.stateInvisible; break;
            case PointerState.stateSetVisible:
                //指针可见时，重新计时
                timeOutCounter = timeOutReset;
                Cursor.visible = true;
                pointerState = PointerState.stateVisible;
                break;
        }
    }
}
