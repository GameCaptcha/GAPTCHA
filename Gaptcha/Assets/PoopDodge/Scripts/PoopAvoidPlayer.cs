using UnityEngine;

public class PoopAvoidPlayer : Player
{
    bool isInverted = false;

    protected override void Init()
    {
        base.Init();
        speed = 5.5f;
        refreshPosition = new Vector3(0, -4.120f, 0);
    }


    override protected void InputUpdate()
    {
        base.InputUpdate();

        if (isInverted)
        {
            movePosX *= -1;
        }
    }
    protected override void Move()
    {
        base.Move();

        Vector3 dir = new Vector3(movePosX, 0f, 0f).normalized;

        transform.localPosition += dir * speed * Time.fixedDeltaTime;
         
        CheckPlayableBound();
    }

    public void SetInverted(bool value)
    {
        isInverted = value;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Poop"))
        {
            globalGameManager.GameOver();
        }
    }
}
