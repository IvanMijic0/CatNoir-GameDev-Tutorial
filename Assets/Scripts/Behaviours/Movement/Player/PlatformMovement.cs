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
    [SerializeField] private float dashingVelocity = 5f;
    [SerializeField] private float dashingTime = .2f;

    [Header("Knock-back")] 
    [SerializeField] private Transform center;
    [SerializeField] private float knockBackVelocity = 5f;
    [SerializeField] private float knockBackTime = .2f;
    [SerializeField] private bool knockBacked;

    [Header("Animation Sprites")] 
    [SerializeField] private Sprite jumpSprite;
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
    private string currentState;

      const string Idle = "Player_Idle";
      const string  Run = "Player_Run";
      const string  Jump = "Player_Jump";
      const string Fall = "Player_Fall";
      const string  Dash = "Player_Dash";
      const string Attack = "Player_Attack";
      const string TakeDamage = "Player_TakeDamage";
      const string  Defeated = "Player_Defeated";

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
      //_dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
      if (Input.GetButtonDown("Dash") && _canDash)
      {
        audioManager.PlaySound(0);
        ChangeAnimationState(Dash);
        anim.SetBool("dash", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("fall", false);
        anim.SetBool("jump", false);
        anim.SetBool("idle", false);
        _isDashing = true;
        _canDash = false;
        _trailRenderer.emitting = true;
        _dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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

        ChangeAnimationState(Idle);
        anim.SetBool("fall", false);
        anim.SetBool("jump", false);
      }

      if (_rigidbody2D.velocity.y < -.1f)
      {
        ChangeAnimationState(Fall);
        anim.SetBool("fall", true);
        anim.SetBool("jump", false);
      }
    
      if (Input.GetButtonDown("Jump") && _jumpsLeft > 0)  
      {
        _rigidbody2D.SetVelocity(Axis.Y, jumpForce);
        //_spriteRenderer.sprite = jumpSprite;
        _jumpsLeft--;

        ChangeAnimationState(Jump);
        anim.SetBool("jump", true);
        anim.SetBool("fall", false);
      }
      
    }

    private bool IsGrounded()
    {
      return _groundCheck.Check();
    }

    private void ApplyHorizontalMovement()
    {
      var inputX = Input.GetAxisRaw(Axis.X.ToUnityAxis());
      var newXVelocity = knockBacked ? Mathf.Lerp(_rigidbody2D.velocity.x, 0f, Time.deltaTime * 3) : inputX * walkSpeed;
      
      FlipSprite(inputX);
      _rigidbody2D.SetVelocity(Axis.X, newXVelocity);
      ApplyDash();

      
        if(newXVelocity != 0f && IsGrounded() && Input.GetAxisRaw(Axis.X.ToUnityAxis()) != 0)
        {
          ChangeAnimationState(Run);
          anim.SetBool("isRunning", true);
          anim.SetBool("fall", false);
        }
        else
        {
          ChangeAnimationState(Idle);
          anim.SetBool("isRunning", false);
        }
    }

    void ChangeAnimationState(string newState)
    {
      //stop the animation from interrupting itself
      if(currentState == newState) return;

      //play the animation
      anim.Play(newState);
      currentState = newState;
    }

    private IEnumerator StopDashing()
    {
      yield return new WaitForSeconds(dashingTime);
      anim.SetBool("dash", false);
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
      anim.SetBool("damage", true);
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
      anim.SetBool("damage", false);
    }

    public IEnumerator AttackSprite()
    {
      ChangeAnimationState(Attack);
      anim.SetBool("attack", true);
      _isAttacking = true;
      _spriteRenderer.sprite = attackSprite; 
      yield return new WaitForSeconds(attackTime);
      ChangeAnimationState(Idle);
      anim.SetBool("attack", false);
      _isAttacking = false;
    }
    
  }  
}

