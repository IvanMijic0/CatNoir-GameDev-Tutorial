using System.Collections;
using Extensions;
using Enums;
using Interfaces;
using Audio;
using UnityEngine;

namespace Behaviours.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlatformMovement : MonoBehaviour
    {
      private Rigidbody2D _rigidbody2D;
      private ICheck _groundCheck;
      private TrailRenderer _trailRenderer;
      private SpriteRenderer _spriteRenderer;
      private int _jumpsLeft;
      private int _directionValue = 0;
      private int _yRotation;
      private bool _isAttacking;
      private Transform _originalParent;

      [Header("Movement Configuration")] 
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject groundCheckObject;
    [SerializeField] private int maxJumps = 2;

    [Header("Dashing")] 
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = .5f;

    [Header("Knock-back")] 
    [SerializeField] private Transform center;
    [SerializeField] private float knockBackVelocity = 8f;
    [SerializeField] private float knockBackTime = .2f;
    [SerializeField] private bool knockBacked;

    [Header("Animation Sprites")] 
    [SerializeField] private Sprite fallSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite attackSprite;
    
    [Header("Attack Sprite")]
    [SerializeField] private float attackTime = .5f;

    [SerializeField] public Animator anim;  
    private AudioManager audioManager;

    private Vector2 _dashingDir;
    private bool _isDashing;
    private bool _canDash = true;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int Dash = Animator.StringToHash("dash");
    private static readonly int Damage = Animator.StringToHash("damage");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Jump = Animator.StringToHash("jump");

    private void Awake()
    {
      _rigidbody2D   = GetComponent<Rigidbody2D>();
      _trailRenderer = GetComponent<TrailRenderer>();
      _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
      _groundCheck   = groundCheckObject.GetComponent<ICheck>();
      _jumpsLeft     = maxJumps;
      _originalParent = transform.parent;
      audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
      anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
      if (ApplyDash()) return;
      
      ApplyHorizontalMovement();
      ApplyJump();
      
      
    }

    private bool ApplyDash()
    {
      if (Input.GetButtonDown("Dash") && _canDash)
      {
        audioManager.PlaySound(0);
        anim.SetBool(IsRunning, false);
        anim.SetBool(Fall, false);
        anim.SetBool(Jump, false);
        anim.SetBool(Dash, true);
        _isDashing = true;
        _canDash = false;
        _trailRenderer.emitting = true;
        _dashingDir = new Vector2(Input.GetAxisRaw(Axis.X.ToUnityAxis()), Input.GetAxisRaw(Axis.Y.ToUnityAxis()));
        if (_dashingDir == Vector2.zero)
        {
          if (transform.rotation.y < 0)
          {
            _dashingDir = new Vector2(-transform.localScale.x, 0);
          }
          else
          {
            _dashingDir = new Vector2(transform.localScale.x, 0);
          }
        }
        StartCoroutine(StopDashing());
      }

      if (_isDashing)
      {
        _rigidbody2D.velocity = _dashingDir * dashingVelocity;
        return true;
      }

      if (IsGrounded())
      {
        _canDash = true;
      }

      return false;
    }

    private void ApplyJump()
    {
      if (IsGrounded() && _rigidbody2D.velocity.y <= 0 && !_isAttacking)
      {
        _spriteRenderer.sprite = idleSprite;
        _jumpsLeft = maxJumps;

        anim.SetBool(Fall, false);
        anim.SetBool(Jump, false);
      }

      if (_rigidbody2D.velocity.y < -.1f)
      {
        _spriteRenderer.sprite = fallSprite;

        anim.SetBool(Fall, true);
        anim.SetBool(Jump, false);
      }
    
      if (Input.GetButtonDown("Jump") && _jumpsLeft > 0)  
      {
        _rigidbody2D.SetVelocity(Axis.Y, jumpForce);
        //_spriteRenderer.sprite = jumpSprite;
        _jumpsLeft--;

        anim.SetBool(Jump, true);
        anim.SetBool(Fall, false);
      }
      
    }

    private bool IsGrounded()
    {
      return _groundCheck.Check();
    }

    private void ApplyHorizontalMovement()
    {
      
       var inputX = Input.GetAxisRaw(Axis.X.ToUnityAxis());
        
        var newXVelocity = 
          knockBacked ? Mathf.Lerp(_rigidbody2D.velocity.x, 0f, Time.deltaTime * 3) : inputX * walkSpeed;
        
        FlipSprite(inputX);
        
        _rigidbody2D.SetVelocity(Axis.X, newXVelocity);
      
        if(newXVelocity != 0f && IsGrounded() && Input.GetAxisRaw(Axis.X.ToUnityAxis()) != 0)
        {
          anim.SetBool(IsRunning, true);
          anim.SetBool(Fall, false);
        }
        else
        {
          anim.SetBool(IsRunning, false);
        }

    }

    private IEnumerator StopDashing()
    {
      yield return new WaitForSeconds(dashingTime);
      anim.SetBool(Dash, false);
      _trailRenderer.emitting = false;
      _isDashing = false;
    }

    private void FlipSprite(float inputX)
    {
      if (inputX < _directionValue)
      {
        _yRotation = 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, _yRotation, 0));
      }
      else if (inputX > _directionValue)
      {
        _yRotation = 0;
        transform.rotation = Quaternion.Euler(new Vector3(0, _yRotation, 0));
      }
    }

    public void SetParent(Transform newParent)
    {
      _originalParent = transform.parent;
      transform.parent = newParent;
    }

    public void ResetParent()
    {
      transform.parent = _originalParent;
    }

    public void KnockBack(Transform t)
    {
      anim.SetBool(Damage, true);
      var direction = center.position - t.position;
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

    public IEnumerator UnKnockBack()
    {
      yield return new WaitForSeconds(knockBackTime);
      knockBacked = false;
      anim.SetBool(Damage, false);
    }

    public IEnumerator AttackSprite()
    {
      anim.SetBool(Attack, true);
      _isAttacking = true;
      _spriteRenderer.sprite = attackSprite; 
      yield return new WaitForSeconds(attackTime);
      anim.SetBool(Attack, false);
      _isAttacking = false;
    }
    
  }  
}

