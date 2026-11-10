using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{
    public void ChargerJeu()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quitter()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoTolevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
