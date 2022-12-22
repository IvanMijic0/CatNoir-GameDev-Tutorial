using System.Collections;
using UnityEngine;
using Audio;
using Behaviours.Movement.PlayerMovement;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class Wolf : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float titleDelay = 3.5f;

    private Rigidbody2D _rigidBody2D;
    private Animator _playerAnim;

    private static readonly int Grabbing = Animator.StringToHash("Grabbing");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Dash = Animator.StringToHash("dash");

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        _rigidBody2D = playerController.GetComponent<Rigidbody2D>();
        _playerAnim = playerController.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            DisableControl();
        }
    }

    private void DisableControl()
    {
        playerController.enabled = false;
        _rigidBody2D.velocity = Vector2.zero;
        
        _playerAnim.SetBool(Fall, false);
        _playerAnim.SetBool(Jump, false);
        _playerAnim.SetBool(Dash, false);
        _playerAnim.SetBool(IsRunning, false);
        animator.SetBool(Grabbing, true);
        
        audioManager.PlaySound(6);
        StartCoroutine(TitleScreen());
    }

    private IEnumerator TitleScreen()
    {
        yield return new WaitForSeconds(titleDelay);
        SceneManager.LoadScene(0);
    }
    
}
