using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class JokeController : MonoBehaviour
{
    public static JokeController Instance { get; private set; }
    public List<String> jokeList = new List<string>();
    public List<String> letterList = new List<string>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> displayLetters = new List<GameObject>();
    public List<string> collectedLetters = new List<string>();
    public String joke;
    public String chosenWord;
    public GameObject letterPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        StoreAllSpawnPoints();
    }

    private void Start()
    {
        PickAJokeAndSelectTheWord(jokeList);
        if (letterList.Count > 0) SpawnAllLetters();
        
        
        //displayLetters.Reverse();
        SetupDisplayLetters();
    }

    private void PickAJokeAndSelectTheWord(List<String> listOfJokes)
    {
        
        joke = listOfJokes[Random.Range(0, listOfJokes.Count)];
        var words = joke.Split(' ');
        var random = new System.Random();
        var index = random.Next(words.Length);
        
        chosenWord = words[index];
        
        var letters = chosenWord.ToCharArray();
        
        if (letters.Length <= 1)
        {
            PickAJokeAndSelectTheWord(listOfJokes);
        }
        else
        {
            foreach (var letter in letters)
            {
               letterList.Add(letter.ToString());
            }
        }
    }

    private void StoreAllSpawnPoints()
    {
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag("LetterSpawnPoint"))
        {
            spawnPoints.Add(spawnPoint);
        }
    }

    private void SpawnAllLetters()
    {
        foreach (var letter in letterList)
        {
            var randomSpawnPoint = Random.Range(0, spawnPoints.Count);
            var spawnPoint = spawnPoints[randomSpawnPoint];
            var letterObject = Instantiate(letterPrefab, spawnPoint.transform.position, Quaternion.identity);
            letterObject.GetComponent<Letter>().letter = letter;
            spawnPoints.Remove(spawnPoint);
        }
    }

    private void SetupDisplayLetters()
    {
        for (int i = 0; i < letterList.Count; i++)
        {
            displayLetters[i].GetComponent<DisplayLetter>().LetterPlaceHolder("_");
            displayLetters[i].GetComponent<DisplayLetter>().assignedLetter = letterList[i];
        }
    }
   
}
