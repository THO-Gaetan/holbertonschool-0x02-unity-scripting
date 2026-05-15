using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 15, -15);
    public float smoothSpeed = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("maze");
    }
}

