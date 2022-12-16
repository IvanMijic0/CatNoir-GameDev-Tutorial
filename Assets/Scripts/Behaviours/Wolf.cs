using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class Wolf : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Animator animator;

    void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Grabbing", true);
            audioManager.PlaySound(6);    
        }
    }
}
