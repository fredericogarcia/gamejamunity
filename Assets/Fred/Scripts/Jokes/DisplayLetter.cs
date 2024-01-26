using System;
using TMPro;
using UnityEngine;

public class DisplayLetter : MonoBehaviour
{
    public string assignedLetter;

    private void Awake()
    {
        assignedLetter ??= "";
        GetComponent<TMP_Text>().text = assignedLetter;
    }

    public void SetLetter() => GetComponent<TMP_Text>().text = assignedLetter;
    public void LetterPlaceHolder(String placeholder) => GetComponent<TMP_Text>().text = placeholder;

}
