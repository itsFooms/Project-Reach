using System.Collections;
using UnityEngine;

public class KillboxLogic : MonoBehaviour
{
    [SerializeField] private GameObject deathPrefab;
    private GameObject respawnPoint;

    private void Start()
    {
        respawnPoint = GameObject.FindWithTag("Respawn");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (deathPrefab != null)
            {
                Instantiate(deathPrefab, collision.transform.position, Quaternion.identity);
            }
            StartCoroutine(Respawn(collision.gameObject));
        }
    }

    private IEnumerator Respawn(GameObject player)
    {
        Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();

        if (rigidbody != null)
        {
            rigidbody.linearVelocity = Vector2.zero;
            rigidbody.simulated = false;
        }

        yield return new WaitForSeconds(1f);

        if (player != null)
        {
            player.transform.position = respawnPoint.transform.position;
        }

        if (rigidbody != null)
        {
            rigidbody.simulated = true;
        }

    }
}
