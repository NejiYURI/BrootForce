using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour
{
    public static MainMenuScript instance;
    private void Awake()
    {
        instance = this;
    }

    public UnityEvent<GameObject,bool> PlayerInField;
    public void SinglePlayer()
    {
        SceneManager.LoadScene("MainGameScene_Single");
    }

    public void TwoPlayers()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
