using System;
using UnityEngine;

public class EndAudio : MonoBehaviour
{
   [SerializeField] private AudioSource audioSource;

   private void Start()
   {
      audioSource.volume = PlayerPrefs.GetFloat("musicVol");
   }
}
