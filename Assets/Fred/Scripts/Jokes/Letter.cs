using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public string letter;
    public TMP_Text letterText;
    public AudioSource Asource;

    private void Start()
    {
        letterText.text = letter;
        Asource = FindObjectOfType<JokeController>().GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        foreach (var displayLetter in JokeController.Instance.displayLetters)
        {
            if (displayLetter.GetComponent<DisplayLetter>().assignedLetter != letter) continue;
            
            displayLetter.GetComponent<DisplayLetter>().assignedLetter = letter;
            displayLetter.GetComponent<DisplayLetter>().SetLetter();
            JokeController.Instance.collectedLetters.Add(letter);
        }
        Asource.Play();
        Destroy(gameObject);
    }
}
