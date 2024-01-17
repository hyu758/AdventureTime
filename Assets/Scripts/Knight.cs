using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public DetectionZone attackZone;
    // Start is called before the first frame update
    [SerializeField]
    private float walkSpeed = 3f;
    [SerializeField]
    private float walkStopRate = 0.05f;
    private Vector2 WalkDirectionVector = Vector2.right;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;

    public GameObject WallBoxx;
    public enum WalkableDirection { Right, Left}

    private WalkableDirection _walkDirection;
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * (-1), gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    WalkDirectionVector = Vector2.right;

                }
                else if (value == WalkableDirection.Left)
                {
                    WalkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget {
        get {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    // Update is called once per frame
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }
    private void FixedUpdate()
    {
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        if (CanMove)
        {
            if (touchingDirections.IsGrounded && !touchingDirections.IsOnWall)
            {
                if (WalkDirection == WalkableDirection.Left)
                {
                    rb.velocity = new Vector2(walkSpeed * Vector2.left.x, rb.velocity.y);
                }
                if (WalkDirection == WalkableDirection.Right)
                {
                    rb.velocity = new Vector2(walkSpeed * Vector2.right.x, rb.velocity.y);
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }


    }
    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
    }
}
