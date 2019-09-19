using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{

    public void PauseGame()
    {
        GameController.PauseGame();
    }

    public void PlayGame()
    {
        GameController.PlayGame();
    }

    public void SwitchGameState()
    {
        GameController.SwitchGameState();
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
