using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// The class that generates a level (by spawning prefabs and aligning them.
/// Once the level is generated, nothing else happens
/// </summary>

public class LevelGenerator : MonoBehaviour
{
    // The start point of the level. Is a transform so designers can adjust it
    public Transform levelStartPoint;
    // The Level Config that the generator gets its rooms from
    public LevelConfig levelConfig;

    // The layermask that defines what a level bound is. Used for raycasts and boxcasts (to check if a chunk will fit)
    public LayerMask levelBoundsLayermask;

    // List of enemies based on levels
    public Dictionary<EnemyClass, LevelConfig.EnemyWeight[]> enemyData;
    public int[] bossModes;
    public int bossModeCount;

    // Should the levels be generated in step through mode (i.e. should it generate one room per frame)
    // It is recommended to run the level generator in step through mode
    public bool stepThroughMode = false;
    private int order;

    // The random number generator that generates the level
    // This variable exists so that level generation can be deterministic
    // Anything involving random number generation MUST use this variable
    Random random;

    // The current position to generate a room from
    Vector2 currentPosition;

    // The direction of the last exit place
    LevelDirection currentDirection;
    // The last exit placed
    LevelExit lastExit;

    // If true, attempt to spawn a shop this frame
    bool needToAttemptSpawnShop = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.Instance.useSeed)
        {
            GameManager.Instance.seed = (int)(System.DateTime.UtcNow.ToFileTimeUtc() % 166384);
        }

        LevelConfig.EnemyItems enemyItems = levelConfig.enemyItems;
        enemyData = new Dictionary<EnemyClass, LevelConfig.EnemyWeight[]> 
        { 
            { EnemyClass.Ground, enemyItems.groundEnemies },
            { EnemyClass.Flying, enemyItems.flyingEnemies}, 
            { EnemyClass.Ranged, enemyItems.rangedEnemies }, 
            { EnemyClass.Boss, enemyItems.boss } 
        };
        bossModes = enemyItems.validBossModeIds;
        bossModeCount = enemyItems.bossModeCount;

        AkSoundEngine.SetRTPCValue("Level", levelConfig.levelID);
        Debug.Log("Level music playing: " + levelConfig.levelID);

        BuildLevel(GameManager.Instance.seed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildLevel(int seed)
    {
        StartCoroutine(GenerateLevel(seed));
    }

    // Encompasses the entire level generation process
    IEnumerator GenerateLevel(int seed)
    {
        EventBus.Publish(new LevelGenerationStartEvent());
        levelConfig.PopulateLists();

        random = new Random(seed);

        yield return StartCoroutine(PlaceLevelChunks());
        EventBus.Publish(new LevelGenerationCompleteEvent());
    }

    // Encompasses the placement of level chunks (i.e. rooms)
    IEnumerator PlaceLevelChunks()
    {
        order = 0;
        currentPosition = levelStartPoint.transform.position;

        // Determine the level length (includes normal rooms, not the start and ened
        int levelLength = random.Next(levelConfig.minLevelLength, levelConfig.maxLevelLength);
        List<int> shopIndices = GenerateShopLocations(levelLength);

        // Place any chunk with type Start
        PlaceChunk(levelConfig.GetChunkOfType(LevelType.Start, ref random), LevelUtils.ALL_DIRECTIONS);
        yield return null;

        for (int i = 0; i < levelLength; i++)
        {
            List<LevelDirection> validDirections = new List<LevelDirection>();
            GameObject levelChunk = ChooseLevelChunk(ref validDirections);
            PlaceChunk(levelChunk, validDirections);

            if (shopIndices.Contains(i) || needToAttemptSpawnShop)
            {
                if (stepThroughMode)
                    yield return null;

                AttemptSpawnShop();
            }

            if (stepThroughMode)
                yield return null;
        }

        GameObject endChunk = levelConfig.GetRandomChunk(chunk =>
            LevelUtils.IsChunkOfType(chunk, LevelType.End) &&
            DoesLevelChunkFit(chunk, LevelUtils.GetOppositeDirection(currentDirection)), ref random);

        // Place any chunk with type End
        PlaceChunk(endChunk, LevelUtils.ALL_DIRECTIONS);

        yield break;
    }

    // Chooses a level chunk. Also computes a list of valid directions for an exi
    // An direction can be invalid if the generator can't generate 3 rooms in that direction
    GameObject ChooseLevelChunk(ref List<LevelDirection> validDirections)
    {
        // Get the valid directions that can be generated
        LevelDirection oppositeDirection = LevelUtils.GetOppositeDirection(currentDirection);
        validDirections = GetValidDirections(oppositeDirection);
        int randIndex = random.Next(validDirections.Count);

        for (int i = 0; i < validDirections.Count; i++)
        {
            LevelDirection nextDirection = validDirections[(randIndex + i) % validDirections.Count];
            // Check the validity of a U-Turn piece
            // (must have two exits in the direction, one for the entrance and one for the exit)
            if (nextDirection == oppositeDirection)
            {
                GameObject chunk = levelConfig.GetChunksWithManyExits(oppositeDirection, 2, ref random);
                if (chunk != null)
                    return chunk;
            }
            // Check for a valid chunk in that direction
            else
            {
                // The level chunk must fit within the existing level structure and must be a standard chunk
                GameObject chunk = levelConfig.GetRandomChunk(chunk => (
                    DoesLevelChunkFit(chunk, oppositeDirection) &&
                    LevelUtils.DoesLevelChunkHaveExits(chunk, nextDirection, oppositeDirection) &&
                    LevelUtils.IsChunkOfType(chunk, LevelType.Standard)), 
                    ref random);
                if (chunk != null)
                    return chunk;
            }
        }

        return null;
    }

    // Place a given prefab
    GameObject PlaceChunk(GameObject prefab, List<LevelDirection> validDirections)
    {
        GameObject spawnedChunk = Instantiate(prefab, transform);
        LevelChunk chunk = spawnedChunk.GetComponent<LevelChunk>();

        LevelExit entrance = null;
        LevelExit exit = null;
        EstablishExits(chunk, validDirections, ref entrance, ref exit);

        lastExit = exit;

        if (chunk.type == LevelType.Start)
        {
            spawnedChunk.transform.position = levelStartPoint.transform.position;
            currentPosition += (Vector2)exit.transform.localPosition;
        }
        else
        {
            // Once spawned, the current position needs to be calculated (for the next level transform)
            // and the chunk needs to be aligned
            Vector2 diffFromCenter = spawnedChunk.transform.position - entrance.transform.position;
            spawnedChunk.transform.position = currentPosition + diffFromCenter;
            if (exit == null)
                currentPosition = entrance.transform.position;
            else
                currentPosition += (Vector2)(exit.transform.position - entrance.transform.position);
        }

        LevelChunk chunkScript = spawnedChunk.GetComponent<LevelChunk>();
        chunkScript.order = order;
        order++;

        return spawnedChunk;
    }

    // Figure out the entrance and exit to the spawned chunk
    // Returns the entrance and exit that were chosen so the chunk can be aligned properly
    void EstablishExits(LevelChunk chunk, List<LevelDirection> validDirections, ref LevelExit entrance, ref LevelExit exit)
    {
        // The entrance of the chunk must be the opposite direction
        // (e.g. a left exit must have a right entrance)
        LevelDirection oppositeDirection = LevelUtils.GetOppositeDirection(currentDirection);
        // If there are no exits, there is a problem
        if (chunk.exits.Count == 0)
        {
            Debug.LogError("Placed chunk has no exits");
        }
        // If there's only one exit, it is both the entrance and exit
        else if (chunk.exits.Count == 1)
        {
            chunk.exits[0].Remove();
            currentDirection = chunk.exits[0].direction;
            exit = chunk.exits[0];
            entrance = chunk.exits[0];
        }
        // If there is only two exits, one is the entrance and one is the exit (default to exit 0)
        else if (chunk.exits.Count == 2)
        {
            if (chunk.type == LevelType.End)
            {
                if (chunk.exits[0].direction == oppositeDirection)
                {
                    chunk.exits[0].Remove();
                    entrance = chunk.exits[0];
                    exit = chunk.exits[0];
                }
                else
                {
                    chunk.exits[1].Remove();
                    entrance = chunk.exits[1];
                    exit = chunk.exits[1];
                }
            }
            else
            {
                chunk.exits[0].Remove();
                chunk.exits[1].Remove();
                if (chunk.exits[0].direction == oppositeDirection)
                {
                    currentDirection = chunk.exits[1].direction;
                    entrance = chunk.exits[0];
                    exit = chunk.exits[1];
                }
                else
                {
                    currentDirection = chunk.exits[0].direction;
                    entrance = chunk.exits[1];
                    exit = chunk.exits[0];
                }
            }
        }
        // If there are more than two, find one randomly
        else
        {
            // Find an entrance such that the chunk will fit into the given layout
            // (i.e. will not overlap another chunk)
            entrance = chunk.GetRandomExit(exit =>
                DoesLevelExitFit(chunk.gameObject, exit, oppositeDirection), ref random);
            if (chunk.type != LevelType.Start)
            {
                entrance.Remove();
            }

            // If there's only a single exit in the opposite direction
            if (chunk.CountExitsInDirection(oppositeDirection) == 1)
                validDirections.Remove(oppositeDirection);

            if (chunk.type != LevelType.End)
            {
                // Find all the invalid exits (i.e. the exits that don't work with the entrance)
                List<LevelExit> invalidExits = entrance.incompatibleExits;
                invalidExits.Add(entrance);

                // Gets a exit that isn't invalid based on the weighting of the level config
                exit = chunk.GetRandomExitWeighted(exit =>
                    (!invalidExits.Contains(exit) && validDirections.Contains(exit.direction)),
                exit => (levelConfig.dirChances[exit.direction]), ref random);

                currentDirection = exit.direction;
                exit.Remove();
            }
        }
    }

    // Test the validity of a direction
    // The generator must be able to place two or more chunks in that direction in a row (to avoid the possibility of getting stuck)
    List<LevelDirection> GetValidDirections(LevelDirection oppositeDirection)
    {
        List<LevelDirection> validDirections = new List<LevelDirection>();
        for(int i = 0; i <= (int)LevelDirection.Right; i++)
        {
            if ((LevelDirection)i == oppositeDirection)
            {
                // Test the viability of a U-Turn Level Chunk
                Vector2 direction = LevelUtils.GetRightDirectionVector(oppositeDirection);
                direction.x *= levelConfig.maxSize.x;
                direction.y *= levelConfig.maxSize.y;

                Vector2 oppositeVector = LevelUtils.GetDirectionVector((LevelDirection)i);
                oppositeVector.x *= levelConfig.maxSize.x;
                oppositeVector.y *= levelConfig.maxSize.y;

                // Test the validity of generating three pieces in a U-turn piece
                if (LevelBoundsRaycast(oppositeVector, 3, direction * 0.5f))
                {
                    // Emsures there is enough room in the left and right directions to actually perform a U-Turn
                    if (LevelBoundsRaycast(direction, 1, Vector2.zero) && LevelBoundsRaycast(-direction, 1, Vector2.zero))
                    {
                        validDirections.Add(oppositeDirection);
                    }
                }
            }
            else
            {
                Vector2 direction = LevelUtils.GetDirectionVector((LevelDirection)i);
                direction.x *= levelConfig.maxSize.x;
                direction.y *= levelConfig.maxSize.y;

                // Test the validity of generating in a direction
                if (LevelBoundsRaycast(direction, 3, Vector2.zero))
                {
                    validDirections.Add((LevelDirection)i);
                }
            }
        }

        return validDirections;
    }

    bool LevelBoundsRaycast(Vector2 direction, float scale, Vector2 originOffset)
    {
        Vector2 origin = lastExit == null ? currentPosition : lastExit.transform.position;

        // This vector should be a magnitude of 10 to ensure the raycast doesn't hit the current tile
        if (lastExit != null)
        {
            origin += LevelUtils.GetDirectionVector(lastExit.direction) * 10;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, direction.magnitude * scale, levelBoundsLayermask.value);

        Debug.DrawLine(origin, origin + direction.normalized + direction * scale, Color.blue);

        if (hit.collider != null)
            return false;
        return true;
    }

    // Is there a valid exit such that the level chunk can fit into the area
    // The box size is slightly smaller than the box to ensure exit fits will not be rejected
    bool DoesLevelChunkFit(GameObject levelChunk, LevelDirection directionAttempting)
    {
        List<LevelExit> exits = levelChunk.GetComponent<LevelChunk>().exits;
        BoxCollider2D box = levelChunk.GetComponent<BoxCollider2D>();
        for(int i = 0; i < exits.Count; i++)
        {
            if (exits[i].direction == directionAttempting)
            {
                // Test the box that the level chunk will occupy
                Vector2 exitOffset = exits[i].transform.localPosition;
                exitOffset.x *= levelChunk.transform.localScale.x;
                exitOffset.y *= levelChunk.transform.localScale.y;

                Vector2 boxCenter = (Vector2)lastExit.transform.position - exitOffset;
                Vector2 boxSize = box.size * 0.85f;

                Collider2D collider = Physics2D.OverlapBox(boxCenter, boxSize, 0, levelBoundsLayermask);
                if (collider == null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Does a specific exit within a level chunk fit if used as an entrance
    bool DoesLevelExitFit(GameObject levelChunk, LevelExit exit, LevelDirection attemptingDirection)
    {
        if (lastExit == null)
            return true;

        if (exit.direction != attemptingDirection)
        {
            return false;
        }

        // Test the box that the level chunk will occupy
        BoxCollider2D box = levelChunk.GetComponent<BoxCollider2D>();
        Vector2 exitOffset = exit.transform.localPosition;
        exitOffset.x *= levelChunk.transform.localScale.x;
        exitOffset.y *= levelChunk.transform.localScale.y;

        Vector2 boxCenter = box.offset + (Vector2)lastExit.transform.position - exitOffset;
        Vector2 boxSize = box.size * 0.85f;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, levelBoundsLayermask);
        if (colliders == null || colliders.Length == 0)
            return true;

        // Ensure that the collider we found isn't ourself
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == (Collider2D)box || colliders[i].gameObject == box.gameObject)
                continue;
            return false;
        }

        return true;
    }

    // Creates the indices after which the shop should be implemented
    List<int> GenerateShopLocations(int levelLength)
    {
        List<int> shopIndices = new List<int>();

        // Shops should not generate as the first or last index
        int minShopIndex = 1;
        int maxShopIndex = levelLength - 1;
        int regionSize = (maxShopIndex - minShopIndex) / levelConfig.numShops;

        for (int i = 0; i < levelConfig.numShops; i++)
        {
            int regionStart = regionSize * i + minShopIndex;
            int randIndex = random.Next(regionSize);
            shopIndices.Add(regionStart + randIndex);
        }

        return shopIndices;
    }

    void AttemptSpawnShop()
    {
        // We need to find a chunk that is a shop and fits
        LevelDirection oppositeDirection = LevelUtils.GetOppositeDirection(currentDirection);
        GameObject shopChunk = levelConfig.GetRandomChunk(chunk =>
            LevelUtils.IsChunkOfType(chunk, LevelType.Shop) &&
            DoesLevelChunkFit(chunk, oppositeDirection), ref random);

        List<LevelDirection> validDirections = GetValidDirections(oppositeDirection);

        // If we can't find one, try again on the next frame
        if (shopChunk == null)
        {
            needToAttemptSpawnShop = true;
        }
        else
        {
            GameObject shop = PlaceChunk(shopChunk, validDirections);
            
            foreach (ShopTrigger trigger in shop.GetComponent<ShopChunk>().shopTriggers)
            {
                trigger.SetIndex(random.Next(166384));
            }

            needToAttemptSpawnShop = false;
        }
    }
}
