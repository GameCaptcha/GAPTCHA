using System;
using UnityEngine;

public class Bullet : SpriteBehaviour
{
    [SerializeField] AfterImageGenerator afterImageGenerator;

    Vector3 targetPosition;

    float baseSpeed = 5.0f;
    float currentSpeed;
    Vector2 normalizedTargetVector;

    float elapsedTime;
    float destroyTime = 3.5f;

    Action<Bullet> restoreAction = null;

    public void Init(Vector2 createPosition, Transform targetTransform, Action<Bullet> action, AfterImageDebuff afterImageDebuff)
    {
        elapsedTime = 0f;
        transform.localPosition = createPosition;

        targetPosition = targetTransform.localPosition;

        afterImageGenerator.SetDebuff(afterImageDebuff);
        normalizedTargetVector = (targetPosition - transform.localPosition).normalized;

        currentSpeed = baseSpeed;

        restoreAction = action;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        currentSpeed = baseSpeed * multiplier;
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        transform.localPosition += (Vector3)normalizedTargetVector * currentSpeed * Time.fixedDeltaTime;
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime >= destroyTime)
        {
            //Destroy(gameObject);
            restoreAction?.Invoke(this);
        }
    }
}
