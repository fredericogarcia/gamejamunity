using UnityEngine;
public class ScenePause : MonoBehaviour
{
    public GameObject pauseMenu; // the pause menu game object

    private void Start()
    {
        if (SceneManager.Instance != null) SceneManager.Instance.isPaused = false; // set the game to not paused
    } 

    private void Update()
    {
        // if (SceneManager.Instance == null) return; // if the scene manager is null, return (this is for the debugging really, it should never be null)
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene_Main_Menu") return; // if in main menu, return
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) TogglePause(); // toggle the pause state if the escape or p key is pressed
    }

    public void TogglePause()
    {
        if (!SceneManager.Instance.isPaused)
        {
            pauseMenu.SetActive(true);
            SceneManager.Instance.TogglePause(); // toggle the pause state
        }
        else
        {
            SceneManager.Instance.TogglePause(); // toggle the pause state   
            pauseMenu.SetActive(false);
        }
    }
    
    public void LoadMainMenu() => SceneManager.Instance.LoadScene(""); // load the main menu 
}