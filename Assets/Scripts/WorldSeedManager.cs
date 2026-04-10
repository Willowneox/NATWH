using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class WorldSeedManager : MonoBehaviour
{
    const string KEY_SEED = "WorldSeed";
    const string KEY_DAY = "CurrentDay";

    const int LARGE_PRIME_1 = 73856093;
    const int LARGE_PRIME_2 = 19349663;

    public static int WorldSeed { get; private set; }
    public static int CurrentDay { get; private set; }
    
    // Initialize a global seed for the run
    public static void Initialize()
    {
        WorldSeed = DateTime.UtcNow.GetHashCode() ^ UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        CurrentDay = 0;

        PlayerPrefs.SetInt(KEY_SEED, WorldSeed);
        PlayerPrefs.SetInt(KEY_DAY, CurrentDay);
        PlayerPrefs.Save();

        Debug.Log($"[WorldSeed] New run started. Seed: {WorldSeed}");
    }


    public static void AdvanceDay()
    {
        CurrentDay++;
        PlayerPrefs.SetInt(KEY_DAY, CurrentDay);
        PlayerPrefs.Save();
    }

    // Derived seeds
    public static int GetLayoutSeed(int x, int y) => HashCombine(x * 73856093, y * 19349663, WorldSeed);
    public static int GetDailySeed(int x, int y) => HashCombine(x * 73856093, y * 19349663, CurrentDay * 12582917);
    public static int GetEdgeSeed(Vector2Int a, Vector2Int b)
    {
        int ax = Mathf.Min(a.x, b.x), ay = Mathf.Min(a.y, b.y);
        int bx = Mathf.Max(a.x, b.x), by = Mathf.Max (a.y, b.y);

        return HashCombine(ax * 73856093, ay * 19349663, bx * 83492791, by * 49979693, WorldSeed);
    }

    // Hash helper functions
    static int HashCombine(params int[] values)
    {
        int hash = 17;
        foreach (int v in values) hash = hash * 31 + v;
        return hash;
    }

    static int HashCombine(int a, int b, int c, long d)
    {
        return HashCombine(a, b, c, (int)(d & 0x7FFFFFFF));
    }

    static int HashCombine(int a, int b, int c, int d, int e)
    {
        int hash = 17;
        hash = hash * 31 + a;
        hash = hash * 31 + b;
        hash = hash * 31 + c;
        hash = hash * 31 + d;
        hash = hash * 31 + e;
        return hash;
    }
}
