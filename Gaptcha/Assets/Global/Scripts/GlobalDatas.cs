using System;
using System.Collections.Generic;
using UnityEngine;


public enum GameKind
{
    Dodge,
    FlappyBird,
    PoopAvoid
}

public class InputValue
{
    public static readonly int NONE = 1;
    public static readonly int LEFT = 2;
    public static readonly int RIGHT = 3;
    public static readonly int UP = 5;
    public static readonly int DOWN = 7;
    public static readonly int SPACE = 100;

    public static readonly List<int> listInputCases = new List<int>()
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        LEFT * RIGHT,
        LEFT * UP,
        LEFT * DOWN,
        RIGHT * UP,
        RIGHT * DOWN,
        UP * DOWN,
        LEFT * RIGHT * UP,
        LEFT * RIGHT * DOWN,
        LEFT * UP * DOWN,
        RIGHT * UP * DOWN,
        LEFT * RIGHT * UP * DOWN,
        SPACE
    };

}

public class GlobalDatas
{
    public static Vector2 screenSize = new Vector2(10.24f, 10.24f);

    public static bool DEBUG_PRINT = false;
    public static bool UPDATE_AI_CONTROL = false;

    public static Vector2 GetMinScreenBound()
    {
        return screenSize / -2.0f;
    }
    public static Vector2 GetMaxScreenBound()
    {
        return screenSize / 2.0f;
    }

    public static void DebugLog(Func<object> logProvider)
    {
        if (DEBUG_PRINT && logProvider != null)
        {
            Debug.Log(logProvider());
        }
    }
    public static void DebugLogError(Func<object> logProvider)
    {
        if (DEBUG_PRINT && logProvider != null)
        {
            Debug.LogError(logProvider());
        }
    }

    public static int ConvertInputValueToIndex(int inputValue)
    {
        int index = 0;

        for(int i = 0; i < InputValue.listInputCases.Count; ++i)
        {
            if (inputValue == InputValue.listInputCases[i])
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public static int ConvertInputIndexeToValue(int index)
    {
        int value = InputValue.listInputCases[index];

        return value;
    }
}
