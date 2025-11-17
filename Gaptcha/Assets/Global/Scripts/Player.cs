using UnityEngine;

public class Player : SpriteBehaviour
{
    [SerializeField] protected GlobalGameManager globalGameManager;
    [SerializeField] protected SpriteRenderer spriteRender;

    protected float speed;
    protected Vector3 refreshPosition;

    protected Vector2 minBound;
    protected Vector2 maxBound;

    protected int movePosX = 0;
    protected int movePosY = 0;

    float spaceElapsedTime = 0;
    const float spaceDelayTime = 0.15f;


    void Awake()
    {
        minBound = GlobalDatas.GetMinScreenBound();
        maxBound = GlobalDatas.GetMaxScreenBound();

        Init();
    }

    protected virtual void Init()
    {

    }
    public virtual void Refresh()
    {
        transform.localPosition = refreshPosition;
    }

    protected override void FUpdate()
    {
        base.FUpdate();
        InputUpdate();
        Move();

        spaceElapsedTime -= Time.fixedDeltaTime;
    }

    protected virtual void InputUpdate()
    {
        GlobalDatas.DebugLog(() => "Player InputUpdate: actionIndex=" + globalGameManager.actionIndex);
        movePosX = 0;
        movePosY = 0;

        if(globalGameManager.actionIndex == globalGameManager.currentJumpActionIndex)
        {
            if (spaceElapsedTime <= 0)
            {
                Space();
                spaceElapsedTime = spaceDelayTime;
            }
        }
        else
        {
            int inputValue = GlobalDatas.ConvertInputIndexeToValue(globalGameManager.actionIndex);

            if (inputValue % InputValue.LEFT == 0)
            {
                movePosX += -1;
            }
            if (inputValue % InputValue.RIGHT == 0)
            {
                movePosX += 1;
            }
            if (inputValue % InputValue.UP == 0)
            {
                movePosY += 1;
            }
            if (inputValue % InputValue.DOWN == 0)
            {
                movePosY += -1;
            }
        }

    }
    protected virtual void Move() { }
    protected virtual void Space() { }

    protected void CheckPlayableBound()
    {
        Vector3 p = transform.localPosition;
        float halfW = spriteRender.bounds.extents.x;
        float halfH = spriteRender.bounds.extents.y;
        p.x = Mathf.Clamp(p.x, minBound.x + halfW, maxBound.x - halfW);
        p.y = Mathf.Clamp(p.y, minBound.y + halfH, maxBound.y - halfH);
        transform.localPosition = p;
    }
    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float value)
    {
        GlobalDatas.DebugLog(() => "PLAYER SPEED CHANGED TO " + value);
        speed = value;
    }
}
