using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBurst : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DoManaBurst());
    }

    IEnumerator DoManaBurst()
    {
        Debug.Log("mana Burst Start");

        yield return new WaitForSeconds(time);

        Debug.Log("Mana Burst Fire");

        EnemyHealth[] enemies = GameObject.FindObjectsByType<EnemyHealth>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (EnemyHealth enemy in enemies)
        {
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                if (spriteRenderer.isVisible)
                {
                    enemy.Damage(200);
                }
            }
        }
        gameObject.SetActive(false);
    }

}
