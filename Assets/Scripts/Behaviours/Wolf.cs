using System.Collections;
using UnityEngine;
using Audio;
using UnityEngine.SceneManagement;


public class Wolf : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float titleDelay = 2.5f;

    private Rigidbody2D _rigidbody2D;
    private Animator _playerAnim;
    
    
    private static readonly int Grabbing = Animator.StringToHash("Grabbing");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        _rigidbody2D = playerController.GetComponent<Rigidbody2D>();
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
        _rigidbody2D.velocity = Vector2.zero;
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
