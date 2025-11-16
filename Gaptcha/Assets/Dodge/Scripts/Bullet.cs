using UnityEngine;

public class Bullet : UpdateBehaviour
{

    Vector3 targetPosition;
    float speed;
    Vector2 normalizedTargetVector;

    float elapsedTime;
    float destroyTime = 3.5f;

    public void Init(Vector2 createPosition, Transform targetTransform)
    {
        elapsedTime = 0f;
        transform.localPosition = createPosition;

        targetPosition = targetTransform.localPosition;

        normalizedTargetVector = (targetPosition - transform.localPosition).normalized;

        speed = 5.0f;
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        transform.localPosition += (Vector3)normalizedTargetVector * speed * Time.fixedDeltaTime;
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
