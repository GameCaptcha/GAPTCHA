using UnityEngine;

public class Poop : UpdateBehaviour
{
    public float fallSpeed = 3f;
    public float destroyY = -6f;

    override protected void FUpdate()
    {
        base.FUpdate();
        transform.localPosition += Vector3.down * fallSpeed * Time.smoothDeltaTime;
        if (transform.localPosition.y < destroyY)
            Destroy(gameObject);
    }
}
