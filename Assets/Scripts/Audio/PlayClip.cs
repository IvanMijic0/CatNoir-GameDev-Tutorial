using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    //Testing audio
    public class PlayClip : MonoBehaviour
    {
        private AudioManager audioManager;
        private int clipNumber;

        public void Awake()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        public void Update()
        {
            if(Input.inputString != ""){
                int number;
                bool is_a_number = int.TryParse(Input.inputString, out number);
                if(is_a_number && number >= 0 && number < 6){
                    Debug.Log(number);
                    audioManager.PlaySound(number);
                }
            }
        }
    }
}

