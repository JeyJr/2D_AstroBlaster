using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PanelName { MainMenu, HUD, EndGame}
public class UIController : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    [Header("MAINMENU")]
    [SerializeField] private TextMeshProUGUI mainMenuTxtRecord;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI hudTxtPoints;
    [SerializeField] private TextMeshProUGUI hudTxtMSG;

    [Header("ENDGAME")]
    [SerializeField] private TextMeshProUGUI endGameTxtPoints;
    [SerializeField] private TextMeshProUGUI endGameTxtRecord;
    [SerializeField] private Button btnPlayAgain;


    private void Start()
    {
        GameEvents.GetInstance().OnMainMenu += EnableMainMenu;
        GameEvents.GetInstance().OnStartMatch += EnableHUD;
        GameEvents.GetInstance().OnEndGame += EnableEndGame;

        GameEvents.GetInstance().OnAddPoint += UpdateHUDTextPoints;

        GameController.GetInstance().SetGameState(GameState.MainMenu);
    }

    private void EnablePanel(PanelName name)
    {
        foreach (var item in panels)
        {
            if(item.name == name.ToString())
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    private void EnableMainMenu() 
    {
        EnablePanel(PanelName.MainMenu);
        UpdateTextRecordMainMenu();
    }
    private void EnableHUD() 
    {
        EnablePanel(PanelName.HUD);
        UpdateHUDTextPoints();
        BtnDisableBtnPlayAgain();
    }
    private void EnableEndGame() 
    {
        EnablePanel(PanelName.EndGame);
        EnableBtnPlayAgain();
        UpdateTextsEndGame();
    }


    public void BtnPlayGame() 
    {
        GameController.GetInstance().SetGameState(GameState.StartMatch);
    }
    public void BtnRestartMatch() 
    {
        GameController.GetInstance().SetGameState(GameState.MainMenu);
        UpdateTextRecordMainMenu();
    }


    #region MainMenu

    private void UpdateTextRecordMainMenu()
    {
        float record = GameData.GetInstance().GetRecord();
        mainMenuTxtRecord.text = "Recorde:\n" + record.ToString();
    }

    public void BtnEnablePanel(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void BtnResetRecord()
    {
        GameData.GetInstance().SetRecorde(0);
        UpdateTextRecordMainMenu();
    }

    #endregion

    #region HUD

    private void UpdateHUDTextPoints()
    {
        float points = GameData.GetInstance().GetPoint();
        hudTxtPoints.text = points.ToString();

        //if (points > 1000)
        //{
        //    float kPoints = points / 1000f;
        //    hudTxtPoints.text = string.Format("{0:#.#}k", kPoints);
        //}
        //else
        //{
        //    hudTxtPoints.text = points.ToString();
        //}
    }


    #endregion

    #region ENDGAME

    public void UpdateTextsEndGame()
    {
        float points = GameData.GetInstance().GetPoint();
        endGameTxtPoints.text = "Pontos:\n" + points.ToString();

        float record = GameData.GetInstance().GetRecord();

        if(points > record)
        {
            endGameTxtRecord.text = "Novo recorde:\n" + points.ToString();
            GameData.GetInstance().SetRecorde((int)points);
        }
        else
        {
            endGameTxtRecord.text = "Recorde:\n" + record.ToString();
        }
    }

    public void BtnDisableBtnPlayAgain()
    {
        btnPlayAgain.interactable = false;
    }

    private void EnableBtnPlayAgain()
    {
        StartCoroutine(EnablePlayAgain());
    }

    IEnumerator EnablePlayAgain()
    {
        yield return new WaitForSeconds(1);
        btnPlayAgain.interactable = true;
    }

    #endregion
}
