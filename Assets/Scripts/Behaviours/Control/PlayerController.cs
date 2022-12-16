using System.Collections;
using Audio;
using Behaviours.Combat.Player;
using Behaviours.Movement.PlayerMovement;
using UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int Damage = Animator.StringToHash("damage");
    

    private Rigidbody2D _rigidbody2D;
    private TrailRenderer _trailRenderer;
    private SpriteRenderer _spriteRenderer;
    private const int DirectionValue = 0;
    private Transform _originalParent;
    private PlayerMovement _playerMovement;
    private PlayerProjectileFire _projectileFire;
    private PlayerHealth _playerHealth;
    

    [Header("Knock-back")] 
    [SerializeField] private Transform center;
    [SerializeField] private float knockBackVelocity = 8f;
    [SerializeField] private float knockBackTime     = .2f;
    [SerializeField] private bool knockBacked;
    
    [Header("Fading")]   
    [SerializeField] private Fader fader;
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private float loadTime = .8f;

    [SerializeField] private Animator anim;
    private AudioManager _audioManager;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _projectileFire = GetComponent<PlayerProjectileFire>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        fader.FadeOutImmediate();
        _originalParent = transform.parent;
        StartCoroutine(LoadingWait());
    }

    private void Update()
    {
        Mechanics();
    }

    private void Mechanics()
    {
        if (_playerMovement.ApplyDash(_trailRenderer, _rigidbody2D, anim, _audioManager)){ return;}
        _playerMovement.ApplyHorizontalMovement(_rigidbody2D, DirectionValue, knockBacked, anim);
        _playerMovement.ApplyJump(_rigidbody2D, _projectileFire.GetIsAttacking(), anim);
        _projectileFire.FireProjectile(anim, _projectileFire.GetIsAttacking(), _audioManager);
        _playerHealth.Defeat(anim, _playerMovement, _projectileFire);
    }

   
        
    private IEnumerator FadeToWhite()
    {
        while (_spriteRenderer.color != Color.white)
        {
            yield return null;
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.white, Time.deltaTime * 3);
        }
    } 
    
    public void KnockBack(Transform trans)
    {
        anim.SetBool(Damage, true);
        var direction = center.position - trans.position;
        knockBacked = true;
        _rigidbody2D.velocity = direction.normalized * knockBackVelocity;
        Debug.Log("Damage");
        _spriteRenderer.color = Color.red;
        StartCoroutine(FadeToWhite());
        StartCoroutine(UnKnockBack());
    }
        
    private IEnumerator UnKnockBack()
    {
        yield return new WaitForSeconds(knockBackTime);
        knockBacked = false;
        anim.SetBool(Damage, false);
    }
    public void SetParent(Transform newParent)
    {
        _originalParent = transform.parent;
        if (transform != null) transform.parent = newParent;
    }
        
    public void ResetParent()
    {
        transform.parent = _originalParent;
    }

    private IEnumerator LoadingWait()
    {
        yield return new WaitForSeconds(loadTime);
        fader.FadeIn(fadeTime);
    }

}
