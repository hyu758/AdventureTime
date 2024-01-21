using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    Collider2D attackCollider;
    public GameObject knightStatus;
    [SerializeField]
    private int attackDamage = 10;

    public Vector2 knockBack;

    private float preVal;
    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
        knockBack = new Vector2(5, 2);
        preVal = knightStatus.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (knightStatus.transform.localScale.x != preVal)
        {
            knockBack = new Vector2(knockBack.x*-1, knockBack.y);
            preVal = knightStatus.transform.localScale.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            // Hit the target
            Debug.Log(transform.localScale.x);
            bool gotHit = damageable.Hit(attackDamage, knockBack);
            if (gotHit) Debug.Log(collision.name + "hit for " + attackDamage);   
        }
    }
}
