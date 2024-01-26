using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIControls : MonoBehaviour
{

    public GameObject controlTest;
    public bool controller;
    public GameObject[] hudHints;
    public GameObject pauseMen;
    public bool isPaused;
    public AudioSource audioplaya;
    public Scene activeScene;
    public string currentScene;
    public bool isMainMenOpen;
    public GameObject menuObj;
    public GameObject menuObj2;
    public Animator menuAnimator;
    public GameObject creditsMenu;
    public Animator creditsAnimator;
    public GameObject settingsMenu;
    public Animator settingsAnimator;
    public string currentMenu;
    public GameObject resumeBTN;

    // Start is called before the first frame update
    void Start()
    {
        
        controller = false;
        isPaused = false;
        isMainMenOpen = false;

        //activeScene = SceneManager.Instance.currentLevel;
        currentScene = SceneManager.Instance.currentLevel;

        menuAnimator = menuObj2.GetComponent<Animator>();
        creditsAnimator = creditsMenu.GetComponent<Animator>();
        settingsAnimator = settingsMenu.GetComponent<Animator>();
        if (currentScene == "Fin-UIMainMenu")
        {
            PlayerPrefs.SetInt("18?", 0);
        }
        
        if (currentScene == "Level_1")
        {
            isMainMenOpen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        currentScene = SceneManager.Instance.currentLevel;
        //DETECT CONTROLLER
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
        {
            controller = false;
            Debug.Log("ControllerNo");
        }

        if (Input.GetButtonDown("ControllerFind"))
        {
            controller = true;
            Debug.Log("ControllerYes");
        }
        
        //MAIN MENU
        if (currentScene == "Fin-UIMainMenu" && Input.GetButtonDown("Confirm") && isMainMenOpen == false)
        {
            menuObj.SetActive(true);
            isMainMenOpen = true;
        }
        
        if (currentScene == "Fin-UIMainMenu" && Input.GetButtonDown("Back") && isMainMenOpen == true)
        {
            CloseCredits();
        }
    
        //SHOW/HIDE CONTROLS
        if (Input.GetButtonUp("ShowHideControls"))
        {
            if (controlTest.activeInHierarchy == false)
            {
                controlTest.SetActive(true);

                if(currentScene == "Fin-UIMainMenu")
                {
                    PlayerPrefs.SetInt("18?", 1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                if (currentScene == "Fin-UIMainMenu")
                {
                    PlayerPrefs.SetInt("18?", 0);
                    PlayerPrefs.Save();
                }
                controlTest.SetActive(false);
            }
        }
        
        //PAUSE GAME
        if (Input.GetButtonUp("Pause"))
        {
            if (isPaused == false)
            {
                pauseMen.SetActive(true);
                isPaused = true;
                currentMenu = "pause";
                EventSystem.current.SetSelectedGameObject(resumeBTN);
                Time.timeScale = 0;
            }

            else
            {
                if (currentMenu == "credits")
                {
                    CloseCredits();
                }
            
                else if (currentMenu == "settings")
                {
                    CloseSettings();
                }
            }
        }

        if (Input.GetButtonUp("Back") && isPaused == true)
        {
            if (currentMenu == "credits")
            {
                CloseCredits();
            }
            
            else if (currentMenu == "settings")
            {
                CloseSettings();
            }
        }
        
        //SWITCH HUD BUTTONS
        if (controller == false)
        {
            hudHints[0].SetActive(true);
            hudHints[1].SetActive(false);
        }
        
        else if (controller == true)
        {
            hudHints[1].SetActive(true);
            hudHints[0].SetActive(false);
        }
        
        if (controller == false && currentScene == "Fin-UIMainMenu" && isMainMenOpen == false)
        {
            hudHints[2].SetActive(true);
            hudHints[3].SetActive(false);
        }
        
        else if (controller == true && currentScene == "Fin-UIMainMenu" && isMainMenOpen == false)
        {
            hudHints[3].SetActive(true);
            hudHints[2].SetActive(false);
        }
        
        if (controller == false && currentScene == "Fin-UIMainMenu" && isMainMenOpen == true)
        {
            hudHints[4].SetActive(true);
            hudHints[5].SetActive(false);
        }
        
        else if (controller == true && currentScene == "Fin-UIMainMenu" && isMainMenOpen == true)
        {
            hudHints[5].SetActive(true);
            hudHints[4].SetActive(false);
        }

        
        
    }

    public void playsound()
    {
        print("gfsdgf");
        audioplaya.Play();
    }

    public void PlayBTN()
    {
       SceneManager.Instance.LoadScene("Level_1");
    }

    public void OpenCredits()
    {
        menuAnimator.SetTrigger("ChangeMenu");
        StartCoroutine(OpenCreditsWait());
    }

    IEnumerator OpenCreditsWait()
    {
        yield return new WaitForSecondsRealtime(1);
        menuObj2.SetActive(false);
        creditsMenu.SetActive(true);
        currentMenu = "credits";
        creditsAnimator.SetTrigger("MainMenu");
        creditsAnimator.ResetTrigger("MainMenu");
        menuAnimator.ResetTrigger("ChangeMenu");
    }
    
    public void CloseCredits()
    {
        creditsAnimator.SetTrigger("ChangeMenu");
        StartCoroutine(CloseCreditsWait());
    }

    IEnumerator CloseCreditsWait()
    {
        yield return new WaitForSecondsRealtime(1);
        creditsMenu.SetActive(false);
        menuObj2.SetActive(true);
        currentMenu = "pause";
        menuAnimator.SetTrigger("MainMenu");
        creditsAnimator.ResetTrigger("ChangeMenu");
        menuAnimator.ResetTrigger("MainMenu");
    }

    public void ResumeBTN()
    {
        menuObj.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }
    
    public void OpenSettings()
    {
        menuAnimator.SetTrigger("ChangeMenu");
        StartCoroutine(OpenSettingsWait());
    }

    IEnumerator OpenSettingsWait()
    {
        yield return new WaitForSecondsRealtime(1);
        menuObj2.SetActive(false);
        settingsMenu.SetActive(true);
        currentMenu = "settings";
        settingsAnimator.SetTrigger("MainMenu");
        settingsAnimator.ResetTrigger("MainMenu");
        menuAnimator.ResetTrigger("ChangeMenu");
    }
    
    public void CloseSettings()
    {
        settingsAnimator.SetTrigger("ChangeMenu");
        StartCoroutine(CloseSettingsWait());
    }

    IEnumerator CloseSettingsWait()
    {
        yield return new WaitForSecondsRealtime(1);
        settingsMenu.SetActive(false);
        menuObj2.SetActive(true);
        currentMenu = "pause";
        menuAnimator.SetTrigger("MainMenu");
        settingsAnimator.ResetTrigger("ChangeMenu");
        menuAnimator.ResetTrigger("MainMenu");
    }

    public void QuitBTN()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_1");
    }
}
