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
    private Transform _originalParent;
    private PlayerMovement _playerMovement;
    private PlayerProjectileFire _projectileFire;
    private PlayerHealth _playerHealth;
    private AudioManager _audioManager;
    private GameObject _faderGameObject;
    private Fader _fader;
    private const int DirectionValue = 0;
    private bool _isFaded;

    [Header("Knock-back")] 
    [SerializeField] private Transform center;
    [SerializeField] private float knockBackVelocity = 8f;
    [SerializeField] private float knockBackTime     = .2f;
    [SerializeField] private bool knockBacked;

    [Header("Fading")] 
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private float loadTime = .8f;

    [Header("Animation")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject healthBar;
    
    private void Awake()
    {
        _faderGameObject = GameObject.FindGameObjectWithTag("Fader"); 
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _projectileFire = GetComponent<PlayerProjectileFire>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        _fader = _faderGameObject.GetComponent<Fader>();
        
    }

    private void Start()
    {
        _fader.FadeOutImmediate();
        _originalParent = transform.parent;
        StartCoroutine(LoadingWait());
        StartCoroutine(DisableFader());
    }

    private void Update()
    {
        Mechanics();
    }

    private void Mechanics()
    {
        if (Movement()) return;
        Combat();
    }

    private void Combat()
    {
        _projectileFire.FireProjectile(anim, _projectileFire.GetIsAttacking(), _audioManager);
        _playerHealth.Defeat(anim, _playerMovement, _projectileFire, _rigidbody2D);
        _playerHealth.healthBar.HealthBarMechanics(_playerHealth);
    }

    private bool Movement()
    {
        if (_playerMovement.ApplyDash(_trailRenderer, _rigidbody2D, anim, _audioManager)) return true;
        _playerMovement.ApplyHorizontalMovement(_rigidbody2D, DirectionValue, knockBacked, anim);
        _playerMovement.ApplyJump(_rigidbody2D, _projectileFire.GetIsAttacking(), anim);
        return false;
    }

    private IEnumerator FadeToWhite()
    {
        while (_spriteRenderer.color != Color.white)
        {
            yield return null;
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.white, Time.deltaTime * 3);
        }
    } 
    
    private IEnumerator LoadingWait()
    {
        yield return new WaitForSeconds(loadTime);
        _fader.FadeIn(fadeTime);
        healthBar.SetActive(true);
        
    }

    private IEnumerator DisableFader()
    {
        yield return new WaitForSeconds(loadTime + 1f);
        _faderGameObject.SetActive(false);
    }

    private IEnumerator UnKnockBack()
    {
        yield return new WaitForSeconds(knockBackTime);
        knockBacked = false;
        anim.SetBool(Damage, false);
    }
    
        
    public void KnockBack(Transform trans)
    {
        anim.SetBool(Damage, true);
        var direction = center.position - trans.position;
        knockBacked = true;
        _rigidbody2D.velocity = direction.normalized * knockBackVelocity;
        _spriteRenderer.color = Color.red;
        StartCoroutine(FadeToWhite());
        StartCoroutine(UnKnockBack());
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

    public PlayerProjectileFire GetProjectileFire()
    {
        return _projectileFire;
    }

    public AudioManager GetAudioManager()
    {
        return _audioManager;
    }


}
