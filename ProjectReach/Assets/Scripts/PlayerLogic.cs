using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    Vector2 respawnPosition;
    Coroutine coroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Destroy(gameObject);

            if (coroutine == null)
            {
                coroutine = StartCoroutine(RespawnCooldown());
            }
        }
    }

    private IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(gameObject);
        gameObject.transform.position = respawnPosition;
        coroutine = null;

        StopCoroutine(coroutine);
    }
}
