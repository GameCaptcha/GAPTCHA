using UnityEngine;

public class PipeMove : UpdateBehaviour
{
    
    [SerializeField] private float _movSpeed; // 파이프 이동속도
    
    void Start()
    {
        
    }

    override protected void FUpdate()
    { 
        base.FUpdate();
        transform.position += Vector3.left * (_movSpeed * Time.deltaTime);
    }
}
