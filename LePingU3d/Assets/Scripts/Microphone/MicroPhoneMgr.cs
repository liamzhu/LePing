using UnityEngine;
using System.Collections;

public class MicroPhoneMgr : MonoSingleton<MicroPhoneMgr>
{

    private AudioClip clip;
    private byte[] recordData;
    private const int SamplingRate = 8000;
    private const int RecordTime = 10;

    private void CheckMicrophoneDevices()
    {
        string[] micDevices = Microphone.devices;

        if (micDevices.Length == 0)
        {
            UIDialogMgr.Instance.ShowDialog("没有找到录音组件");
            return;
        }
    }

    public void OnRecording(GameObject goSender, bool flag)
    {
        if (flag)
        {
            ViSpeak.Instance.StartRecording();
            //Recording();
        }
        else {//按钮弹起结束录音
            ViSpeak.Instance.StopReconding();
            //StopRecord();
        }
    }

    public void Recording()
    {
        CheckMicrophoneDevices();
        Microphone.End(null);//录音前先停掉录音，录音参数为null时采用的是默认的录音驱动
        clip = Microphone.Start(null, false, RecordTime, SamplingRate);
    }

    public void StopRecord()
    {
        CheckMicrophoneDevices();
        int audioLength;
        int lastPos = Microphone.GetPosition(null);
        if (Microphone.IsRecording(null))
        {
            audioLength = lastPos / SamplingRate;
        }
        else
        {
            audioLength = RecordTime;
        }

        Microphone.End(null);

        if (audioLength < 1.0f)
        {
            UIDialogMgr.Instance.ShowDialog("录音时间过短");
            return;
        }
        PrintRecord();
    }

    private void PrintRecord()
    {
        if (Microphone.IsRecording(null))
        {
            return;
        }
        byte[] data = GetClipData();
        string slog = "total length:" + data.Length;
        Debug.Log(slog);
    }

    public byte[] GetClipData()
    {
        if (clip == null)
        {
            Debug.Log("GetClipData clip is null");
            return null;
        }

        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        byte[] outData = new byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            short temshort = (short)(samples[i] * rescaleFactor);

            byte[] temdata = System.BitConverter.GetBytes(temshort);

            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];

        }
        if (outData == null || outData.Length <= 0)
        {
            Debug.Log("GetClipData intData is null");
            return null;
        }
        return outData;
    }

    public float Volume
    {
        get
        {
            if (Microphone.IsRecording(null))
            {
                // 采样数
                int sampleSize = 128;
                float[] samples = new float[sampleSize];
                int startPosition = Microphone.GetPosition(null) - (sampleSize + 1);
                // 得到数据
                clip.GetData(samples, startPosition);

                // Getting a peak on the last 128 samples
                float levelMax = 0;
                for (int i = 0; i < sampleSize; ++i)
                {
                    float wavePeak = samples[i];
                    if (levelMax < wavePeak)
                        levelMax = wavePeak;
                }

                return levelMax * 99;
            }
            return 0;
        }
    }
}
