using UnityEngine;
using UnityEngine.InputSystem;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            pause();
        }
    }

    public void pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
