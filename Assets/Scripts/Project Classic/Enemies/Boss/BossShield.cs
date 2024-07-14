using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    // Reference to boss game object
    [HideInInspector]
    public BossBehavior boss;
    float startingAlpha;

    // Start is called before the first frame update
    void OnEnable()
    {
        boss = FindObjectOfType<BossBehavior>();
        startingAlpha = GetComponent<SpriteRenderer>().color.a;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = boss.transform.position;
    }

    public void SetShieldActive()
    { 
        GetComponent<CircleCollider2D>().enabled = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startingAlpha);
    }

    public void SetShieldInactive()
    {
        // Destroy self
        GameObject.Destroy(this.gameObject);
    }
}
