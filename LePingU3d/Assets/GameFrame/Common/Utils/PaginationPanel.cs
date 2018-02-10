using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PaginationPanel : MonoBehaviour
{
    /// <summary>
    /// 当前页面索引
    /// </summary>
    private int m_PageIndex = 1;

    /// <summary>
    /// 总页数
    /// </summary>
    private int m_PageCount = 0;

    /// <summary>
    /// 元素总个数
    /// </summary>
    private int m_ItemsCount = 0;

    /// <summary>
    /// 元素列表
    /// </summary>
    private List<GridItem> m_ItemsList;

    /// <summary>
    /// 上一页
    /// </summary>
    private Button m_BtnPrevious;

    /// <summary>
    /// 下一页
    /// </summary>
    private Button m_BtnNext;

    /// <summary>
    /// 显示当前页数的标签
    /// </summary>
    private Text m_PanelText;



    void Start()
    {
        InitGUI();
        InitItems();
    }

    private void InitGUI()
    {
        m_BtnNext = GameObject.Find("Canvas/Panel/BtnNext").GetComponent<Button>();
        m_BtnPrevious = GameObject.Find("Canvas/Panel/BtnPrevious").GetComponent<Button>();
        m_PanelText = GameObject.Find("Canvas/Panel/Text").GetComponent<Text>();

        m_BtnNext.onClick.AddListener(() => { Next(); });
        m_BtnPrevious.onClick.AddListener(() => { Previous(); });
    }

    private void InitItems()
    {
        GridItem[] items = new GridItem[]
        {
            new GridItem("鼠","Mouse"),
            new GridItem("牛","Ox"),
            new GridItem("虎","Tiger"),
            new GridItem("兔","Rabbit"),
            new GridItem("龙","Dragon"),
            new GridItem("蛇","Snake"),
            new GridItem("马","Horse"),
            new GridItem("羊","Goat"),
            new GridItem("猴","Monkey"),
            new GridItem("鸡","Rooster"),
            new GridItem("狗","Dog"),
            new GridItem("猪","Pig")
        };

        //利用12生肖数组来随机生成列表
        m_ItemsList = new List<GridItem>();
        for (int i = 0; i < Random.Range(1, 1000); i++)
        {
            m_ItemsList.Add(items[Random.Range(0, items.Length)]);
        }

        //计算元素总个数
        m_ItemsCount = m_ItemsList.Count;
        //计算总页数
        m_PageCount = (m_ItemsCount % 12) == 0 ? m_ItemsCount / 12 : (m_ItemsCount / 12) + 1;

        BindPage(m_PageIndex);
        //更新界面页数
        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
    }

    public void Next()
    {
        if (m_PageCount <= 0)
            return;
        //最后一页禁止向后翻页
        if (m_PageIndex >= m_PageCount)
            return;

        m_PageIndex += 1;
        if (m_PageIndex >= m_PageCount)
            m_PageIndex = m_PageCount;

        BindPage(m_PageIndex);

        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
    }

    public void Previous()
    {
        if (m_PageCount <= 0)
            return;
        //第一页时禁止向前翻页
        if (m_PageIndex <= 1)
            return;
        m_PageIndex -= 1;
        if (m_PageIndex < 1)
            m_PageIndex = 1;

        BindPage(m_PageIndex);

        m_PanelText.text = string.Format("{0}/{1}", m_PageIndex.ToString(), m_PageCount.ToString());
    }

    /// <summary>
    /// 绑定指定索引处的页面元素
    /// </summary>
    /// <param name="index">页面索引</param>
    private void BindPage(int index)
    {
        //列表处理
        if (m_ItemsList == null || m_ItemsCount <= 0)
            return;

        //索引处理
        if (index < 0 || index > m_ItemsCount)
            return;

        //按照元素个数可以分为1页和1页以上两种情况
        if (m_PageCount == 1)
        {
            int canDisplay = 0;
            for (int i = 12; i > 0; i--)
            {
                if (canDisplay < 12)
                {
                    BindGridItem(transform.GetChild(canDisplay), m_ItemsList[12 - i]);
                    transform.GetChild(canDisplay).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(canDisplay).gameObject.SetActive(false);
                }
                canDisplay += 1;
            }
        }
        else if (m_PageCount > 1)
        {
            //1页以上需要特别处理的是最后1页
            //和1页时的情况类似判断最后一页剩下的元素数目
            //第1页时显然剩下的为12所以不用处理
            if (index == m_PageCount)
            {
                int canDisplay = 0;
                for (int i = 12; i > 0; i--)
                {
                    //最后一页剩下的元素数目为 m_ItemsCount - 12 * (index-1)
                    if (canDisplay < m_ItemsCount - 12 * (index - 1))
                    {
                        BindGridItem(transform.GetChild(canDisplay), m_ItemsList[12 * index - i]);
                        transform.GetChild(canDisplay).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(canDisplay).gameObject.SetActive(false);
                    }
                    canDisplay += 1;
                }
            }
            else
            {
                for (int i = 12; i > 0; i--)
                {
                    BindGridItem(transform.GetChild(12 - i), m_ItemsList[12 * index - i]);
                    transform.GetChild(12 - i).gameObject.SetActive(true);
                }
            }
        }
    }


    /// <summary>
    /// 加载一个Sprite
    /// </summary>
    /// <param name="assetName">资源名称</param>
    private Sprite LoadSprite(string assetName)
    {
        Texture texture = (Texture)Resources.Load(assetName);

        Sprite sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    /// <summary>
    /// 将一个GridItem实例绑定到指定的Transform上
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="gridItem"></param>
    private void BindGridItem(Transform trans, GridItem gridItem)
    {
        trans.GetComponent<Image>().sprite = LoadSprite(gridItem.ItemSprite);
        trans.Find("Item/Name").GetComponent<Text>().text = gridItem.ItemName;
        trans.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("当前点击的元素名称为:" + gridItem.ItemName);
        });
    }
}

public class GridItem
{
    public GridItem(string sprite, string name)
    {
        ItemSprite = sprite;
        ItemName = name;
    }
    public string ItemSprite;
    public string ItemName;
}