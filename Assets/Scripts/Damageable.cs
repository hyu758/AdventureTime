using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;

    [SerializeField]
    private float _maxHeath = 100;
    public float MaxHeath
    {
        get
        {
            return _maxHeath;
        }
        private set
        {
            _maxHeath = value;
        }
    }
    [SerializeField]
    private float _heath = 100;

    public float Heath
    {
        get { return _heath; }
        private set {
            _heath = value;
            if (_heath < 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;
    private float invincibilityTimer = 0.25f;
    private float timeSinceHit = 0;

    public bool IsAlive{
        get{
            return _isAlive;
        }
        private set {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive: " + value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTimer)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit+= Time.deltaTime;
        }
        Debug.Log(Heath);
        Hit(10);
    }
    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Heath -= damage;
            isInvincible = true;
        }
    }
}
