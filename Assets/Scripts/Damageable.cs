using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityEvent<int, Vector2> damageableHit;
    Animator animator;

    [SerializeField]
    private int _maxHeath = 100;
    public int MaxHeath
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
    private int _heath = 100;

    public int Heath
    {
        get { return _heath; }
        private set {
            _heath = value;
            if (_heath <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private float invincibilityTimer = 0.5f;
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

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
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
    }
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Heath -= damage;
            isInvincible = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }
        return false;
    }

    public bool Heal(int heathRestore)
    {
        if (IsAlive && Heath < MaxHeath)
        {
            int actualHeath = Mathf.Min(Mathf.Max(MaxHeath - Heath, 0), heathRestore);
            Heath += actualHeath;

            CharacterEvents.characterHealed(gameObject, actualHeath);
            return true;
        }
        return false;
    }
}
