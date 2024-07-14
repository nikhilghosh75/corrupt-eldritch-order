using System.Collections.Generic;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using GameObject = UnityEngine.GameObject;

public static class LevelUtils
{
    public static readonly List<LevelDirection> ALL_DIRECTIONS = new List<LevelDirection> { LevelDirection.Up, LevelDirection.Down, LevelDirection.Left, LevelDirection.Right };

    public static bool AreOppositeDirections(LevelDirection d1, LevelDirection d2)
    {
        if (d1 == LevelDirection.Up && d2 == LevelDirection.Down)
            return true;
        if (d1 == LevelDirection.Down && d2 == LevelDirection.Up)
            return true;
        if (d1 == LevelDirection.Left && d2 == LevelDirection.Right)
            return true;
        if (d1 == LevelDirection.Right && d2 == LevelDirection.Left)
            return true;

        return false;
    }

    public static LevelDirection GetRandomDifferentDirection(LevelDirection d, ref Random random)
    {
        if (d == LevelDirection.Up)
        {
            return (LevelDirection)(random.Next(3) + 1);
        }
        else if (d == LevelDirection.Down)
        {
            int rand = random.Next(3);
            return (LevelDirection)(rand == 0 ? 0 : rand + 1);
        }
        else if (d == LevelDirection.Left)
        {
            int rand = random.Next(3);
            return (LevelDirection)(rand == 2 ? 3 : rand);
        }
        else if (d == LevelDirection.Right)
        {
            return (LevelDirection)(random.Next(3));
        }

        return LevelDirection.Up;
    }

    public static LevelDirection GetOppositeDirection(LevelDirection d)
    {
        if (d == LevelDirection.Up)
        {
            return LevelDirection.Down;
        }
        else if (d == LevelDirection.Down)
        {
            return LevelDirection.Up;
        }
        else if (d == LevelDirection.Left)
        {
            return LevelDirection.Right;
        }
        else if (d == LevelDirection.Right)
        {
            return LevelDirection.Left;
        }
        return LevelDirection.Up;
    }

    public static Vector2 GetDirectionVector(LevelDirection direction)
    {
        switch(direction)
        {
            case LevelDirection.Up: return new Vector2(0, 1);
            case LevelDirection.Down: return new Vector2(0, -1);
            case LevelDirection.Left: return new Vector2(-1, 0);
            case LevelDirection.Right: return new Vector2(1, 0);
        }

        return Vector2.zero;
    }

    public static Vector2 GetRightDirectionVector(LevelDirection direction)
    {
        switch (direction)
        {
            case LevelDirection.Up: return new Vector2(1, 0);
            case LevelDirection.Down: return new Vector2(-1, 0);
            case LevelDirection.Left: return new Vector2(0, 1);
            case LevelDirection.Right: return new Vector2(0, -1);
        }

        return Vector2.zero;
    }

    public static bool DoesLevelChunkHaveExits(GameObject levelChunk, LevelDirection direction1, LevelDirection direction2)
    {
        LevelExit exitInDirection1 = null;
        LevelExit exitInDirection2 = null;

        List<LevelExit> exits = levelChunk.GetComponent<LevelChunk>().exits;
        for (int i = 0; i < exits.Count; i++)
        {
            if (exits[i].direction == direction1)
            {
                exitInDirection1 = exits[i];
            }
            if (exits[i].direction == direction2)
            {
                exitInDirection2 = exits[i];
            }
        }

        if (exitInDirection1 && exitInDirection2)
        {
            // If the exits found are incompatible with each other, don't choose this chunk
            if (exitInDirection1.incompatibleExits.Contains(exitInDirection2)
                || exitInDirection2.incompatibleExits.Contains(exitInDirection1))
            {
                return false;
            }
            return true;
        }

        return false;
    }

    public static bool IsChunkOfType(GameObject chunk, LevelType type)
    {
        return chunk.GetComponent<LevelChunk>().type == type;
    }
}
