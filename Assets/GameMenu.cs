using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject optionsCanvas;

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Options()
    {
        optionsCanvas.SetActive(!optionsCanvas.activeSelf);
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
