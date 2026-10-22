using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseButtonManager : MonoBehaviour
{
   
    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
