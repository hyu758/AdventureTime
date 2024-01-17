using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    Collider2D touchingCol;

    Animator animator;
    public ContactFilter2D castFilter;

    public GameObject footBoxPos;
    public Vector2 footBoxSize;
    public LayerMask groundLayer;

    //Wall Check
    public GameObject wallBoxPos;
    public Vector2 wallBoxSize;
    public LayerMask wallLayer;


    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    [SerializeField]
    private bool _isGrounded;
    public float ceilingDistance = 0.05f;
    public bool IsGrounded { 
        get
        {
            return _isGrounded;
        } 
        private set {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }
    [SerializeField]
    private bool _isOnWall;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

    }


    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapBox(footBoxPos.transform.position, footBoxSize, 0f, groundLayer);
        IsOnWall = Physics2D.OverlapBox(wallBoxPos.transform.position, wallBoxSize, 0f, wallLayer);
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(footBoxPos.transform.position, footBoxSize);
        Gizmos.DrawCube(wallBoxPos.transform.position, wallBoxSize);
    }
}
