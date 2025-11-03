using UnityEngine;

public class GameModifier : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public PoopSpawner spawner;
    public Camera mainCamera;
    public PoopAvoidPlayer playerController;

    // 랜덤으로 선택될 변형 타입
    enum Mode { ColorChange, SpeedChange, CameraFlip, ControlInvert }
    Mode activeMode;

    // 타이머
    float timer = 0f;
    float nextActionTime = 0f;

    // 제어 반전 관련
    bool isControlInverted = false;
    float invertEndTime = 0f;

    void Start()
    {
        // 게임 시작 시 랜덤으로 모드 선택
        activeMode = (Mode)Random.Range(0, 4);

        Debug.Log($"[GameModifier] Selected Mode: {activeMode}");
    }

    void Update()
    {
        float t = Time.time;

        // 현재 모드에 따라 작동 주기 결정
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

        // 방향 반전 종료 처리
        if (isControlInverted && Time.time > invertEndTime)
        {
            isControlInverted = false;
            playerController.SetInverted(false);
        }
    }

    // === 기능별 메서드 ===

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
