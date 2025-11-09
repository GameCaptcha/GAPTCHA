using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GlobalGameManager globalGameManager;

    float speed = 5.5f;


    public void SetPosition()
    {
        transform.localPosition = new Vector3();
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Debug.Log("Hit bullet");
            globalGameManager.GameOver();
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localPosition += Vector3.down * speed * Time.smoothDeltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localPosition += Vector3.up * speed * Time.smoothDeltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localPosition += Vector3.left * speed * Time.smoothDeltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localPosition += Vector3.right * speed * Time.smoothDeltaTime;
        }
    }
}
