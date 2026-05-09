using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 8.0f;
    public int health = 5;
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public Canvas GoalCanvas;
    private int score = 0;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GoalCanvas.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Debug.Log("Game Over!");
            enabled = false;

            StartCoroutine(ResetRunCoroutine(3));
        }
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
            scoreText.text = "Score : " + score;
            transform.localScale = new Vector3(1 + score * 0.2f, 1 + score * 0.2f, 1 + score * 0.2f);
            if (speed <= 3.5f)
                speed = speed - 0.1f + (score * 0.1f) * 0.010f;
            else
                speed = speed * (1 - (score * 0.5f) * 0.022f);
            
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            health -= 1;
            healthText.text = "Health : " + health;
        }
        if (other.gameObject.CompareTag("Goal"))
        {
            if (score >= 3)
            {
                Debug.Log("You win!");
                GoalCanvas.enabled = true;
                enabled = false;
                StartCoroutine(ResetRunCoroutine(5));
            }
            else
            {
                Debug.Log("You need at least 3 points to win!");
            }
        }
    }
    
    IEnumerator ResetRunCoroutine(int seconds)
    {
        Debug.Log("Revive after " + seconds + " seconds");
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
