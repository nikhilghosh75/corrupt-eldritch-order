using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDamageOnCollide : MonoBehaviour
{
    public string damageSource;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController controller))
        {
            PlayerHealth playerHealth = controller.GetComponent<PlayerHealth>();
            if (playerHealth.CanBeDamaged())
            {
                ProgressionManager.instance.RecordDamage(damageSource);
            }
        }
    }
}
