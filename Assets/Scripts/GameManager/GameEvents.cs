using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameEvents instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static GameEvents GetInstance()
    {
        return instance;
    }

    public event Action OnMainMenu;
    public event Action OnStartMatch;
    public event Action OnEndGame;

    public event Action OnAddPoint;
    public event Action OnSFXChangedVolume;

    public void SFXChangedVolume()
    {
        OnSFXChangedVolume?.Invoke();
    }

    public void MainMenu()
    {
        OnMainMenu?.Invoke();
    }

    public void StartMatch()
    {
        OnStartMatch?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public void AddPoint()
    {
        OnAddPoint?.Invoke();
    }
}
