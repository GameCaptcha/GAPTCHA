using UnityEngine;

public class FlappyBirdPlayer : Player
{
    private float _jumpPower = 4;

    private bool _isDie = false;
    private bool _jumpPressed;

    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] KeyDebuff keyDebuff;


    protected override void Init()
    {
        base.Init();
        myRigidbody = GetComponent<Rigidbody2D>();
        _isDie = false;
    }

    public override void Refresh()
    {
        base.Refresh();
        _isDie = false;
    }


    protected override void Space()
    {
        base.Space();
        _jumpPressed = true;
    }

    protected override void FUpdate()
    {
        base.FUpdate();
        if (_jumpPressed)
        {
            myRigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(0, 0, 15f);

            _jumpPressed = false;
        }

        if (myRigidbody.linearVelocity.y < -.5f && myRigidbody.linearVelocity.y > -4f)
        {
            transform.Rotate(0, 0, -1.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isDie = true;

        globalGameManager.GameOver();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

    
        if (keyDebuff != null && keyDebuff.IsActive)
        {
            int mapped = keyDebuff.MappedKey;
            int currentValue = GlobalDatas.ConvertInputIndexeToValue(globalGameManager.actionIndex);

            if (currentValue == mapped)
            {
               
                globalGameManager.actionIndex =
                    GlobalDatas.ConvertInputValueToIndex(InputValue.SPACE);
            }

            //Debug.Log($"[Flappy Input] current={currentValue}, mapped={mapped}");

        }

    }
    public float GetJumpPower()
    {
        return _jumpPower;
    }

    public void SetJumpPower(float value)
    {
        _jumpPower = value;
    }

}
