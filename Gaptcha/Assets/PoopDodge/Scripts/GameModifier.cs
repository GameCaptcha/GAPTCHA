using Unity.MLAgents;
using UnityEngine;

public class GameModifier : UpdateBehaviour
{
    public SpriteRenderer playerRenderer;
    public PoopSpawner spawner;
    public Camera mainCamera;
    public PoopAvoidPlayer playerController;

    enum Mode { ColorChange, SpeedChange, CameraFlip, ControlInvert }
    Mode activeMode;

    float timer = 0f;
    float nextActionTime = 0f;

    bool isControlInverted = false;
    float invertEndTime = 0f;

    void Start()
    {
        enabled = false;
        return;
        activeMode = (Mode)Random.Range(0, 4);

        GlobalDatas.DebugLog(() => $"[GameModifier] Selected Mode: {activeMode}");
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        float t = Time.time;

        switch (activeMode)
        {
            case Mode.ColorChange:
                if (t - timer > 5f)
                {
                    timer = t;
                    RandomizeColors();
                }
                break;

            case Mode.SpeedChange:
                if (t - timer > 5f)
                {
                    timer = t;
                    if (spawner != null)
                        spawner.poopSpeed = Random.Range(2f, 6f);
                }
                break;

            case Mode.CameraFlip:
                if (t - timer > 10f)
                {
                    timer = t;
                    FlipCamera();
                }
                break;

            case Mode.ControlInvert:
                if (!isControlInverted && t - timer > 7.5f)
                {
                    timer = t;
                    StartInvertControl(3f);
                }
                break;
        }

        if (isControlInverted && Time.time > invertEndTime)
        {
            isControlInverted = false;
            playerController.SetInverted(false);
        }
    }

    void RandomizeColors()
    {
        if (playerRenderer != null)
            playerRenderer.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.8f, 1f);

        var poops = FindObjectsOfType<Poop>();
        foreach (var p in poops)
        {
            var sr = p.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.8f, 1f);
        }
    }

    void FlipCamera()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera != null)
            mainCamera.transform.Rotate(0f, 0f, 180f);
    }

    void StartInvertControl(float duration)
    {
        isControlInverted = true;
        invertEndTime = Time.time + duration;
        playerController.SetInverted(true);
    }
}
