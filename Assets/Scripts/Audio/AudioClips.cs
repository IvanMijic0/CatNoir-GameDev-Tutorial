using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips
{
    private string audioClipName;
    private AudioClip[] audioClips;

    public void AudioClip(string audioClipName, AudioClip[] audioClips)
    {
        this.audioClipName = audioClipName;
        this.audioClips = audioClips;
    }   
}
