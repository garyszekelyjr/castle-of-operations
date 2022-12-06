using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    void Awake(){
        Instance = this;
    }

    void Start(){
        UpdateGameState(GameState.NotInBattle);
    }

    public void UpdateGameState(GameState newState){
        state = newState;

        switch (newState) {
            case GameState.NotInBattle:
                break;
            case GameState.InBattle:
                break;
            case GameState.Victory:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState {
    NotInBattle,
    InBattle,
    Victory
}
