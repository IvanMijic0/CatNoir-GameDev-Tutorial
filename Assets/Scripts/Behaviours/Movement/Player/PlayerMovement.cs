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

    [Header("Dashing")] 
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = .1f;
    
    private Vector2 _dashingDir;
    private bool _isDashing;
    private ICheck _groundCheck;
    private int _yRotation;
    private bool _canDash = true;
    private int _jumpsLeft;

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
      if(Input.GetButtonDown("Dash") && Input.GetKey(KeyCode.W) && _canDash){
        anim.SetBool(IsRunning, false);
        anim.SetBool(Fall, false);
        anim.SetBool(Jump, true);
        anim.SetBool(Dash, false);
      }
      else if(Input.GetButtonDown("Dash") && _canDash){
          anim.SetBool(IsRunning, false);
          anim.SetBool(Fall, false);
          anim.SetBool(Jump, false);
          anim.SetBool(Dash, true);
      }

      if (Input.GetButtonDown("Dash") && _canDash)
      {
        audioManager.PlaySound(0);
        
        _isDashing = true;
        _canDash = false;
        trailRenderer.emitting = true;
        _dashingDir = new Vector2(Input.GetAxisRaw(Axis.X.ToUnityAxis()), Input.GetAxisRaw(Axis.Y.ToUnityAxis()));
        if (_dashingDir == Vector2.zero)
        {
          _dashingDir = transform.rotation.y < 0 ? new Vector2(-transform.localScale.x, 0) : new Vector2(transform.localScale.x, 0);
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

    private bool IfJump(TrailRenderer trailRenderer, Animator anim)
    {
      if (!Input.GetButtonDown("Jump") || !IsGrounded()) return true;
      DisableDash(trailRenderer, anim);
      _canDash = true;
      return false;
    }

    public void ApplyJump(Rigidbody2D rigidBody2D, bool isAttacking, Animator anim)
    {
      if (IsGrounded() && rigidBody2D.velocity.y <= 0 && !isAttacking){
        anim.SetBool(Jump, false);
        anim.SetBool(Fall, false);
        _jumpsLeft = maxJumps;
      }

      if (rigidBody2D.velocity.y < -.1f){
        anim.SetBool(Jump, false);
        anim.SetBool(Fall, true);
      }

      if (!Input.GetButtonDown("Jump") || _jumpsLeft <= 0) return;
      rigidBody2D.SetVelocity(Axis.Y, jumpForce);
      _jumpsLeft--;
      anim.SetBool(Jump, true);
      anim.SetBool(Fall, false);
    }

    public bool IsGrounded()
    {
      return _groundCheck.Check();
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
    
    }  
}

