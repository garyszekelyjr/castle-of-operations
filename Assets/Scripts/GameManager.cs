using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum Level { Start, Tutorial, AdditionHallway, AdditionArena, SubtractionHallway, SubtractionArena, MultiplicationHallway, MultiplicationArena, DivisionHallway, DivisionArena }

public enum State { NotInBattle, InBattle, Victory }

public enum GameState { NotInBattle, InBattle, Victory }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChanged;

    Level level;
    string username;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.NotInBattle);
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
            case Level.AdditionHallway:
                break;
            case Level.AdditionArena:
                break;
            case Level.SubtractionHallway:
                break;
            case Level.SubtractionArena:
                break;
            case Level.MultiplicationHallway:
                break;
            case Level.MultiplicationArena:
                break;
            case Level.DivisionHallway:
                break;
            case Level.DivisionArena:
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

    public void UpdateGameState(GameState newState)
    {
        if (Enum.IsDefined(typeof(GameState), newState))
        {
            state = newState;
            OnGameStateChanged?.Invoke(newState);
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
