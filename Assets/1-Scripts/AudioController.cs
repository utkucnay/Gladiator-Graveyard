using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    MurmilloGladiusSwing,
    FleshHit,
    ShieldHit,
    Parry,
    ArrowHit,
    BowRelease,
    CardDeal,
    CardHover
}

[System.Serializable]public struct AudioInfo
{
    public AudioType audioType;
    public AudioSource audioSource;
}

public class AudioController : Singleton<AudioController>
{
    [SerializeField] public List<AudioInfo> audioTypes;
    private Dictionary<AudioType, AudioSource> audioDict;
    public override void Awake()
    {
        base.Awake();

        audioDict = new Dictionary<AudioType, AudioSource>();
        foreach (AudioInfo info in audioTypes)
        {
            audioDict.Add(info.audioType, info.audioSource);
        }

    }
    public override void Start()
    {
        base.Start();
    }

    public override void RunStarted()
    {
        base.RunStarted();
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    public void PlayAudio(AudioType audioType)
    {
        audioDict[audioType].Play();
    }
    public void PlayAudio(AudioType audioType, float pitch)
    {
        audioDict[audioType].pitch = pitch;
        audioDict[audioType].Play();
    }
    public void PlayAudio(AudioType audioType, float minPitch, float maxPitch)
    {
        audioDict[audioType].pitch = Random.Range(minPitch,maxPitch);
        audioDict[audioType].Play();
    }
}
