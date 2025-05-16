using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefeatPageUI : MonoBehaviour
{
    public Text defaetText;
    public Text score;
    
    public Button restartBtn;
    public Button lobbyBtn;
    public Button exitBtn;

    
    public InputField inputField;
    public Text inputText;
    public Button inputBtn;

    public void DefeatPageInit()
    {
        this.DefeatEffect();
        //this.ShowScore();
        this.DefeatBtn();
    }

    public void DefeatEffect()
    {
        
    }
    public void ShowScore()
    {
        this.score.text = GameManager.instance.GetCurrentScore();
    }

    public void DefeatBtn()
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
        this.transform.parent.parent.GetComponent<InGameUI>().keyBoardApps[1].GetComponent<KeyBoardApp>().TurnOffKeyBoard();
        GameManager.instance.ResetScore();
        GameManager.instance.PlayerReset();
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
