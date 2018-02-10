using UnityEngine;
using System.Collections;

public class MahjongAudioMgr : Singleton<MahjongAudioMgr>
{
    private const string BGM = "bgm_2";
    private const string male = "male";
    private const string female = "female";

    /// <summary> 国语0 </summary>
    private const string national = "national";

    /// <summary> 方言1 </summary>
    private const string localism = "localism";

	AudioSource dadadaAudio;

    public void PlayBGM()
    {
        AudioManager.Instance.PlayBGAudio(BGM);
    }

    public void PauseBGM(bool isPause)
    {
        if (isPause)
        {
            AudioManager.Instance.StopBGAudio();
        }
        else
        {
            AudioManager.Instance.PlayCurrBGAudio();
        }
    }

    public void PlayChiPaiSound(int sex,int type)
    {
        string name = string.Format("{2}/{0}/chi{1}", sex == 1 ? male : female, Random.Range(1, 4), type == 0 ? national : localism);
        AudioManager.Instance.PlayEffectAudio(name);
    }

    public void PlayPengPaiSound(int sex, int type)
    {
        string name = string.Format("{2}/{0}/peng{1}", sex == 1 ? male : female, Random.Range(1, 5), type == 0 ? national : localism);
        AudioManager.Instance.PlayEffectAudio(name);
    }

    public void PlayGangPaiSound(int sex, int type)
    {
        string name = string.Format("{2}/{0}/gang{1}", sex == 1 ? male : female, Random.Range(1, 3), type == 0 ? national : localism);
        AudioManager.Instance.PlayEffectAudio(name);
    }

    public enum HuPaiType
    {
        Normal,
        ZiMo
    }

    public void PlayHuPaiSound(int sex, int mHuPaiType, int type)
    {
        if (mHuPaiType == 1)
        {
            //string name = string.Format("{0}/zimo{1}", sex == 1 ? male : female, Random.Range(1, 2));
			string name = string.Format("{2}/{0}/zimo{1}", sex == 1 ? male : female, Random.Range(1, 2),type == 0 ? national : localism);
            AudioManager.Instance.PlayEffectAudio(name);
        }
        else
        {
            string name = string.Format("{2}/{0}/hu{1}", sex == 1 ? male : female, Random.Range(1, 3), type == 0 ? national : localism);
            AudioManager.Instance.PlayEffectAudio(name);
        }
    }

    public void PlayChuPaiSound(int sex, Card card, int type)
    {
        string name = string.Format("{3}/{0}/mjt{1}_{2}", sex == 1 ? male : female, card.CardType + 1, card.CardValue,type == 0 ? national : localism);
        AudioManager.Instance.PlayEffectAudio(name);
    }

    public void PlayChatSound(int sex, int msgIndex, int type)
    {
        string name = string.Format("{2}/{0}/fix_msg_{1}", sex == 1 ? male : female, msgIndex, type == 0 ? national : localism);
        AudioManager.Instance.PlayEffectAudio(name);
    }

    public void PlayDaBao(int sex)
    {
        string name = string.Format("common/dabao_{0}", sex == 1 ? male : female);
        AudioManager.Instance.PlayEffectAudio(name);
    }

	//摸牌
	public void PlayMoPai(){
		string name = "common/audio_deal_card";//string.Format("common/dabao_{0}", sex == 1 ? male : female);
		AudioManager.Instance.PlayEffectAudio(name);
	}

	//出牌
	public void PlayChuPai(){
		string name = "common/audio_card_out";//string.Format("common/dabao_{0}", sex == 1 ? male : female);
		AudioManager.Instance.PlayEffectAudio(name);
	}

	//哒哒哒
	public void PlayTimeDaDaDa(){
		if (dadadaAudio == null) {
			dadadaAudio = GameObject.Find ("AlarmDaDaDa").GetComponent<AudioSource> ();
		}
		//string name = "common/timeup_alarm_15";//string.Format("common/dabao_{0}", sex == 1 ? male : female);
		//AudioManager.Instance.PlayEffectAudio(name);

		//NGUIDebug.Log ("@@@@@@@@@@@@@      "+AudioManager.Instance.SoundVolume.ToString());

		dadadaAudio.volume = AudioManager.Instance.SoundVolume;
		dadadaAudio.Play();
	}

	public void StopTimeDaDaDa(){
		if (dadadaAudio != null) {
			dadadaAudio.Stop();
		}
	}
}
