using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    public bool isPaused;
    public string currentLevel;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    private void Start() => DontDestroyOnLoad(this);

    public void LoadScene(string sceneName)
    {
        if (currentLevel == "Level_1")
        {
            TimerManager.Instance.ResetTimer(); // reset the timer
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // load a scene by name
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
    }
    public void QuitGame() => Application.Quit(); // quit the game

    public void TogglePause() // toggle the pause state
    {
        isPaused = !isPaused; // toggle the pause state
        if (isPaused) PauseGame(); // if the game is not paused, pause it
        else ResumeGame(); // otherwise, resume it    
    }
    
    private static void PauseGame() => Time.timeScale = 0; // pause the game
    private static void ResumeGame() => Time.timeScale = 1; // resume the game

    private void Update()
    {
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
}
