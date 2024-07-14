using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    public bool damagesPlayer = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>() && damagesPlayer)
        {
            other.GetComponent<PlayerHealth>().Damage(damage);
        }
    }
}
