using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockRegistry : MonoBehaviour
{
    static readonly HashSet<(Vector2Int, Vector2Int)> _unlockedEdges = new();

    public static bool IsUnlocked(Vector2Int cellA, Vector2Int cellB)
    {
        return _unlockedEdges.Contains(SortedEdge(cellA, cellB));
    }
    public static void SetUnlocked(Vector2Int cellA, Vector2Int cellB)
    {
        _unlockedEdges.Add(SortedEdge(cellA, cellB));
    }
    public static void Reset()
    {
        _unlockedEdges.Clear();
    }

    static (Vector2Int, Vector2Int) SortedEdge(Vector2Int a, Vector2Int b)
    {
        if (a.x < b.x || (a.x == b.x && a.y < b.y))
        {
            return (a, b);
        }
        return (b, a);
    }
}
