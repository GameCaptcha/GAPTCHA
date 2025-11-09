using UnityEngine;

public class PoopAvoidPlayer : MonoBehaviour
{
    [SerializeField] PoopAvoidManager poopAvoidManager;

    [Header("이동")]
    public float moveSpeed = 6f;

    [Header("플레이 범위(월드 좌표)"), SerializeField]
    Vector2 minBound;

    [SerializeField]
    Vector2 maxBound;

    [SerializeField]
    SpriteRenderer spriteRender;

    bool isInverted = false; // ← 방향 반전 여부를 저장


    void Awake()
    {
        minBound = GlobalDatas.GetMinScreenBound();
        maxBound = GlobalDatas.GetMaxScreenBound();
    }

    void Update()
    {
        float x = 0;
        if (GlobalGameManager.Instance.actionIndex == 0)
        {
            x = 0;
        } else if (GlobalGameManager.Instance.actionIndex == 1)
        {
            x = -1;
        } else if (GlobalGameManager.Instance.actionIndex == 2)
        {
            x = 1;
        }

        // GameModifier에서 방향 반전이 켜지면 입력을 반대로
        if (isInverted)
        {
            x *= -1;
        }

        Vector3 dir = new Vector3(x, 0f, 0f).normalized;


        // 이동
        transform.position += dir * moveSpeed * Time.deltaTime;

        // 화면 밖으로 나가지 않게 위치 제한
        Vector3 p = transform.position;
        float halfW = spriteRender.bounds.extents.x;
        float halfH = spriteRender.bounds.extents.y;
        p.x = Mathf.Clamp(p.x, minBound.x + halfW, maxBound.x - halfW);
        p.y = Mathf.Clamp(p.y, minBound.y + halfH, maxBound.y - halfH);
        transform.position = p;
    }

    public void SetInverted(bool value)
    {
        isInverted = value;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Poop"))
        {
            poopAvoidManager.OnPlayerHit();
        }
    }
}
