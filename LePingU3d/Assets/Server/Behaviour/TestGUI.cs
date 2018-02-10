using GameRanking.Pack;
using System.Collections.Generic;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    public string url = "127.0.0.1:9001";
    public string Uid = "kkk001";
    public string Pwd = "123456";
    public string OpenID = "";
    public string DeviceID = "00-E1-4C-36-F5-C3";
    public int Action = 100;
    public string value = "Hello World!";

    private ActionParam actionParam;

    //todo 启用自定的结构
    private bool useCustomAction = false;

    // Use this for initialization
    private void Start()
    {
        if (useCustomAction)
        {
            //Net.Instance.HeadFormater = new CustomHeadFormater();
            Request1001Pack requestPack = new Request1001Pack() { PageIndex = 1, PageSize = 20 };
            actionParam = new ActionParam(requestPack);
        }
        else
        {
            actionParam = new ActionParam();
            actionParam["PageIndex"] = "1";
            actionParam["PageSize"] = "20";
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnGUI()
    {
        // Any Controls created here will use the default Skin and not the custom Skin

        if (GUILayout.Button("Get ranking for Socket"))
        {
            NetWriter.SetUrl(ApplicationMgr.Instance.SeverHost);
            Net.Instance.Send((int)1001, OnRankingCallback, actionParam);
        }

        if (GUILayout.Button("Hello world!"))
        {
            NetWriter.SetUrl(ApplicationMgr.Instance.SeverHost);
            ActionParam action = new ActionParam();
            action["Content"] = value;
            Net.Instance.Send(100, null, action);
        }
    }

    private void OnRankingCallback(ActionResult actionResult)
    {
        Response1001Pack pack = actionResult.GetValue<Response1001Pack>();
        if (pack == null)
        {
            return;
        }
        DebugMgr.Log(pack.PageCount);
    }
}
