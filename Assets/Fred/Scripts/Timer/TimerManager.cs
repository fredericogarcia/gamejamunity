using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }
    public float currentTime;
    public float bestTime;
    
    public TMP_Text timeDisplay;

    private void Awake()
    {
        // There can only be one instance of this running
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    

    private void Start()
    {
        if (PlayerPrefs.HasKey(GetSceneName())) bestTime = PlayerPrefs.GetFloat(GetSceneName());
        else
        {
            PlayerPrefs.SetFloat(GetSceneName(), 1000000);
            bestTime = PlayerPrefs.GetFloat(GetSceneName());
        }
    }
    
    private void Update()
    {
        currentTime += Time.deltaTime;
        DisplayTimer(currentTime, timeDisplay);
    }

    public void ResetTimer() => currentTime = 0;
    
    // to be used by GUI manager, updates timer on every frame
    private void DisplayTimer(float time, TMP_Text text)
    {
        // starts timer if not paused and displays time left to player
        if (!(time > 0)) return;
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        text.text = $"{minutes:00}:{seconds:00}";
    }

    public void SaveLevelTime()
    {

        
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat(GetSceneName(), bestTime);
        }
        else bestTime = PlayerPrefs.GetFloat(GetSceneName());

        DisplayTimer(currentTime, FindObjectOfType<EndOfLevel>().currentTimeDisplay);
        DisplayTimer(bestTime, FindObjectOfType<EndOfLevel>().bestTimeDisplay);
        
        PlayerPrefs.Save();
        Time.timeScale = 0;
    }

    private string GetSceneName() => SceneManager.Instance.currentLevel;

}