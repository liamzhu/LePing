using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordControl : MonoBehaviour {

	UIButton btnReduce;
	UIButton btnPlayPause;
	UIButton btnAdd;
	UIButton btnBack;
	UILabel progressLabel;
	bool isPlay = true;

	private void Awake()
	{
		btnReduce = transform.FindComponent<UIButton> ("BtnReduce");
		btnPlayPause = transform.FindComponent<UIButton> ("BtnPlayPause");
		btnAdd = transform.FindComponent<UIButton> ("BtnAdd");
		btnBack = transform.FindComponent<UIButton> ("BtnBack");
		progressLabel = transform.FindComponent<UILabel> ("LabelProgress");

		UIEventListener.Get(btnReduce.gameObject).onClick += OnReduceClick;
		UIEventListener.Get(btnPlayPause.gameObject).onClick += OnPlayPauseClick;
		UIEventListener.Get(btnAdd.gameObject).onClick += OnAddClick;
		UIEventListener.Get(btnBack.gameObject).onClick += OnBackClick;

		isPlay = true;
	}

	//减速 3 2 1 0.5
	void OnReduceClick(GameObject go){
		TimerManager.Instance.recordBackDurTime += 0.5f; 
		if (TimerManager.Instance.recordBackDurTime >= 2.0f) {
			TimerManager.Instance.recordBackDurTime = 2.0f;
		}
	}

	void OnPlayPauseClick(GameObject go){
		//UISprite curSprite = go.GetComponentInChildren<UISprite> ();
		UIButton curBtn = go.GetComponent<UIButton>();

		if (isPlay) {
			isPlay = false;
			//TimerManager.Instance.LiamTimeCancel ();
			//Time.timeScale = 0;
			//curSprite.spriteName = "pause";

			curBtn.normalSprite = "pause";
			curBtn.hoverSprite = "pause";
			curBtn.pressedSprite = "pause";
			StartCoroutine ("WaitToPause");
		}
		else {
			isPlay = true;
			//TimerManager.Instance.LiamTimeResume ();
			Time.timeScale = 1;
			//curSprite.spriteName = "play";

			curBtn.normalSprite = "play";
			curBtn.hoverSprite = "play";
			curBtn.pressedSprite = "play";
		}
	}

	IEnumerator WaitToPause(){
		yield return null;
		yield return null;
		yield return null;
		Time.timeScale = 0;
	}

	//加速
	void OnAddClick(GameObject go){
		TimerManager.Instance.recordBackDurTime -= 0.5f; 
		if (TimerManager.Instance.recordBackDurTime <= 0.0f) {
			TimerManager.Instance.recordBackDurTime = 0.5f;
		}
	}

	void OnBackClick(GameObject go){
		TimerManager.Instance.LiamTimeCancel ();
		GameMgr.Instance.EnterToMainWindow();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 5 != 0) {
			return;
		}

		float tempProgress = (float)Action1210.curIndex / (float)Action1210.nodeLen;
		if (tempProgress >= 1.0f) {
			tempProgress = 1.0f;
		}

		int progress = (int)(tempProgress * 100);

		progressLabel.text = "进度：" + progress.ToString () + "%";
	}
}
