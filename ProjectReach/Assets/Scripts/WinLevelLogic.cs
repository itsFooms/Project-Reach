using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevelLogic : MonoBehaviour
{
    [SerializeField] private GameObject WinScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
