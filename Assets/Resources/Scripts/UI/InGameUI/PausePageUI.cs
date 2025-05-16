using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePageUI : MonoBehaviour
{
    public Text pauseText;

    public Button resumeBtn;
    public Button restartBtn;
    public Button lobbyBtn;
    public Button exitBtn;
    public Button soundBtn;

    public GameObject soundPage;
    private bool IsSoundPageReady;
    public Slider bgmSlider;
    private bool IsBgmChange;
    public Slider sfxSlider;

    public void PausePageInit()
    {
        this.PauseBtn();
        this.soundPage.SetActive(false);
    }

    public void PauseBtn()
    {
        this.resumeBtn.onClick.RemoveAllListeners();
        this.resumeBtn.onClick.AddListener(() =>
        {
            this.ActResumeBtn();
        });

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

        this.soundBtn.onClick.RemoveAllListeners();
        this.soundBtn.onClick.AddListener(() =>
        {
            this.ActSoundBtn();
        });
    }

    public void ActResumeBtn()
    {
        foreach (var data in CometController.instance.GetActiveComet())
        {
            data.GetComponent<Comet>().TurnOnComet();
        }
        TimeManager.instance.Resume();
        SoundController.instance.UnPauseBGM();
        this.gameObject.SetActive(false);
    }
    public void ActRestartBtn()
    {
        SoundController.instance.UnPauseBGM();
        SoundController.instance.StopBGM();
        CometController.instance.ResetComet(SoundController.instance.bgmName);
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
            tempLobby.SetLobbyUI("astro_opening");
            SoundController.instance.StopBGM();
        };
    }
    public void ActSoundBtn()
    {   
        if(!this.IsSoundPageReady)
        {
            this.soundPage.SetActive(true);
            this.IsBgmChange = true;
            this.IsSoundPageReady = true;
        }
        else
        {
            this.soundPage.SetActive(false);
            this.IsBgmChange = false;
            this.IsSoundPageReady = false;
        }
    }

    private void Update()
    {
        if(this.IsBgmChange)
        {
            SoundController.instance.ChangeBGMVolume(this.bgmSlider.value);
        }
    }
}
