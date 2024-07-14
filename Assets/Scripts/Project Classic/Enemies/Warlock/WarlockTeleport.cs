using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockTeleport : EnemyAction
{
    public AK.Wwise.Event teleport;

    GameObject target;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.Instance.gameObject;
        anim = GetComponent<Animator>();
    }

    public override void Act()
    {
        Debug.Log("Teleporting!");

        bool valid = false;
        int attempt = 0;
        int maxAttempts = 20;
        Vector2 newPos = transform.position;

        while (!valid && attempt < maxAttempts)
        {
            attempt++;
            float randX = UnityEngine.Random.Range(-18, 18) / 2f;
            float randY = UnityEngine.Random.Range(2, 12) / 2f;

            newPos = new Vector2(target.transform.position.x + randX, target.transform.position.y + randY);

            valid = true;
            if (Physics.Raycast(newPos, Vector3.forward, 100))
            {
                valid = false;
            }
        }

        if (attempt < maxAttempts)
        {
            StartCoroutine(TeleportCoroutine(newPos));
        }
    }

    public override float GetActionTime()
    {
        return 0.5f;
    }

    public override void Interrupt()
    {
        
    }

    IEnumerator TeleportCoroutine(Vector2 newPos)
    {
        anim.SetTrigger("Teleport");
        teleport?.Post(gameObject);
        yield return new WaitForSeconds(.5f);
        transform.position = newPos;
    }
}
