using System;
using UnityEngine;


public enum GameKind
{
    Dodge,
    FlappyBird,
    PoopAvoid
}

public class GlobalDatas
{
    public static Vector2 screenSize = new Vector2(10.24f, 10.24f);

    public static bool DEBUG_PRINT = false;

    public static Vector2 GetMinScreenBound()
    {
        return screenSize / -2.0f;
    }
    public static Vector2 GetMaxScreenBound()
    {
        return screenSize / 2.0f;
    }

    public static void DebugLog(string log)
    {
        if (DEBUG_PRINT)
        {
            Debug.Log(log);
        }
    }
    public static void DebugLog(int log)
    {
        if (DEBUG_PRINT)
        {
            Debug.Log(log);
        }
    }
    public static void DebugLogError(string log)
    {
        if (DEBUG_PRINT)
        {
            Debug.LogError(log);
        }
    }
    public static void DebugLogError(int log)
    {
        if (DEBUG_PRINT)
        {
            Debug.LogError(log);
        }
    }
}
