using UnityEngine;
using System.Collections;

public class RecordItem : MonoBehaviour
{
	private GameRecordData mRecord;
	private UILabel mWinLose;
    private UILabel mLabelDes;
    private UILabel mDateTime;
    private Transform mCacheTransform;

    private void Awake()
    {
        mCacheTransform = this.transform;
		mWinLose = mCacheTransform.FindComponent<UILabel> ("LabelWinLose");
        mLabelDes = mCacheTransform.FindComponent<UILabel>("LabelDes");
        mDateTime = mCacheTransform.FindComponent<UILabel>("LabelDateTime");
        mCacheTransform.GetUIEventListener("BtnDetails").onClick += OnDetailsClick;
    }

	public void Refresh(GameRecordData record)
    {
		this.mRecord = record;

		if (record.Flag == 1) {
			mWinLose.text = "胜利";
			mWinLose.color = Color.blue;
		}
		else {
			mWinLose.text = "失败";		
			mWinLose.color = Color.red;
		}
			
		mLabelDes.text = "当局分数："+record.RecordScore;

		//分数格式修改
		string tempStr_1 = record.RecordTime.Substring(0,19);
		//Debug.Log ("**************************** "+ tempStr_1);
		string tempStr_2 = tempStr_1.Replace ("T","  ");
		mDateTime.text = tempStr_2;
    }

    private void OnDetailsClick(GameObject go)
    {
        //UIWindowMgr.Instance.PushPanel<UIMailDetailsWindow>(mEmail);
		GameMgr.Instance.isFromRecord = true;
		GameMgr.Instance.EnterToGameWindow();

		ActionParam actionParam = new ActionParam();
		actionParam["RoundID"] = mRecord.RecordId;

		Net.Instance.Send ((int)ActionType.GameActionRecord, null, actionParam);

		//通过roundId 获取 gameId
		//然后通过GameID 获取房间信息
		//通过房间信息中的UseID，获取用户名 及 头像
    }
}
