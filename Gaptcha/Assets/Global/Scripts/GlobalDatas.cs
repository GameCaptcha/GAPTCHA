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

    private static int aliveScore = 0;
    public static int aliveScore_public = 0;

    private static double aliveTime = 0.0;
    public static double aliveTime_public = 0.0;

    public static Vector2 GetMinScreenBound()
    {
        return screenSize / -2.0f;
    }
    public static Vector2 GetMaxScreenBound()
    {
        return screenSize / 2.0f;
    }

    public static int GetScore()
    {
        return aliveScore;
    }

    public static double GetAliveTime()
    {
        return aliveTime;
    }

    public static void SetScore(int score)
    {
        aliveScore = score;
        aliveScore_public = score;
    }

    public static void SetAliveTime(double score)
    {
        aliveTime = score;
        aliveTime_public = score;
    }

}
