using UnityEngine;
using System.Collections;

public class MenuPlayer : MonoBehaviour
{
    
    public Transform player;
    private bool isActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            StartCoroutine(TpPlayerRandomely());
        }
    }

    IEnumerator TpPlayerRandomely()
    {
        isActive = false;
        yield return new WaitForSeconds(2.5f);
        float randomX = Random.Range(-25f, 25f);
        float randomZ = Random.Range(-25f, 25f);
        player.position = new Vector3(randomX, player.position.y, randomZ);
        isActive = true;
    }
}
