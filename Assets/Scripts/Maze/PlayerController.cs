using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Player stats
    private int score = 0;
    public float speed = 5.0f;
    public int health = 5;

    // Movement variables
    private float originalSpeed = 5.0f;
    public int jumpForce = 5;
    
    // UI elements
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text TestText;
    public Canvas GoalCanvas;

    // Dash skill variables
    float DashSkillCooldown = 5f;
    int NumberOfDashes = 1;
    int MaxDashes = 3;
    private bool isRecharging = false;
    bool isGrounded = true;
    bool isDashing = false;

    public Image dashCooldownImage;

    // Reference to the Rigidbody component
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
        // Check for game over condition
        if (health == 0)
        {
            Debug.Log("Game Over!");
            enabled = false;

            StartCoroutine(ResetRunCoroutine(3));
        }

        // Jumping mechanic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash skill activation
        if (Input.GetKeyDown(KeyCode.E) && NumberOfDashes > 0)
        {
            if (isDashing)
            {
                StopCoroutine("Dash");
                speed = speed / 1.5f - (speed / 1.5f - originalSpeed);
            }
            StartCoroutine("Dash");
        }
    }

    void FixedUpdate()
    {
        // Movement controls
        if (Input.GetKey(KeyCode.W))
            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.S))
            rb.MovePosition(rb.position - transform.forward * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.A))
            rb.MovePosition(rb.position - transform.right * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.D))
            rb.MovePosition(rb.position + transform.right * speed * Time.fixedDeltaTime);

        // Dash recharge logic
        if (NumberOfDashes < MaxDashes && !isRecharging)
        {
            isRecharging = true;
            StartCoroutine(DashCooldownAnimation());
            StartCoroutine(RechargeDash());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Increase score, player size, and reduce speed when colliding with pickups
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            score += 1;
            scoreText.text = "Score : " + score;
            transform.localScale = new Vector3(1 + score * 0.2f, 1 + score * 0.2f, 1 + score * 0.2f);
            if (originalSpeed <= 3.5f)
                originalSpeed = originalSpeed - 0.1f + (originalSpeed * 0.1f) * 0.010f;
            else
                originalSpeed = originalSpeed * (1 - (originalSpeed * 0.5f) * 0.022f);
            if (!isDashing)
                speed = originalSpeed;

        }


        // Reduce health when colliding with traps
        if (other.gameObject.CompareTag("Trap"))
        {
            health -= 1;
            healthText.text = "Health : " + health;
        }


        // Check for win condition when reaching the goal
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

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player has landed on the ground to allow jumping again
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    
    // Coroutine to reset the game after a delay, used for both game over and win conditions
    IEnumerator ResetRunCoroutine(int seconds)
    {
        Debug.Log("Revive after " + seconds + " seconds");
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Coroutine to handle the dash skill, increasing speed temporarily and then resetting it
    IEnumerator Dash()
    {
        isDashing = true;
        NumberOfDashes--;
        speed *= 1.5f;
        TestText.text = "dashes : " + NumberOfDashes;
        yield return new WaitForSeconds(2.5f);
        speed = speed / 1.5f - (speed / 1.5f - originalSpeed);
        isDashing = false;
    }

    // Coroutine to recharge the dash skill after a cooldown period, allowing the player to use it again
    IEnumerator RechargeDash()
    {
        yield return new WaitForSeconds(DashSkillCooldown);
        NumberOfDashes++;
        TestText.text = "dashes : " + NumberOfDashes;
        isRecharging = false;
    }

    IEnumerator DashCooldownAnimation()
    {
        float ClockTurn = 0f;

        dashCooldownImage.gameObject.SetActive(true);
        dashCooldownImage.fillAmount = 1f;       

        while (ClockTurn < DashSkillCooldown)
        {
            ClockTurn += Time.deltaTime;
            dashCooldownImage.fillAmount = 1f - (ClockTurn / DashSkillCooldown); // ✅ empties like a clock
            yield return null;                   
        }

        dashCooldownImage.fillAmount = 0f;
        dashCooldownImage.gameObject.SetActive(false);
    }
}
