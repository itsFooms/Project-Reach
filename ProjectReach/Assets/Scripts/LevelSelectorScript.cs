using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorScript : MonoBehaviour
{   
    public void GoToLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

}
