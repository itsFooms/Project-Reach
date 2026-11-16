using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuLogic : BaseButtonManager
{
    private GameObject pauseScreen;

    void Awake()
    {
        pauseScreen = transform.parent.gameObject;
    }

    public void resume()
    {
        Time.timeScale = 1.0f;
        pauseScreen.SetActive(false);
    }
}
