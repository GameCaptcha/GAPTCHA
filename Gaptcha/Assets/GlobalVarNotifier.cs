using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class GlobalVarNotifier : MonoBehaviour
{
    public float timescale;
    [Header("Scaled (Update)")]
    public float updateDeltaAvg1Sec;
    public float fpsBasedUpdateDelta;
    public int updatePer1Sec;

    [Header("Scaled (FixedUpdate)")]
    public float fixedDeltaAvg1Sec;
    public float fixedFpsBasedUpdateDelta;
    public int fixedUpdatesPer1Sec;

    [Header("Unscaled (Update)")]
    public float unscaledUpdateDeltaAvg1Sec;
    public float unscaledFpsBasedUpdateDelta;
    public int unscaledUpdatePer1Sec;

    [Header("Unscaled (FixedUpdate)")]
    public float fixedUnscaledDeltaAvg1Sec;
    public float fixedUnscaledFpsBasedUpdateDelta;
    public int fixedUnscaledUpdatesPer1Sec;

    // Update accumulators (scaled)
    private double accUpdateDelta = 0;
    private int cntUpdate = 0;

    // Update accumulators (unscaled)
    private double accUpdateUnscaledDelta = 0;
    private int cntUpdateUnscaled = 0;

    // FixedUpdate accumulators (scaled)
    private double accFixedDelta = 0;
    private int cntFixed = 0;

    // FixedUpdate accumulators (unscaled)
    private double accFixedUnscaledDelta = 0;
    private int cntFixedUnscaled = 0;

    void Update()
    {
        this.timescale = Time.timeScale; // report current time scale
        // scaled
        accUpdateDelta += Time.deltaTime;
        cntUpdate++;
        if (accUpdateDelta > 1.0)
        {
            updateDeltaAvg1Sec = (float)(accUpdateDelta / cntUpdate);
            fpsBasedUpdateDelta = 1f / Mathf.Max(1e-6f, updateDeltaAvg1Sec);
            updatePer1Sec = cntUpdate;
            accUpdateDelta = 0;
            cntUpdate = 0;
        }

        // unscaled
        accUpdateUnscaledDelta += Time.unscaledDeltaTime;
        cntUpdateUnscaled++;
        if (accUpdateUnscaledDelta > 1.0)
        {
            unscaledUpdateDeltaAvg1Sec = (float)(accUpdateUnscaledDelta / cntUpdateUnscaled);
            unscaledFpsBasedUpdateDelta = 1f / Mathf.Max(1e-6f, unscaledUpdateDeltaAvg1Sec);
            unscaledUpdatePer1Sec = cntUpdateUnscaled;
            accUpdateUnscaledDelta = 0;
            cntUpdateUnscaled = 0;
        }
    }

    void FixedUpdate()
    {
        // scaled
        accFixedDelta += Time.fixedDeltaTime;
        cntFixed++;
        if (accFixedDelta > 1.0)
        {
            fixedDeltaAvg1Sec = (float)(accFixedDelta / cntFixed);
            fixedFpsBasedUpdateDelta = 1f / Mathf.Max(1e-6f, fixedDeltaAvg1Sec);
            fixedUpdatesPer1Sec = cntFixed;
            accFixedDelta = 0;
            cntFixed = 0;
        }

        // unscaled (use the real-time fixed step)
        float fixedUnscaledDt = Time.fixedUnscaledDeltaTime;
        accFixedUnscaledDelta += fixedUnscaledDt;
        cntFixedUnscaled++;
        if (accFixedUnscaledDelta > 1.0)
        {
            fixedUnscaledDeltaAvg1Sec = (float)(accFixedUnscaledDelta / cntFixedUnscaled);
            fixedUnscaledFpsBasedUpdateDelta = 1f / Mathf.Max(1e-6f, fixedUnscaledDeltaAvg1Sec);
            fixedUnscaledUpdatesPer1Sec = cntFixedUnscaled;
            accFixedUnscaledDelta = 0;
            cntFixedUnscaled = 0;
        }
    }
}
