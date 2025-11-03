using UnityEngine;

public class PipeMove : MonoBehaviour {
    
    [SerializeField] private float _movSpeed; // 파이프 이동속도
    
    void Start()
    {
        
    }

    void Update() {
        transform.position += Vector3.left * (_movSpeed * Time.deltaTime);
    }
}
