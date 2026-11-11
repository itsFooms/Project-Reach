using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{
    public void ChargerJeu()
    {
        SceneManager.LoadScene("Game");
    }

    public void NiveauScreen()
    {
        SceneManager.LoadScene("Niveaux");
    }

    public void RetourMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ChargerNiveau(string nomNiveau)
    {
        SceneManager.LoadScene(nomNiveau);
    }

    public void Quitter()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
