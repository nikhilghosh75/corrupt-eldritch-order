using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Combat
{
    public class Explode : MonoBehaviour
    {
        public int damage;
        public int range;
        public float lifetime;
        public bool activatedByPlayer = false;

        public LayerMask damageLayers;

        private IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(gameObject);
        }

        private void DoDamage(GameObject target)
        {
            HealthSystem health = target.GetComponent<HealthSystem>();
            if (health)
            {
                health.Damage(damage);
            }
        }

        public void Start()
        {
            Vector2 pos = transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, range, damageLayers);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (!(activatedByPlayer && hitCollider.gameObject.layer == 7))
                {
                    Debug.Log("Hit: " + hitCollider.gameObject.name);
                    DoDamage(hitCollider.gameObject);
                }
            }
            StartCoroutine(DestroySelf());
        }
    }
}

