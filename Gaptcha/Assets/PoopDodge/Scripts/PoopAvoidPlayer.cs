using UnityEngine;

public class PoopAvoidPlayer : MonoBehaviour
{
    [SerializeField] PoopAvoidManager poopAvoidManager;
    [SerializeField] GlobalGameManager globalGameManager;

    public float moveSpeed = 6f;

    Vector2 minBound;

    [SerializeField]
    Vector2 maxBound;

    [SerializeField]
    SpriteRenderer spriteRender;

    bool isInverted = false;


    void Awake()
    {
        minBound = GlobalDatas.GetMinScreenBound();
        maxBound = GlobalDatas.GetMaxScreenBound();
    }

    void Update()
    {
        float x = 0;
        if (globalGameManager.actionIndex == 0)
        {
            x = 0;
        } else if (globalGameManager.actionIndex == 1)
        {
            x = -1;
        } else if (globalGameManager.actionIndex == 2)
        {
            x = 1;
        }

        if (isInverted)
        {
            x *= -1;
        }

        Vector3 dir = new Vector3(x, 0f, 0f).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

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
