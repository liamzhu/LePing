using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GameFrame/Component/AudioManager")]
[RequireComponent(typeof(AudioListener)), DisallowMultipleComponent]
public class AudioManager : MonoSingleton<AudioManager>
{

    #region 音量控制

    private float mGlobalVolume = 1f;

    public float GlobalVolume
    {
        get
        {
            return mGlobalVolume;
        }
        set
        {
            mGlobalVolume = value;
            PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_GLOBAL_VOLUME, mGlobalVolume);
            OnMusicVolumeChange();
            OnSoundVolumeChange();
        }
    }

    private float mMusicVolume = 1f;

    public float MusicVolume
    {
        get
        {
            return mMusicVolume * mGlobalVolume;
        }
        set
        {
            mMusicVolume = value;
            OnMusicVolumeChange();
        }
    }

    private float mSoundVolume = 1f;

    public float SoundVolume
    {
        get
        {
            return mSoundVolume * mGlobalVolume;
        }
        set
        {
            mSoundVolume = value;
            OnSoundVolumeChange();
        }
    }

    private void OnMusicVolumeChange()
    {
        PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_MUSIC_VOLUME, mMusicVolume);
        if (mBGAudioSource != null)
        {
            mBGAudioSource.volume = MusicVolume;
        }
    }

    private void OnSoundVolumeChange()
    {
        PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_SOUND_VOLUME, mSoundVolume);
        if (!mSoundAudioSources.IsNullOrEmpty())
        {
            for (int i = 0; i < mSoundAudioSources.Count; i++)
            {
                mSoundAudioSources[i].volume = SoundVolume;
            }
        }
    }

    #endregion 音量控制

    private const int MaxCount = 10;

    [SerializeField]
    private Dictionary<string, AudioClip> mAudioClips;

    private Queue<AudioSource> mCacheSourceQueue;
    private AudioSource mBGAudioSource;
    private List<AudioSource> mSoundAudioSources;

    #region 背景音乐相关操作

    public void PlayBGAudio(string audioName)
    {
        Play(GetAudioClip(audioName), SoundType.Music, MusicVolume, 1f, true, null);
    }

    public void StopBGAudio()
    {
        if (mBGAudioSource != null && mBGAudioSource.isPlaying)
        {
            mBGAudioSource.Pause();
        }
    }

    public void PlayCurrBGAudio()
    {
        if (mBGAudioSource != null && !mBGAudioSource.isPlaying)
        {
            mBGAudioSource.Play();
        }
    }

    #endregion 背景音乐相关操作

    public void PlayEffectAudio(string audioEffectName, System.Action action = null)
    {
        Play(GetAudioClip(audioEffectName), SoundType.Sound, SoundVolume, 1f, false, action);
    }

    public void PlayEffectAudio(AudioClip audioClip, System.Action action = null)
    {
        Play(audioClip, SoundType.Sound, 1f, 1f, false, action);
    }

    private AudioSource Play(AudioClip audioClip, SoundType soundType, float volume, float pitch, bool isloop, System.Action action)
    {
        if (audioClip == null) { return null; }
        AudioSource source;
        if (soundType == SoundType.Music)
        {
            if (mBGAudioSource == null)
            {
                mBGAudioSource = Spawn();
            }
            source = mBGAudioSource;
        }
        else { source = Spawn(); }
        if (source != null)
        {
            source.gameObject.name = string.Format("Audio: {0}", audioClip.name);
            source.clip = audioClip;
            source.loop = isloop;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
        //Debug.Log(source.clip.length);
        if (soundType == SoundType.Sound)
        {
            TimerMgr.instance.Subscribe(source.clip.length, false, TimeEventType.IngoreTimeScale).OnComplete(delegate ()
            {
                Recycle(source);
                if (action != null) { action(); }
            });
        }
        return source;
    }

    public AudioSource PlayEmitter(string name, Transform emitter, System.Action action = null)
    {
        return PlayEmitter(GetAudioClip(name), emitter, 1f, 1f, action);
    }

    public AudioSource PlayEmitter(string name, Transform emitter, float volume, System.Action action = null)
    {
        return PlayEmitter(GetAudioClip(name), emitter, volume, 1f, action);
    }

    public AudioSource PlayEmitter(AudioClip clip, Transform emitter, float volume, float pitch, System.Action action = null)
    {
        if (clip == null) { return null; }
        AudioSource source = Spawn();
        if (source != null)
        {
            source.gameObject.name = string.Format("Audio: {0}", clip.name);
            source.transform.position = emitter.position;
            source.transform.parent = emitter;
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
        TimerMgr.instance.Subscribe(source.clip.length, false, TimeEventType.IngoreTimeScale).OnComplete(delegate ()
        {
            Recycle(source);
            if (action != null) { action(); }
        });
        return source;
    }

    public AudioSource PlayEmitter(string audioEffectName, Vector3 point, System.Action action = null)
    {
        return PlayEmitter(GetAudioClip(audioEffectName), point, SoundVolume, 1f, action);
    }

    public AudioSource PlayEmitter(string audioEffectName, Vector3 point, float volume, System.Action action = null)
    {
        return PlayEmitter(GetAudioClip(audioEffectName), point, volume, 1f, action);
    }

    public AudioSource PlayEmitter(AudioClip clip, Vector3 point, float volume, float pitch, System.Action action = null)
    {
        if (clip == null) { return null; }
        AudioSource source = Spawn();
        if (source != null)
        {
            source.gameObject.name = string.Format("Audio: {0}", clip.name);
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
        TimerMgr.instance.Subscribe(source.clip.length, false, TimeEventType.IngoreTimeScale).OnComplete(delegate ()
        {
            Recycle(source);
            if (action != null) { action(); }
        });
        return source;
    }

    private void Recycle(AudioSource mAudioSource)
    {
        mAudioSource.gameObject.name = "AudioCache";
        mAudioSource.transform.parent = this.transform;
        mAudioSource.transform.position = Vector3.zero;
        mCacheSourceQueue.Enqueue(mAudioSource);
    }

    private AudioSource Spawn()
    {
        if (mCacheSourceQueue.Count <= 0)
        {
            CreateAudioSource();
        }
        return mCacheSourceQueue.Dequeue();
    }

    private AudioClip GetAudioClip(string name)
    {
        AudioClip mAudioClip;
        if (mAudioClips.TryGetValue(name, out mAudioClip))
        {
            return mAudioClip;
        }
        else
        {
            mAudioClip = Resources.Load<AudioClip>(ResConst.SOUND_PATH + name);
            mAudioClips.Add(name, mAudioClip);
            return mAudioClip;
        }
    }

    private void GenerateCacheSourceQueue()
    {
        for (int i = 0; i < MaxCount; i++)
        {
            CreateAudioSource();
        }
    }

    private void CreateAudioSource()
    {
        GameObject go = new GameObject("AudioCache");
        go.transform.parent = this.transform;
        mCacheSourceQueue.Enqueue(go.GetOrAddComponent<AudioSource>());
    }

    #region 重写虚方法

    protected override void OnInit()
    {
        LoadAllSetting();
        if (mAudioClips == null) { mAudioClips = new Dictionary<string, AudioClip>(); }
        if (mCacheSourceQueue == null) { mCacheSourceQueue = new Queue<AudioSource>(); }
        if (mSoundAudioSources == null) { mSoundAudioSources = new List<AudioSource>(); }
        GenerateCacheSourceQueue();
        base.OnInit();
    }

    protected override void OnUnInit()
    {
        PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_SOUND_VOLUME, mSoundVolume);
        PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_MUSIC_VOLUME, mMusicVolume);
        PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_GLOBAL_VOLUME, mGlobalVolume);
        if (mAudioClips != null) { mAudioClips.Clear(); }
        if (mCacheSourceQueue != null) { mCacheSourceQueue.Clear(); }
        if (mSoundAudioSources != null) { mSoundAudioSources.Clear(); }
    }

    #endregion 重写虚方法

    #region 保存和加载音量设置

    private void LoadAllSetting()
    {
        bool isAppLaunchedFirstTime = PlayerPrefsUtil.GetBool(PrefsConstant.KEY_IS_APP_LAUNCHED_FIRST_TIME);

        if (!isAppLaunchedFirstTime)
        {
            PlayerPrefsUtil.SetBool(PrefsConstant.KEY_IS_APP_LAUNCHED_FIRST_TIME, true);
            PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_SOUND_VOLUME, 1f);
            PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_MUSIC_VOLUME, 1f);
            PlayerPrefsUtil.SetFloat(PrefsConstant.KEY_GLOBAL_VOLUME, 1f);
        }
        mSoundVolume = PlayerPrefsUtil.GetFloat(PrefsConstant.KEY_SOUND_VOLUME);
        mMusicVolume = PlayerPrefsUtil.GetFloat(PrefsConstant.KEY_MUSIC_VOLUME);
        mGlobalVolume = PlayerPrefsUtil.GetFloat(PrefsConstant.KEY_GLOBAL_VOLUME);
        //Debug.Log("Load " + mSoundVolume);
        //Debug.Log("Load " + mMusicVolume);
        //Debug.Log("Load " + mGlobalVolume);
    }

    #endregion 保存和加载音量设置

    public enum SoundType { Sound, Music, };
}
