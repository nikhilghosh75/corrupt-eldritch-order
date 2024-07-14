using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using Random = System.Random;

public enum LevelType
{
    Standard,
    Start,
    End,
    Shop
}

public enum LevelDirection
{
    Up,
    Down,
    Left,
    Right
}

public enum LevelTheme
{
    Default,
    Organic,
    Industrial
}

public class LevelChunk : MonoBehaviour
{
    public LevelType type;
    public LevelTheme theme;

    public Vector2 spawnPoint;
    public Vector2 endPoint;
    public int order;

    public List<LevelExit> exits;

    public LevelExit GetRandomExitInDirection(LevelDirection direction, ref Random random)
    {
        List<LevelExit> validExits = new List<LevelExit>();
        for(int i = 0; i < exits.Count; i++)
        {
            if (exits[i].direction == direction)
                validExits.Add(exits[i]);
        }

        return validExits[random.Next(validExits.Count)];
    }

    public LevelExit GetRandomExitInDirections(List<LevelDirection> directions, ref Random random)
    {
        List<LevelExit> validExits = new List<LevelExit>();
        for (int i = 0; i < exits.Count; i++)
        {
            if (directions.Contains(exits[i].direction))
                validExits.Add(exits[i]);
        }

        return validExits[random.Next(validExits.Count)];
    }

    public LevelExit GetRandomExitNotInDirection(LevelDirection direction, ref Random random)
    {
        List<LevelExit> validExits = new List<LevelExit>();
        for (int i = 0; i < exits.Count; i++)
        {
            if (exits[i].direction != direction)
                validExits.Add(exits[i]);
        }

        return validExits[random.Next(validExits.Count)];
    }

    public LevelExit GetRandomExit(System.Func<LevelExit, bool> isValid, ref Random random)
    {
        List<LevelExit> validExits = new List<LevelExit>();
        for (int i = 0; i < exits.Count; i++)
        {
            if (isValid(exits[i]))
                validExits.Add(exits[i]);
        }

        return validExits[random.Next(validExits.Count)];
    }

    public LevelExit GetRandomExitWeighted(System.Func<LevelExit, bool> isValid, System.Func<LevelExit, int> weight, ref Random random)
    {
        List<LevelExit> validExits = new List<LevelExit>();
        List<int> exitWeights = new List<int>();
        int totalWeight = 0;

        for (int i = 0; i < exits.Count; i++)
        {
            if (isValid(exits[i]))
            {
                validExits.Add(exits[i]);
                totalWeight += weight(exits[i]);
                exitWeights.Add(totalWeight);
            }
        }

        int randIndex = random.Next(totalWeight);
        for (int i = 0; i < exitWeights.Count; i++)
        {
            if (exitWeights[i] > randIndex)
                return validExits[i];
        }

        return null;
    }

    public int CountExitsInDirection(LevelDirection direction)
    {
        return exits.Count(x => x.direction == direction);
    }
}
