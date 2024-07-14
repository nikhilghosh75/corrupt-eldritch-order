using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Config", menuName = "Level Config")]
public class LevelConfig : ScriptableObject
{
    public int levelID;

    public int minLevelLength;
    public int maxLevelLength;

    public int numShops;

    public List<GameObject> levelChunks;

    [System.Serializable]
    public struct DirectionChances
    {
        public int upChance;
        public int downChance;
        public int leftChance;
        public int rightChance;
    }

    public DirectionChances directionChances;

    public Dictionary<LevelDirection, int> dirChances =
        new Dictionary<LevelDirection, int>();

    [System.Serializable]
    public struct EnemyWeight
    {
        public GameObject enemy;
        public int probability;
    }

    [System.Serializable]
    public struct EnemyItems
    {
        public EnemyWeight[] groundEnemies, flyingEnemies, rangedEnemies, boss;

        public BossMode bossMode;
        public int[] validBossModeIds;
        public int bossModeCount;
        public int bossAttackCount;
    }

    public EnemyItems enemyItems;

    [Header("Automatically Generated Properties")]
    public Vector2 minSize;
    public Vector2 maxSize;

    HashSet<GameObject> upChunks;
    HashSet<GameObject> downChunks;
    HashSet<GameObject> leftChunks;
    HashSet<GameObject> rightChunks;

    public void PopulateLists()
    {
        upChunks = new HashSet<GameObject>();
        downChunks = new HashSet<GameObject>();
        leftChunks = new HashSet<GameObject>();
        rightChunks = new HashSet<GameObject>();

        dirChances = new Dictionary<LevelDirection, int>
        {
            { LevelDirection.Up, directionChances.upChance },
            { LevelDirection.Down, directionChances.downChance },
            { LevelDirection.Left, directionChances.leftChance },
            { LevelDirection.Right, directionChances.rightChance }
        };

        minSize = Vector2.positiveInfinity;
        maxSize = Vector2.zero;

        for (int i = 0; i < levelChunks.Count; i++)
        {
            LevelChunk chunk = levelChunks[i].GetComponent<LevelChunk>();
            if (chunk.type == LevelType.Start || chunk.type == LevelType.End)
                continue;

            // Populate Dictionaries for Chunks
            for(int j = 0; j < chunk.exits.Count; j++)
            {
                if (chunk.exits[j].direction == LevelDirection.Up)
                    upChunks.Add(levelChunks[i]);
                else if (chunk.exits[j].direction == LevelDirection.Down)
                    downChunks.Add(levelChunks[i]);
                else if (chunk.exits[j].direction == LevelDirection.Left)
                    leftChunks.Add(levelChunks[i]);
                else if (chunk.exits[j].direction == LevelDirection.Right)
                    rightChunks.Add(levelChunks[i]);
            }

            // Set the min and max sizes
            BoxCollider2D collider = levelChunks[i].GetComponent<BoxCollider2D>();
            if (collider.size.x < minSize.x)
                minSize.x = collider.size.x;
            if (collider.size.y < minSize.y)
                minSize.y = collider.size.y;
            if (collider.size.x > maxSize.x)
                maxSize.x = collider.size.x;
            if (collider.size.y > maxSize.y)
                maxSize.y = collider.size.y;
        }

    }

    public GameObject GetChunkOfType(LevelType type, ref System.Random random)
    {
        List<GameObject> validPrefabs = new List<GameObject>();
        for(int i = 0; i < levelChunks.Count; i++)
        {
            if (levelChunks[i].GetComponent<LevelChunk>().type == type)
            {
                validPrefabs.Add(levelChunks[i]);
            }
        }

        return validPrefabs[random.Next(validPrefabs.Count)];
    }

    public GameObject GetChunkWithExit(LevelDirection direction, ref System.Random random)
    {
        if (direction == LevelDirection.Up)
        {
            return upChunks.ElementAt(random.Next(upChunks.Count));
        }
        else if (direction == LevelDirection.Down)
        {
            return downChunks.ElementAt(random.Next(downChunks.Count));
        }
        else if (direction == LevelDirection.Left)
        {
            return leftChunks.ElementAt(random.Next(leftChunks.Count));
        }
        else if (direction == LevelDirection.Right)
        {
            return rightChunks.ElementAt(random.Next(rightChunks.Count));
        }

        return null;
    }

    public GameObject GetChunkWithExits(LevelDirection d1, LevelDirection d2, ref System.Random random)
    {
        HashSet<GameObject> chunks1 = new HashSet<GameObject>();
        HashSet<GameObject> chunks2 = new HashSet<GameObject>();

        if (d1 == LevelDirection.Up)
            chunks1 = upChunks;
        else if (d1 == LevelDirection.Down)
            chunks1 = downChunks;
        else if (d1 == LevelDirection.Left)
            chunks1 = leftChunks;
        else if (d1 == LevelDirection.Right)
            chunks1 = rightChunks;

        if (d2 == LevelDirection.Up)
            chunks2 = upChunks;
        else if (d2 == LevelDirection.Down)
            chunks2 = downChunks;
        else if (d2 == LevelDirection.Left)
            chunks2 = leftChunks;
        else if (d2 == LevelDirection.Right)
            chunks2 = rightChunks;

        HashSet<GameObject> validChunks = new HashSet<GameObject>(chunks1.Intersect(chunks2));

        if (validChunks.Count == 0)
            return null;

        return validChunks.ElementAt(random.Next(validChunks.Count));
    }

    public GameObject GetChunksWithManyExits(LevelDirection d, int numExits, ref System.Random random)
    {
        List<GameObject> validChunks = new List<GameObject>();

        for (int i = 0; i < levelChunks.Count; i++)
        {
            int exitsInChunk = levelChunks[i].GetComponent<LevelChunk>().exits.Count(exit => exit.direction == d);
            if (exitsInChunk >= numExits)
            {
                validChunks.Add(levelChunks[i]);
            }
        }

        if (validChunks.Count == 0)
            return null;

        return validChunks[random.Next(validChunks.Count)];
    }

    public GameObject GetRandomChunk(System.Func<GameObject, bool> isValid, ref System.Random random)
    {
        List<GameObject> validChunks = new List<GameObject>();

        for (int i = 0; i < levelChunks.Count; i++)
        {
            if (isValid(levelChunks[i]))
            {
                validChunks.Add(levelChunks[i]);
            }
        }

        if (validChunks.Count == 0)
            return null;

        return validChunks[random.Next(validChunks.Count)];
    }
}
