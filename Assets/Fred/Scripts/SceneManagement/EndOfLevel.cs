using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndOfLevel : MonoBehaviour
{
    public TMP_Text bestTimeDisplay;
    public TMP_Text currentTimeDisplay;
    public TMP_Text jokeText;
    public GameObject endOfLevelPanel;
    public GameObject SteamBoatWilly;
    public TMP_Text RestartText;
    public GameObject RestartLevelUI;
    public GameObject RestartButton;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (JokeController.Instance.letterList.Count != JokeController.Instance.collectedLetters.Count) return;
        jokeText.text = JokeController.Instance.joke;
        TimerManager.Instance.SaveLevelTime();
        endOfLevelPanel.SetActive(true);
        SteamBoatWilly.SetActive(true);
        Time.timeScale = 1;
        Invoke("ShowRestart",5f);
    }

    public void ShowRestart()
    {
        RestartLevelUI.SetActive(true);
        RestartText.text = "you died'nt";
        RestartText.fontSize = 140;
        EventSystem.current.SetSelectedGameObject(RestartButton);
        Time.timeScale = 0;
    }
}
