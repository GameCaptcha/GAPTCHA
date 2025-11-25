using System;
using UnityEngine;

public class Poop : SpriteBehaviour
{
    [SerializeField] AfterImageGenerator afterImageGenerator;

    float fallSpeed = 3f;
    float destroyY = -6f;
    Action<Poop> restoreAction = null;

    public void Init(float speed, AfterImageDebuff afterImageDebuff, Action<Poop> action)
    {
        fallSpeed = speed;
        afterImageGenerator.SetDebuff(afterImageDebuff);
        restoreAction = action;
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        transform.localPosition += Vector3.down * fallSpeed * Time.fixedDeltaTime;
        if (transform.localPosition.y < destroyY)
        {
            restoreAction?.Invoke(this);
        }

    }
}
