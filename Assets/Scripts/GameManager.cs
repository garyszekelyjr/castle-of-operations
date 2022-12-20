using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum Level { Start, Tutorial, Hallway, Arena }

public enum State { NotInBattle, InBattle }

public class GameManager : MonoBehaviour
{
    public State state;
    public Level level;

    string username;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        UpdateLevel(Level.Start);
        UpdateState(State.NotInBattle);
    }

    void Update()
    {
        switch (level)
        {
            case Level.Start:
                if (GameObject.Find("Username Input").GetComponent<TMP_InputField>().text != "")
                {
                    GameObject.Find("Start Button").GetComponent<Button>().interactable = true;
                }
                else
                {
                    GameObject.Find("Start Button").GetComponent<Button>().interactable = false;
                }
                break;
            case Level.Tutorial:
                break;
            case Level.Hallway:
                break;
            case Level.Arena:
                break;
        }
    }

    public void UpdateLevel(Level newLevel)
    {
        if (Enum.IsDefined(typeof(Level), newLevel))
        {
            level = newLevel;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(newLevel), newLevel, null);
        }
    }

    public void UpdateState(State newState)
    {
        if (Enum.IsDefined(typeof(State), newState))
        {
            state = newState;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void StartGame()
    {
        username = GameObject.Find("Username Input").GetComponent<TMP_InputField>().text;
        UpdateLevel(Level.Tutorial);
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
