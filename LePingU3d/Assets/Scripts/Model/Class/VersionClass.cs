using System.Collections;
using UnityEngine;

public class VersionInfoReq
{
    public string version;
}

public class VersionInfoResp
{
    public bool update; //是否更新（fasle|true）
    public string path; //路径
    public string version;//版本号
    public string port;
}
