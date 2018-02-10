//此代码是工具根据 Dialog.json 自动生成,不要手动修改!!!
//生成时间： 2/4/2017 10:16:58 AM
using System;
using System.Collections.Generic;

[System.Serializable]
public class DialogCfg : ICfg
{
	public int Id; // ID
	public int OutType; // 消息框类型 1:OK 2:确认和取消 9:文字漂浮 3:无按钮
	public string Title; // 标题
	public string Contents; // 描述

	public DialogCfg() { }

	public string GetKey()
	{
		return Id.ToString();
	}
}
