using UnityEngine;

public class SceneLoaders : MonoBehaviour
{
    public void LoadGame() => SceneManager.Instance.LoadScene("Game"); // load the game scene
    public void LoadMainMenu() => SceneManager.Instance.LoadScene("MainMenu"); // load the main menu scene
    public void QuitGame() => SceneManager.Instance.QuitGame(); // quit the game
}