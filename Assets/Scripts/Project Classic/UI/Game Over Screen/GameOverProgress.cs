using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverProgress : MonoBehaviour
{
    public float totalWidth;
    public List<float> levelXPositions;

    public RectTransform progressObject;

    public float progressTime;

    public LayerMask levelLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowProgress()
    {
        progressObject.anchoredPosition = new Vector2(0, progressObject.anchoredPosition.y);
        float finalPosition = GetFinalPosition();

        for(float t = 0; t < progressTime; t += Time.unscaledDeltaTime)
        {
            progressObject.anchoredPosition = new Vector2(Mathf.Lerp(0, finalPosition, t / progressTime), progressObject.anchoredPosition.y);
            yield return null;
        }

        progressObject.anchoredPosition = new Vector2(finalPosition, progressObject.anchoredPosition.y);
    }

    float GetFinalPosition()
    {
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        int numChunks = levelGenerator.GetComponentsInChildren<LevelChunk>().Length;
        int level = levelGenerator.levelConfig.levelID;
        float minPosition = levelXPositions[level - 1];
        float maxPosition = levelXPositions[level];

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = levelLayerMask;
        contactFilter.useTriggers = true;

        Collider2D[] colliders = new Collider2D[10];
        Collider2D playerCollider = PlayerController.Instance.GetComponent<Collider2D>();
        int numColliders = Physics2D.OverlapCollider(playerCollider, contactFilter, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            if (colliders[i].TryGetComponent(out LevelChunk chunk))
            {
                int currentChunk = chunk.order;
                return Mathf.Lerp(minPosition, maxPosition, (float)currentChunk / (float)numChunks);
            }
        }

        return minPosition;
    }
}
