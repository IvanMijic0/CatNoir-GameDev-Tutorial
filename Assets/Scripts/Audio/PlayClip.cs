using UnityEngine;

namespace Audio
{
    //Testing audio
    public class PlayClip : MonoBehaviour
    {
        private AudioManager _audioManager;
        private int _clipNumber;

        public void Awake()
        {
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        public void Update()
        {
            if (Input.inputString == "") return;
            var isANumber = int.TryParse(Input.inputString, out var number);
            if (!isANumber || number is < 0 or >= 6) return;
            Debug.Log(number);
            _audioManager.PlaySound(number);
        }
    }
}

