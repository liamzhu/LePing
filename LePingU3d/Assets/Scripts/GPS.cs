using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  
using LitJson;
  
public class GPS : MonoBehaviour  
{
    private const float TimeLimit = 600;  

    public event System.Action<string> OnLocate;
    public event System.Action OnError;

    public static GPS _instence = null;

    private string lastResult; //最终结果
    private float lastRequestTime; //最终请求时间
    private bool isRequesting; //正在请求

    private void Awake()
    {
        lastResult = string.Empty;
        lastRequestTime = 0;
        isRequesting = false;

        _instence = this;
    }

    public void GetGPS ()  
    {
        if (!isRequesting)
        {
            if (string.IsNullOrEmpty(lastResult) ||
                (Time.realtimeSinceStartup - lastRequestTime) > TimeLimit) //游戏时间开始到最终请求时间大于10分钟
            {
                StartCoroutine(StartGPS());
            }
            else if (!string.IsNullOrEmpty(lastResult))
            {
                OnLocate(lastResult);
            }
        }
    }  
  
    IEnumerator StartGPS ()
	{
        isRequesting = true;

        // 检查位置服务是否可用  
        if (!Input.location.isEnabledByUser) {
            if (OnError != null)
            {
                isRequesting = false;
                lastResult = string.Empty;
                lastRequestTime = Time.realtimeSinceStartup;
                OnError();
            }

			yield break;  
		}  
  
		// 查询位置之前先开启位置服务  
		Input.location.Start ();

        // 等待服务初始化  
        int maxWait = 10;  
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {  
			yield return new WaitForSeconds (1);  
			maxWait--;  
		}  
  
		// 服务初始化超时  
		if (maxWait < 1)
        {
            if (OnError != null)
            {
                isRequesting = false;
                lastResult = string.Empty;
                lastRequestTime = Time.realtimeSinceStartup;
                OnError();
            }
            yield break;  
		}  
  
		// 连接失败  
		if (Input.location.status == LocationServiceStatus.Failed) {
            if (OnError != null)
            {
                isRequesting = false;
                lastResult = string.Empty;
                lastRequestTime = Time.realtimeSinceStartup;
                OnError();
            }
            yield break;  
		}
  
		// 停止服务，如果没必要继续更新位置，（为了省电）  
		Input.location.Stop ();

        string loca = Input.location.lastData.latitude + "," + Input.location.lastData.longitude;
        string Url = "http://api.map.baidu.com/geocoder/v2/?callback=&location=" + 
            loca + "&output=json&pois=1&ak=h3PUGf0qjY4HeEimXgfuy1xNnkk14m8G";

		WWW str = new WWW (Url);

		yield return str;

        if (str.error == null)
        {
            JsonData json_data2 = new JsonData();
            json_data2 = JsonMapper.ToObject(str.text);
            Debug.Log(str.text);

            var address = json_data2["result"]["addressComponent"];
            if (OnLocate != null)
            {
                isRequesting = false;
                lastResult = address["city"].ToString() + address["district"].ToString() + address["street"].ToString();
                lastRequestTime = Time.realtimeSinceStartup;
                OnLocate(lastResult);
            }
        }
        else
        {
            if (OnError != null)
            {
                isRequesting = false;
                lastResult = string.Empty;
                lastRequestTime = Time.realtimeSinceStartup;
                OnError();
            }
        }
	}  
}  
