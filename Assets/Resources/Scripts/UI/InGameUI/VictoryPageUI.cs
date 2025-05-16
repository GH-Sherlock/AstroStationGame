using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryPageUI : MonoBehaviour
{
    public Text victoryText;
    public Text score;

    public Button restartBtn;
    public Button lobbyBtn;
    public Button exitBtn;


    public InputField inputField;
    public Text inputText;
    public Button inputBtn;

    public void VictoryPageInit()
    {
        this.victoryEffect();
        //this.ShowScore();
        this.victoryBtn();
    }

    public void victoryEffect()
    {
        
    }
    public void ShowScore()
    {
        this.score.text = GameManager.instance.GetCurrentScore();
    }

    public void victoryBtn()
    {
        this.restartBtn.onClick.RemoveAllListeners();
        this.restartBtn.onClick.AddListener(() =>
        {
            this.ActRestartBtn();
        });

        this.lobbyBtn.onClick.RemoveAllListeners();
        this.lobbyBtn.onClick.AddListener(() =>
        {
            this.ActlobbyBtn();
        });

        this.exitBtn.onClick.RemoveAllListeners();
        this.exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        this.inputBtn.onClick.RemoveAllListeners();
        this.inputBtn.onClick.AddListener(() =>
        {
            this.ActInputBtn();
        });
    }

    public void ActRestartBtn()
    {
        SoundController.instance.UnPauseBGM();
        SoundController.instance.StopBGM();
        CometController.instance.ResetComet(SoundController.instance.bgmName);
        this.transform.parent.parent.GetComponent<InGameUI>().keyBoardApps[0].GetComponent<KeyBoardApp>().TurnOffKeyBoard();
        GameManager.instance.ResetScore();
        this.gameObject.SetActive(false);
    }
    public void ActlobbyBtn()
    {
        SoundController.instance.StopBGM();
        var operation = SceneManager.LoadSceneAsync("TestMap");
        operation.completed += (a) =>
        {
            var tempLobby = GameObject.FindObjectOfType<LobbyUI>();
            SoundController.instance.StopBGM();
            tempLobby.SetLobbyUI("astro_opening");
        };
    }
    public void ActInputBtn()
    {
        GameManager.instance.SetPlayerName(this.inputText.text);
        GameManager.instance.CalculateRank();
    }
}
