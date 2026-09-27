using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KillboxlLogic : MonoBehaviour
{

    [SerializeField]
    private Vector2 spawnPosition = new Vector2(-10f, -4f);

    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Respawn(collision.gameObject));
        }
    }

    private IEnumerator Respawn(GameObject player)
    {
        Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();

        if (rigidbody != null) { 
            rigidbody.linearVelocity = Vector2.zero;
            rigidbody.simulated = false;
        }

        yield return new WaitForSeconds(3f);

        if (player != null) {
            player.transform.position = spawnPosition;
        }

        if (rigidbody != null)
        {
            rigidbody.simulated = true; 
        }
    }
}
