using System.Collections;
using Extensions;
using Enums;
using Interfaces;
using Audio;
using UnityEngine;

namespace Behaviours.Movement.PlayerMovement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
      
    [Header("Movement Configuration")] 
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject groundCheckObject;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float hangTime = .2f;
    [SerializeField] private float jumpBufferLength = .2f;

    [Header("Dashing")] 
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = .1f;
    
    private Vector2 _dashingDir;
    private ICheck _groundCheck;
    private bool _isDashing;
    private int _yRotation;
    private bool _canDash = true;
    private int _jumpsLeft;
    private float _hangCounter;
    private float _jumpBufferCount;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int Dash = Animator.StringToHash("dash");
    private static readonly int Jump = Animator.StringToHash("jump");


    private void Awake()
    {
      _groundCheck   = groundCheckObject.GetComponent<ICheck>();
    }
    

    public bool ApplyDash(TrailRenderer trailRenderer, Rigidbody2D rigidBody2D, Animator anim, AudioManager audioManager)
    {
      DashUp(anim);
      return HandleDash(trailRenderer, rigidBody2D, anim, audioManager);
    }

    
    public void ApplyJump(Rigidbody2D rigidBody2D, bool isAttacking, Animator anim)
    {
      OnGround(rigidBody2D, isAttacking, anim);
      SetAnimation(rigidBody2D, anim);
      HangHandler();
      HandleJumpBuffer();
      JumpHandler(rigidBody2D, anim);
    }

    private void HandleJumpBuffer()
    {
      if (Input.GetButtonDown("Jump"))
      {
        _jumpBufferCount = jumpBufferLength;
      }
      else
      {
        _jumpBufferCount -= Time.deltaTime;
      }
    }

    public void ApplyHorizontalMovement(Rigidbody2D rigidBody2D, int directionValue, bool knockBacked, Animator anim)
    {
      
      var inputX = Input.GetAxisRaw(Axis.X.ToUnityAxis());
        
      var newXVelocity = 
        knockBacked ? Mathf.Lerp(rigidBody2D.velocity.x, 0f, Time.deltaTime * 3) : inputX * walkSpeed;
        
      FlipSprite(inputX,  directionValue);
        
      rigidBody2D.SetVelocity(Axis.X, newXVelocity);
      
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


    private static void SetAnimation(Rigidbody2D rigidBody2D, Animator anim)
    {
      if (!(rigidBody2D.velocity.y < -.1f)) return;
      anim.SetBool(Jump, false);
      anim.SetBool(Fall, true);
    }

    private void OnGround(Rigidbody2D rigidBody2D, bool isAttacking, Animator anim)
    {
      if (!IsGrounded() || !(rigidBody2D.velocity.y <= 0) || isAttacking) return;
      anim.SetBool(Jump, false);
      anim.SetBool(Fall, false);
      _jumpsLeft = maxJumps;
    }

    private void JumpHandler(Rigidbody2D rigidBody2D, Animator anim)
    {
      if (_jumpBufferCount >= 0 && _jumpsLeft >= 0 && _hangCounter > 0f)
      {
        rigidBody2D.SetVelocity(Axis.Y, jumpForce);
        _jumpBufferCount = 0;
        _jumpsLeft--;
        anim.SetBool(Jump, true);
        anim.SetBool(Fall, false);
      }

      if (Input.GetButtonUp("Jump") && rigidBody2D.velocity.y > 0)
      {
        rigidBody2D.SetVelocity(Axis.Y, jumpForce * .5f);
        _jumpsLeft--;
        anim.SetBool(Jump, true);
        anim.SetBool(Fall, false);
      }
    }

    private void HangHandler()
    {
      if (IsGrounded())
      {
        _hangCounter = hangTime;
      }
      else
      {
        _hangCounter -= Time.deltaTime;
      }
    }

    private bool IsGrounded()
    {
      return _groundCheck.Check();
    }
    
    private bool HandleDash(TrailRenderer trailRenderer, Rigidbody2D rigidBody2D, Animator anim, AudioManager audioManager)
    {
      if (Input.GetButtonDown("Dash") && _canDash)
      {
        audioManager.PlaySound(0);
        _isDashing = true;
        _canDash = false;
        trailRenderer.emitting = true;

        _dashingDir = new Vector2(Input.GetAxisRaw(Axis.X.ToUnityAxis()), Input.GetAxisRaw(Axis.Y.ToUnityAxis()));
        if (_dashingDir == Vector2.zero)
        {
          _dashingDir = transform.rotation.y < 0
            ? new Vector2(-transform.localScale.x, 0)
            : new Vector2(transform.localScale.x, 0);
        }

        StartCoroutine(StopDashing(trailRenderer, anim));
      }

      if (_isDashing)
      {
        if (!IfJump(trailRenderer, anim)) return false;

        rigidBody2D.velocity = _dashingDir * dashingVelocity;
        return true;
      }

      if (IsGrounded())
      {
        _canDash = true;
      }

      return false;
    }

    
    private IEnumerator StopDashing(TrailRenderer trailRenderer, Animator anim)
    {
      yield return new WaitForSeconds(dashingTime);
      DisableDash(trailRenderer, anim);
    }

    private void DisableDash(TrailRenderer trailRenderer, Animator anim)
    {
      anim.SetBool(Dash, false);
      trailRenderer.emitting = false;
      _isDashing = false;
    }

    private void FlipSprite(float inputX, int directionValue)
    {
      
      if (inputX < directionValue)
      {
        _yRotation = 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, _yRotation, 0));
      }
      else if (inputX > directionValue)
      {
        _yRotation = 0;
        transform.rotation = Quaternion.Euler(new Vector3(0, _yRotation, 0));
      }
    }
    
    private void DashUp(Animator anim)
    {
      if (Input.GetButtonDown("Dash") && Input.GetKey(KeyCode.W) && _canDash)
      {
        anim.SetBool(IsRunning, false);
        anim.SetBool(Fall, false);
        anim.SetBool(Jump, true);
        anim.SetBool(Dash, false);
      }
      else if (Input.GetButtonDown("Dash") && _canDash)
      {
        anim.SetBool(IsRunning, false);
        anim.SetBool(Fall, false);
        anim.SetBool(Jump, false);
        anim.SetBool(Dash, true);
      }
    }

    private bool IfJump(TrailRenderer trailRenderer, Animator anim)
    {
      if (!Input.GetButtonDown("Jump") || !IsGrounded()) return true;
      DisableDash(trailRenderer, anim);
      _canDash = true;
      return false;
    }
    
    }  
}

