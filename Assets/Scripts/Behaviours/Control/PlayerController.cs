using System.Collections;
using Audio;
using Behaviours.Combat.Player;
using Behaviours.Movement.PlayerMovement;
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
    
   
    
    [SerializeField] public Animator anim;  
    private AudioManager _audioManager;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _projectileFire = GetComponent<PlayerProjectileFire>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rigidbody2D   = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalParent = transform.parent;
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_playerMovement.ApplyDash(_trailRenderer, _rigidbody2D, anim, _audioManager)) return;
        _playerMovement.ApplyHorizontalMovement(_rigidbody2D, DirectionValue, knockBacked, anim);
        _playerMovement.ApplyJump(_rigidbody2D, _projectileFire.GetIsAttacking(), anim);
        _projectileFire.FireProjectile(anim, _projectileFire.GetIsAttacking(), _audioManager);
        _playerHealth.Defeat(anim, _playerMovement, _projectileFire);
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
        
    private IEnumerator FadeToWhite()
    {
        while (_spriteRenderer.color != Color.white)
        {
            yield return null;
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, Color.white, Time.deltaTime * 3);
        }
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
    

}
