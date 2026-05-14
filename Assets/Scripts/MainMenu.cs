using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playBut, credBut, quitBut, gTitle, credText, backBut;

    private void Start()
    {
        credText.SetActive(false);
        backBut.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void quitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        playBut.SetActive(false);
        credBut.SetActive(false);
        quitBut.SetActive(false);
        gTitle.SetActive(false);
        credText.SetActive(true);
        backBut.SetActive(true);
        print("ADADAddadadaD");
    }

    public void Back()
    {
        playBut.SetActive(true);
        credBut.SetActive(true);
        quitBut.SetActive(true);
        gTitle.SetActive(true);
        credText.SetActive(false);
        backBut.SetActive(false);
    }
}
