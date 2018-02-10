using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ViSpeak : MonoSingleton<ViSpeak>
{
    private AudioSource audioSource;
    private UIMainModel mUIMainModel;

    protected override void OnInit()
    {
        mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        audioSource = gameObject.GetOrAddComponent<AudioSource>();
        base.OnInit();
    }

    public void StartRecording()
    {
        _list.Clear();
        ViSpeakRecorder.Instance.StartRecording();
        ViSpeakRecorder.Instance.OnReSampleCompleteCallback = _ReSample;
    }

    private void _ReSample(ViChatPacket packet)
    {
        _list.AddRange(ViChatPacketSerializer.Serializer(packet));
    }

    public void StopReconding()
    {
        ViSpeakRecorder.Instance.StopRecording();
        byte[] buffer = _list.ToArray();
        //Debug.Log("Length : " + buffer.Length.ToString());
        RoomMsgInfo msg = new RoomMsgInfo()
        {
            UID = mUIMainModel.PlayerInfo.UserId,
            MsgType = RoomMsgType.Sound,
            Content = buffer
        };
        ActionParam actionParam = new ActionParam(msg);
        Net.Instance.Send((int)ActionType.RoomChatReq, null, actionParam);
    }

    public void PlayReconding(byte[] buffer, Action action = null)
    {
        audioSource.clip = AudioClip.Create("AA", 30 * 8000, 1, 8000, false);
        //byte[] buffer = _list.ToArray();
        //Debug.Log("Length : " + buffer.Length.ToString());
        List<ViChatPacket> list = new List<ViChatPacket>();
        int offest = 0;
        while (offest < buffer.Length)
        {
            ViChatPacket packet = ViChatPacketSerializer.DeSerializer(buffer, offest);
            offest = offest + 8 + packet.Data.Length;
            list.Add(packet);
        }
        //
        int sampleIndex = 0;
        for (int iter = 0; iter < list.Count; ++iter)
        {
            ViChatPacket iterPacket = list[iter];
            byte[] sampleData = new byte[iterPacket.DataLength];
            Buffer.BlockCopy(iterPacket.Data, 0, sampleData, 0, iterPacket.Length);
            float[] sample = ViSpeexCompress.DeCompress(_speexDec, sampleData, iterPacket.Length);
            audioSource.clip.SetData(sample, sampleIndex);
            sampleIndex += sample.Length;
        }
        AudioManager.Instance.PlayEffectAudio(audioSource.clip, action);
        //audioSource.Play();
        Debug.Log(audioSource.clip.length);
        //if (action != null)
        //{
        //    TimerMgr.instance.Subscribe(audioSource.clip.length * Time.timeScale, false, TimeEventType.IngoreTimeScale).OnComplete(delegate ()
        //      {
        //          if (action != null) { action(); }
        //      });
        //}

    }

    private List<byte> _list = new List<byte>();
    private NSpeex.SpeexDecoder _speexDec = new NSpeex.SpeexDecoder(NSpeex.BandMode.Narrow);
}
