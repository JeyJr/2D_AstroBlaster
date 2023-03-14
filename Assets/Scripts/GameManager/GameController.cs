using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, StartMatch, EndGame}
public class GameController : MonoBehaviour, IGameState
{
    [SerializeField] private BoxCollider2D ground;

    public static GameController instance;
    public static GameController GetInstance() 
    {
        return instance;
    }
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                MainMenu();
                break;
            case GameState.StartMatch:
                StartMatch();
                break;
            case GameState.EndGame:
                EndMatch();
                break;
            default:
                throw new ArgumentException("State invalido!");
        }
    }

    private void MainMenu()
    {
        GameEvents.GetInstance().MainMenu();
    }

    private void StartMatch()
    {
        GameEvents.GetInstance().StartMatch();
        ground.size = new Vector2(100, 1);
    }

    private void EndMatch()
    {
        GameEvents.GetInstance().EndGame();
        ground.size = new Vector2(100, 100);
    }



}

