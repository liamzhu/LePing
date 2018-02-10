using System;
using System.Collections.Generic;
using UnityEngine;

public class ViSpeakRecorder : MonoSingleton<ViSpeakRecorder>
{
    public Action<ViChatPacket> OnReSampleCompleteCallback;

    public bool IsRecording { get { return _isRecording; } }
    public string RecorderDevice { get { return _device; } }
    public AudioClip Clip { get { return _clip; } }

    protected override void OnInit()
    {
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }
        base.OnInit();
    }

    private void Update()
    {
        if (!_isRecording)
        {
            return;
        }
        //
        int currentRecodingPos = Microphone.GetPosition(null);
        //Debug.Log("Time: " + currentRecodingPos / ViSpeakSetting.SamplingRate);
        if (currentRecodingPos < _lastRecordingPos)
        {

        }
        //
        _lastRecordingPos = currentRecodingPos;
        while (_sampleIndex + _recordSampleSize <= currentRecodingPos)
        {
            _OnResample();
        }
    }

    private void _OnResample()
    {
        _clip.GetData(_sampleBuffer, _sampleIndex);
        float[] targetBuffer = new float[ViSpeakSetting.FrameSampleSize];
        Resample(_sampleBuffer, targetBuffer);
        _sampleIndex += _recordSampleSize;
        TransmitBuffer(targetBuffer);
    }

    private void TransmitBuffer(float[] sampleBuffer)
    {
        int length = 0;
        byte[] buffer = ViSpeexCompress.SpeexCompress(sampleBuffer, out length);
        ViChatPacket packet = new ViChatPacket();
        byte[] availableSampleBuffer = new byte[length];
        Buffer.BlockCopy(buffer, 0, availableSampleBuffer, 0, length);
        packet.Data = availableSampleBuffer;
        packet.Length = length;
        packet.DataLength = buffer.Length;
        if (OnReSampleCompleteCallback != null)
        {
            OnReSampleCompleteCallback(packet);
        }
    }

    public void StartRecording()
    {
        _targetFrequency = ViSpeakSetting.SamplingRate;
        _targetSampleSize = ViSpeakSetting.FrameSampleSize;
        int minFreq;
        int maxFreq;
        _InitDevice();
        Microphone.GetDeviceCaps(_device, out minFreq, out maxFreq);
        _recordFrequency = minFreq == 0 && maxFreq == 0 ? 8000 : maxFreq;
        _recordSampleSize = _recordFrequency / (_targetFrequency / _targetSampleSize);
        _sampleBuffer = new float[_recordSampleSize];
        _clip = Microphone.Start(_device, true, 10, _recordFrequency);
        //
        _isRecording = true;
    }

    public void StopRecording()
    {
        Microphone.End(null);
        _isRecording = false;
        _lastRecordingPos = 0;
        _sampleIndex = 0;
    }

    private void _InitDevice()
    {
        if (Microphone.devices.Length > 0)
        {
            _device = Microphone.devices[0];
        }
    }

    ///音频去噪
    private void Resample(float[] src, float[] dst)
    {
        if (src.Length == dst.Length)
        {
            Array.Copy(src, 0, dst, 0, src.Length);
        }
        else
        {
            float rec = 1.0f / (float)dst.Length;

            for (int i = 0; i < dst.Length; ++i)
            {
                float interp = rec * (float)i * (float)src.Length;
                dst[i] = src[(int)interp];
            }
        }
    }

    private static ViSpeakRecorder _instance;
    private bool _isRecording = false;
    private int _lastRecordingPos = 0;
    private int _sampleIndex = 0;
    private int _recordFrequency = 0;
    private int _recordSampleSize = 0;
    private int _targetFrequency = 0;
    private int _targetSampleSize = 0;
    private float[] _sampleBuffer;
    private string _device;
    private AudioClip _clip;
}
