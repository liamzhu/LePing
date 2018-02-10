using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropController : MonoBehaviour
{
    [HideInInspector]
    public Animator mAnim;
    private AudioSource mAudioSource;

    public PropFSM mPropFSM;
    public Vector3 mSpriteScale = Vector3.one;
    public bool mHasStartAnim = true;
    public bool mHasRotate = false;

    public string mStartAnimName;
    public string mDurationAnimName;
    public string mEndAnimName;
    public string mStartSound;

    public Vector3 mPosFrom;
    public Vector3 mPosTo;

    private void Awake()
    {
        mPropFSM = new PropFSM(this);
        mAnim = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();

        transform.localScale = mSpriteScale;
        Play();  //test
    }

    public void Play()
    {
        StartAnim();
    }

    private void Update()
    {
        mPropFSM.Update();
    }

    public void StartAnim()
    {
        mPropFSM.SwitchState(AnimState.Start);
    }

    public void PlayAnim(string stateName)
    {
        mAnim.Play(stateName);
    }

    public void PlaySound(string soundName)
    {
        mAudioSource.clip = Resources.Load<AudioClip>("Sounds/Prop/" + soundName);
        mAudioSource.Play();
    }

    public enum AnimState
    {
        Start,
        Duration,
        End
    }

}

public class PropFSM
{
    Dictionary<PropController.AnimState, AbsPropState> states = new Dictionary<PropController.AnimState, AbsPropState>();

    AbsPropState current;

    public PropFSM(PropController propController)
    {
        states.Add(PropController.AnimState.Start, new PropStartState(propController));
        states.Add(PropController.AnimState.Duration, new PropDurationState(propController));
        states.Add(PropController.AnimState.End, new PropEndState(propController));
    }

    public void SwitchState(PropController.AnimState state)
    {
        if (current != null)
        {
            current.OnLeave();
        }

        states[state].OnEnter();
        current = states[state];
    }

    public void Update()
    {
        current.Update();
    }
}

interface IPropState
{
    void OnEnter();
    void OnLeave();
}

abstract class AbsPropState : IPropState
{
    protected PropController mPropController;

    public AbsPropState(PropController propController)
    {
        mPropController = propController;
    }

    public abstract void OnEnter();
    public abstract void Update();
    public abstract void OnLeave();
    public abstract void OnAnimEnd();
}





class PropStartState : AbsPropState
{
    float timeTotal = 0;

    public PropStartState(PropController propController) : base(propController) { }

    public override void OnEnter()
    {
        timeTotal = 0;
        this.mPropController.gameObject.transform.position = this.mPropController.mPosFrom;
        this.mPropController.PlaySound(this.mPropController.mStartSound);

        if (!this.mPropController.mHasStartAnim)
        {
            this.mPropController.mPropFSM.SwitchState(PropController.AnimState.Duration);
            return;
        }

        this.mPropController.PlayAnim(this.mPropController.mStartAnimName);
    }

    public override void OnLeave()
    {
        
    }

    public override void Update()
    {
        timeTotal += Time.deltaTime;
        var info = this.mPropController.mAnim.GetCurrentAnimatorStateInfo(0);
        float length = info.length;
        if (timeTotal >= length)
        {
            OnAnimEnd();
        }
    }

    public override void OnAnimEnd()
    {
        this.mPropController.mPropFSM.SwitchState(PropController.AnimState.Duration);
    }
}

class PropDurationState : AbsPropState
{
    public PropDurationState(PropController propController) : base(propController) { }

    public override void OnEnter()
    {
        this.mPropController.PlayAnim(this.mPropController.mDurationAnimName);
        var tweener = this.mPropController.gameObject.transform.DOMove(this.mPropController.mPosTo, 1f);
        tweener.OnComplete(() => 
        {
            this.mPropController.mPropFSM.SwitchState(PropController.AnimState.End);
        });

        if (this.mPropController.mHasRotate)
        {
            Vector3 targetEuler = this.mPropController.gameObject.transform.eulerAngles + new Vector3(0, 0, -360 * 5);
            DOTween.To(() => this.mPropController.gameObject.transform.eulerAngles, x => this.mPropController.gameObject.transform.eulerAngles = x, targetEuler, 1f);
        }
    }

    public override void OnLeave()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void OnAnimEnd()
    {
        
    }
}

class PropEndState : AbsPropState
{
    float timeTotal = 0;

    public PropEndState(PropController propController) : base(propController) { }

    public override void OnEnter()
    {
        timeTotal = 0;
        this.mPropController.PlayAnim(this.mPropController.mEndAnimName);
    }

    public override void OnLeave()
    {
        
    }

    public override void Update()
    {
        timeTotal += Time.deltaTime;
        var info = this.mPropController.mAnim.GetCurrentAnimatorStateInfo(0);
        float length = info.length;
        if (timeTotal>= length)
        {
            OnAnimEnd();
        }
    }

    public override void OnAnimEnd()
    {
        GameObject.Destroy(this.mPropController.gameObject);
    }
}
