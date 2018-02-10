using System.Collections;
using UnityEngine;

public class YouKeTurnPage : MonoBehaviour
{
    ///
    /// 每页宽度(游-客-学-院)
    ///
    public float pageWidth;

    ///
    /// 翻页力度(游.客.学.院)
    ///
    public int EffortsFlip = 50;

    ///
    /// 总页数
    ///
    public int pageNums = 0;

    ///
    /// 当前所在页
    ///
    public int pageIndex
    {
        get
        {
            return mPageIndex;
        }
    }

    ///
    /// 当前所在页
    ///
    private int mPageIndex = 1;

    private UIScrollView mScrollView = null;
    private float nowLocation = 0;
    private bool isDrag = false;
    private bool isSpringMove = false;
    private SpringPanel mSp = null;
    private bool isHorizontal = true;

    private void Awake()
    {
        mScrollView = gameObject.GetComponent<UIScrollView>();
        if (mScrollView == null)
        {
            mScrollView = gameObject.AddComponent<UIScrollView>();
        }
        mScrollView.onDragStarted = OnDragStarted;
        mScrollView.onMomentumMove = onMomentumMove;
        mScrollView.onStoppedMoving = onStoppedMoving;
        if (mScrollView.movement == UIScrollView.Movement.Horizontal)
        {
            isHorizontal = true;
        }
        else
        {
            isHorizontal = false;
        }
        onStoppedMoving();
    }

    private void OnDragStarted()
    {
        isDrag = false;
        SetNowLocation();
    }

    private void onMomentumMove()
    {
        if (isDrag) return;
        Vector3 v3 = transform.localPosition;
        float value = 0;
        if (isHorizontal)
        {
            value = nowLocation - v3.x;
            if (Mathf.Abs(value) < EffortsFlip) return;
            if (value > 0)
            {
                if (mPageIndex < pageNums) Page(-pageWidth);
            }
            else
            {
                if (mPageIndex > 1) Page(pageWidth);
            }
        }
        else
        {
            value = nowLocation - v3.y;
            if (Mathf.Abs(value) < EffortsFlip) return;
            if (value > 0)
            {
                if (mPageIndex > 1) Page(-pageWidth);
            }
            else
            {
                if (mPageIndex < pageNums) Page(pageWidth);
            }
        }
    }

    private void Page(float value)
    {
        isSpringMove = true;
        isDrag = true;
        mSp = GetComponent<SpringPanel>();
        if (mSp == null) mSp = gameObject.AddComponent<SpringPanel>();
        //mSp.enabled = false;
        Vector3 pos = mSp.target;
        pos = isHorizontal ? new Vector3(pos.x + value, pos.y, pos.z) : new Vector3(pos.x, pos.y + value, pos.z);
        if (!SetIndexPage(pos)) return;
        SpringPanel.Begin(gameObject, pos, 13f).strength = 8f;
        mSp.onFinished = SpringPanleMoveEnd;
        Debug.Log("page index=" + mPageIndex);
    }

    private void SpringPanleMoveEnd()
    {
        isSpringMove = false;
    }

    private void onStoppedMoving()
    {
        isDrag = false;
        SetNowLocation();
    }

    private void SetNowLocation()
    {
        if (isHorizontal)
        {
            nowLocation = gameObject.transform.localPosition.x;
        }
        else
        {
            nowLocation = gameObject.transform.localPosition.y;
        }
    }

    private bool SetIndexPage(Vector3 v3)
    {
        float value = isHorizontal ? v3.x : v3.y;
        //Debug.Log((pageNums - 1) * pageWidth);
        if (isHorizontal)
        {
            if (value > 0 || value < (pageNums - 1) * -pageWidth) return false;
        }
        else
        {
            if (value < 0 || value > (pageNums - 1) * pageWidth) return false;
        }
        value = Mathf.Abs(value);
        mPageIndex = (int)(value / pageWidth) + 1;
        return true;
    }

    #region 公共接口 游*客*学*院

    ///
    /// 上一页
    ///
    public void PreviousPage()
    {
        if (isHorizontal)
        {
            if (mPageIndex > 1) Page(pageWidth);
        }
        else
        {
            if (mPageIndex < pageNums) Page(pageWidth);
        }
    }

    ///
    /// 下一页
    ///
    public void NextPage()
    {
        if (isHorizontal)
        {
            if (mPageIndex < pageNums) Page(-pageWidth);
        }
        else
        {
            if (mPageIndex > 1) Page(-pageWidth);
        }
    }

    #endregion 公共接口 游*客*学*院
}
