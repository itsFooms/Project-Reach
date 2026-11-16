using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevelScreen : BaseButtonManager
{

    private int currentLevelIndex;
    private int nextLevelIndex;

    void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        nextLevelIndex = currentLevelIndex + 1;
    }

    public void nextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelIndex);
    }

}
