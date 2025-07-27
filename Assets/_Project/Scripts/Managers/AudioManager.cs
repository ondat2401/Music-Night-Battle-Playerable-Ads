using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonModule<AudioManager>
{
    protected override bool DontDestroy => true;
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> audioClips = new List<AudioClip>();
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.volume = 1;
        bgmSource.Play();
    }
    public void PlaySFX(int clip)
    {
        if (audioClips[clip] == null) return;
        sfxSource.PlayOneShot(audioClips[clip]);
    }
    public void StopBGM()
    {
        if (bgmSource.clip != null)
            bgmSource.Stop();
    }
}
public static class AudioName
{
    public static readonly int SFX_CountDown_Go = 0;
    public static readonly int SFX_CountDown_3 = 1;
    public static readonly int SFX_CountDown_2 = 2;
    public static readonly int SFX_CountDown_1 = 3;
    public static readonly int SFX_Failed = 4;
}
