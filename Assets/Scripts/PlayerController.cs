using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float jumpForce1 = 6f;
    [SerializeField]
    private float jumpForce2 = 6f;
    [SerializeField]
    private float runJump = 9f;
    [SerializeField]
    private int JumpRemaining;

    [SerializeField]
    private float airWalkSpeed = 3f;
    public bool _isFacingLeft = false;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }
    public bool CanMove
    { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public bool IsFacingLeft
    {
        get { return _isFacingLeft; }
        private set
        {
            if (_isFacingLeft != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingLeft = value;
        }
    }
    

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else return walkSpeed;
                    }
                    else return airWalkSpeed;
                }
                else return 0;
            }
            else return 0;
        }
    }
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get { return _isRunning;}
        private set 
        { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (!damageable.LockVelocity && CanMove)
        {
            if (touchingDirections.IsGrounded) JumpRemaining = 1;
            if (!touchingDirections.IsOnWall)
            {
                // Only update the velocity if the player is not on a wall
                //if (IsRunning)
                //{
                //    rb.velocity.y = 0;
                //}
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }
            else
            {
                // If the player is on a wall, set horizontal velocity to 0
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }


        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);


    }

    // Update is called once per frame

    private void Update()
    {
        
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

        
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x < 0 && IsFacingLeft)
        {
            // Face the left
            IsFacingLeft = false;
        }
        else if (moveInput.x > 0 && !IsFacingLeft)
        {
            //Face the right
            IsFacingLeft = true;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (touchingDirections.IsGrounded)
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;
            }
        }
        else if (context.canceled) 
        {
            IsRunning = false;
        }
       
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (JumpRemaining < 1) return;
        if (context.started)
        {
            Jumping();
        }
    }
    private void Jumping()
    {
        
        if (JumpRemaining == 1)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce1);
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            
        }
        else if (JumpRemaining == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce2);
            animator.SetTrigger(AnimationStrings.jumpTrigger);
        }
        --JumpRemaining;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);

    }
}
