using System.Collections;
using UnityEngine;

public class OperationEffectItem : MonoBehaviour
{
    private OperationEffectType mOperationEffectType;
    private UISpriteAnimation mUISpriteAnimation;
    private UISprite mUISprite;

	UIAtlas normalAtlas;
	UIAtlas newAddAtlas;

    private void Awake()
    {
        mUISpriteAnimation = this.transform.GetComponent<UISpriteAnimation>();
        mUISprite = this.transform.GetComponent<UISprite>();

		normalAtlas = mUISprite.atlas;
		//newAddAtlas = Resources.Load ("10_UICardEffectAtlas_Add") as UIAtlas;
		newAddAtlas = (UIAtlas)Resources.Load("10_UICardEffectAtlas_Add",typeof(UIAtlas));  
    }

    public void Initialize()
    {
        this.gameObject.SetActive(false);
    }

    public void SetEffectType(OperationEffectType type)
    {
		if (type == OperationEffectType.DaBao || type == OperationEffectType.ZiMo) {
			mUISprite.atlas = newAddAtlas;
		} 
		else {
			mUISprite.atlas = normalAtlas;
		}
			
        this.mOperationEffectType = type;
        ResetToBeginning();
    }

    public void ResetToBeginning()
    {
        this.gameObject.SetActive(true);
        mUISprite.spriteName = string.Format("{0}_{1}", mOperationEffectType, "0000");
        mUISpriteAnimation.namePrefix = string.Format("{0}_", mOperationEffectType);
        mUISpriteAnimation.ResetToBeginning();
        mUISpriteAnimation.enabled = true;

        TimerMgr.instance.Subscribe(1.2f, false, TimeEventType.IngoreTimeScale).OnComplete(() => this.gameObject.SetActive(false));
    }

    public enum OperationEffectType
    {
        Chi = 1,
        Peng = 2,
        Gang = 3,
        Hu = 5,
        Ting = 6,
		ZiMo = 7,
		DaBao = 8,
    }
}
