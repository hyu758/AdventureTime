using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public int heathRestore = 10;
    public Vector3 spinRotationSpeed = new Vector3 (0, 180, 0);
    void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        
        if (damageable != null )
        {
            bool wasHeal = damageable.Heal(heathRestore);
            if (wasHeal)
            {
                Destroy(gameObject);
            }
        }
    }
}
