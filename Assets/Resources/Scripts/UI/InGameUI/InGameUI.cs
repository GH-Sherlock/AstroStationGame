using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public GameObject PausePage;
    public GameObject VictoryPage;
    public GameObject DefeatPage;
    public LanguageText ingameLanguage;
    public Transform Anchor;
    public KeyBoardApp[] keyBoardApps;

    public void InGameUIinit(string country)
    {
        this.Anchor = this.transform.root.transform;
        this.ingameLanguage.LanguageTextInit();
        this.ChangeLanguage(country);
        this.PausePage.GetComponent<PausePageUI>().PausePageInit();
        this.VictoryPage.GetComponent<VictoryPageUI>().VictoryPageInit();
        this.DefeatPage.GetComponent<DefeatPageUI>().DefeatPageInit();
        this.CloseInGameUI();
    }
    public void ChangeLanguage(string country)
    {
        this.ingameLanguage.ChangeIngameLanguage(country);
    }
    public void CloseInGameUI()
    {
        this.PausePage.SetActive(false);
        this.VictoryPage.SetActive(false);
        this.DefeatPage.SetActive(false);
        this.keyBoardApps[0].TurnOffKeyBoard();
        this.keyBoardApps[1].TurnOffKeyBoard();
    }
    public void OpenPauseUI()
    {
        foreach(var data in CometController.instance.GetActiveComet())
        {
            data.GetComponent<Comet>().PauseComet();
        }
        this.PausePage.SetActive(true);
        //this.Anchor.rotation = Camera.main.transform.rotation;
        TimeManager.instance.Pause();
        SoundController.instance.PauseBGM();
    }
    public void OpenVictoryUI()
    {
        this.VictoryPage.SetActive(true);
        //this.Anchor.rotation = Camera.main.transform.rotation;
        TimeManager.instance.Pause();
        SoundController.instance.StopBGM();
        this.VictoryPage.GetComponent<VictoryPageUI>().ShowScore();
        this.keyBoardApps[0].TurnOnKeyBoard();
    }
    public void OpenDefeatUI()
    {
        this.DefeatPage.SetActive(true);
        //this.Anchor.rotation = Camera.main.transform.rotation;
        TimeManager.instance.Pause();
        SoundController.instance.StopBGM();
        this.DefeatPage.GetComponent<DefeatPageUI>().ShowScore();
        this.keyBoardApps[1].TurnOnKeyBoard();
    }
    private void Update()
    {
        if(QuestController.instance.FourButtonDown())
        {
            this.OpenPauseUI();
        }
    }
}
