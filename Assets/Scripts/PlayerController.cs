using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public int health = 5;
    private int score = 0;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.S))
            rb.MovePosition(rb.position - transform.forward * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.A))
            rb.MovePosition(rb.position - transform.right * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.D))
            rb.MovePosition(rb.position + transform.right * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            score += 1;
            Debug.Log("Score: " + score);
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            health -= 1;
            Debug.Log("Health: " + health);
        }
        if (other.gameObject.CompareTag("Goal"))
        {
            if (score >= 3)
            {
                Debug.Log("You win!");
            }
            else
            {
                Debug.Log("You need at least 3 points to win!");
            }
        }
    }
}
