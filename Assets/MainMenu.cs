using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject aboutCanvas;
    public GameObject howToPlayCanvas;
    public GameObject optionsCanvas;

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void HowToPlay()
    {
        aboutCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(!howToPlayCanvas.activeSelf);
    }

    public void About()
    {
        howToPlayCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        aboutCanvas.SetActive(!aboutCanvas.activeSelf);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Options()
    {
        aboutCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        optionsCanvas.SetActive(!optionsCanvas.activeSelf);
    }
}
