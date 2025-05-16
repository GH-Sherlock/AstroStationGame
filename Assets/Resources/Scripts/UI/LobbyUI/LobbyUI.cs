using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wilberforce;

public class LobbyUI : MonoBehaviour
{
    private void Start()
    {
        this.LobbyUIinit("astro_opening");
    }
    private void Update()
    {
        if(IsSoundBtnReady)
        {
            SoundController.instance.ChangeBGMVolume(arrSlide[0].value);
        }
    }

    public GameObject[] arrPages;

    public void LobbyUIinit(string bgmName)
    {
        // 데이터,사운드 init
        if(DataManager.GetInstance().initDone == false)
        {
            DataManager.GetInstance().DataManagerInit();
            SoundController.instance.SoundControllerInit();
        }

        // 로비UI init
        this.SetLobbyUI(bgmName);

        Debug.Log("Lobby UI Ready");
    }
    public void SetLobbyUI(string bgmName)
    {
        SoundController.instance.PlayBGM(bgmName);
        SoundController.instance.bgmPlayer.loop = true;
        SoundController.instance.ChangeBGMVolume(0.1f);
        this.pageSpeed = 4f;
        this.SetStagePage();
        this.SetSettingPage();
        this.BlindBtn();
        this.TranslateLanguage();
        this.SetLobbyLanguage();
        this.ActLobbyButton();
        this.LayoutBtn();
        this.ResetPages();
    }

    public void ResetPages()
    {
        for (int i = 0; i < arrPages.Length; i++)
        {
            this.arrPages[i].transform.position = this.arrPivot[0].position;
        }
        if(this.listStages != null)
        {
            foreach (var x in this.listStages)
            {   
                x.ResetStagePage();
            }
        }
        this.IsPlayBtnReady = false;
        this.IsSettingBtnReady = false;
        this.IsSoundBtnReady = false;
        this.IsGameplayBtnReady = false;
        this.IsLanguageBtnReady = false;
        this.IsVisualBtnReady = false;
        this.IsMarketBtnReady = false;
    }

    public Canvas lobby_canvas;
    public Camera camera_2d;
    public Camera camera_vr;
    public GameObject UIHelpers;
    public OVRRaycaster oVRRaycaster;
    public GameObject parent;

    public void SetLobbyCamera(string vr)
    {
        if("vr" == vr)
        {
            this.lobby_canvas.worldCamera = this.camera_vr;
            this.UIHelpers.SetActive(true);
            this.oVRRaycaster.enabled = true;
        }
        else
        {
            this.lobby_canvas.worldCamera = this.camera_2d;
            this.UIHelpers.SetActive(false);
            this.oVRRaycaster.enabled = false;
        }
    }

    public Transform[] arrPivot;
    public Button[] arrMainBtn;
    private float pageSpeed;

    public void ActLobbyButton()
    {
        this.arrMainBtn[0].onClick.RemoveAllListeners();
        this.arrMainBtn[0].onClick.AddListener(() =>
        {
            this.PlayBtn(); 
        });

        this.arrMainBtn[1].onClick.RemoveAllListeners();
        this.arrMainBtn[1].onClick.AddListener(() =>
        { 
            this.SettingBtn(); 
        });

        this.arrMainBtn[2].onClick.RemoveAllListeners();
        this.arrMainBtn[2].onClick.AddListener(() =>
        { 
            this.MarketBtn(); 
        });
    }
    private void PlayBtn()
    {
        if (this.IsPlayBtnReady)
        {
            this.ResetPages();
            this.IsPlayBtnReady = false;
        }
        else
        {
            this.ResetPages();
            this.arrPages[1].transform.position = Vector3.MoveTowards(this.arrPivot[0].position, this.arrPivot[1].position, this.pageSpeed);
            this.IsPlayBtnReady = true;
        }
    }

    public Transform content;
    public List<StagePageUI> listStages;
    private bool IsPlayReady;
    private bool IsPlayBtnReady;

    public void SetStagePage()
    {
        if (this.IsPlayReady) return;
        this.listStages = new List<StagePageUI>();
        foreach(var pair in DataManager.GetInstance().GetDicSoundData())
        {
            var key = pair.Key;
            var data = pair.Value;
            var path = "Prefabs/UI/StagePageUI_2";
            var model = Resources.Load(path) as GameObject;
            var page = Instantiate<GameObject>(model, this.arrPivot[0].position, Quaternion.identity);
            page.transform.SetParent(this.content);
            page.transform.localScale = new Vector3(1, 1, 1);
            var stage = page.GetComponent<StagePageUI>();
            stage.StagePageInit(data, arrPivot, this.parent);
            this.listStages.Add(stage);
        }
        foreach(var data in this.listStages)
        {
           
            data.SetListStages(this.listStages, arrPages[9].transform);
            this.languageText.listLobbyLanguageText.Add(data.songinfo.btnText);
        }
        this.IsPlayReady = true;
    }

    private bool IsSettingReady;
    private bool IsSettingBtnReady;

    private void SettingBtn()
    {
        if(this.IsSettingBtnReady)
        {
            this.ResetPages();
            this.IsSettingBtnReady = false;
        }
        else
        {
            this.ResetPages();
            this.arrPages[2].transform.position = Vector3.MoveTowards(this.arrPivot[0].position, this.arrPivot[1].position, this.pageSpeed);
            this.IsSettingBtnReady = true;
        }
    }

    public Button[] arrSettingBtn;
    public Slider[] arrSlide;
    public void ResetSettingPage()
    {
        this.ResetPages();
        this.arrPages[2].transform.position = Vector3.MoveTowards(this.arrPivot[0].position, this.arrPivot[1].position, this.pageSpeed);
        this.IsSettingBtnReady = true;
    }
    public void SetSettingPage()
    {
        if (this.IsSettingReady) return;
        //사운드
        this.arrSettingBtn[0].onClick.RemoveAllListeners();
        this.arrSettingBtn[0].onClick.AddListener(() =>
        {
            this.SoundBtn();
        });
        //비주얼,시각
        this.arrSettingBtn[1].onClick.RemoveAllListeners();
        this.arrSettingBtn[1].onClick.AddListener(() =>
        {
            this.VisualBtn();
        });
        //게임플레이세팅
        this.arrSettingBtn[2].onClick.RemoveAllListeners();
        this.arrSettingBtn[2].onClick.AddListener(() =>
        {
            this.GameplayBtn();
        });
        //언어바꾸기, 한국어/영어/중국어
        this.arrSettingBtn[3].onClick.RemoveAllListeners();
        this.arrSettingBtn[3].onClick.AddListener(() =>
        {
            this.LanguageBtn();
        });
        this.IsSettingReady = true;
    }
    private bool IsSoundBtnReady;
    private float bgmVolume;
    public void SoundBtn()
    {
        if(!this.IsSoundBtnReady)
        {
            this.ResetSettingPage();
            this.arrPages[5].transform.position = Vector3.MoveTowards(this.arrPivot[1].position, this.arrPivot[2].position, this.pageSpeed);
            this.IsSoundBtnReady = true;
        }
        else
        {
            this.ResetSettingPage();
            this.IsSoundBtnReady = false;
        }
    }
    private bool IsVisualReady;
    private bool IsVisualBtnReady;
    public void VisualBtn()
    {
        if(!this.IsVisualBtnReady)
        {
            this.ResetSettingPage();
            this.arrPages[6].transform.position = Vector3.MoveTowards(this.arrPivot[1].position, this.arrPivot[2].position, this.pageSpeed);
            this.IsVisualBtnReady = true;
        }
        else
        {
            this.ResetSettingPage();
            this.IsVisualBtnReady = false;
        }
    }

    public Button[] arrVisualBtn;
    public void BlindBtn()
    {
        //일반시야
        this.arrVisualBtn[0].onClick.RemoveAllListeners();
        this.arrVisualBtn[0].onClick.AddListener(() =>
        {
            this.NormalVision();
        });
        //적녹색맹
        this.arrVisualBtn[1].onClick.RemoveAllListeners();
        this.arrVisualBtn[1].onClick.AddListener(() =>
        {
            this.RedGreenBlind();
        });
        //녹색맹
        this.arrVisualBtn[2].onClick.RemoveAllListeners();
        this.arrVisualBtn[2].onClick.AddListener(() =>
        {
            this.GreenBlind();
        });
        //청색맹
        this.arrVisualBtn[3].onClick.RemoveAllListeners();
        this.arrVisualBtn[3].onClick.AddListener(() =>
        {
            this.BlueBlind();
        });
    }
    public void NormalVision()
    {
        this.camera_2d.gameObject.GetComponent<ColorBlindFilter>().mode = 0;
        this.camera_vr.gameObject.GetComponent<ColorBlindFilter>().mode = 0;
    }
    public void RedGreenBlind()
    {
        this.camera_2d.gameObject.GetComponent<ColorBlindFilter>().mode = (ColorBlindMode)1;
        this.camera_vr.gameObject.GetComponent<ColorBlindFilter>().mode = (ColorBlindMode)1;
    }
    public void GreenBlind()
    {
        this.camera_2d.gameObject.GetComponent<ColorBlindFilter>().mode = (ColorBlindMode)3;
        this.camera_vr.gameObject.GetComponent<ColorBlindFilter>().mode = (ColorBlindMode)3;
    }
    public void BlueBlind()
    {
        this.camera_2d.gameObject.GetComponent<ColorBlindFilter>().mode = (ColorBlindMode)5;

        this.camera_vr.gameObject.GetComponent<ColorBlindFilter>().mode = (ColorBlindMode)5;
    }

    private bool IsGameplayBtnReady;
    public void GameplayBtn()
    {
        if (!this.IsGameplayBtnReady)
        {
            this.ResetSettingPage();
            this.arrPages[7].transform.position = Vector3.MoveTowards(this.arrPivot[1].position, this.arrPivot[2].position, this.pageSpeed);
            this.IsGameplayBtnReady = true;
        }
        else
        {
            this.ResetSettingPage();
            this.IsGameplayBtnReady = false;
        }
    }

    public Button[] arrLayoutBtn;

    public void LayoutBtn()
    {
        this.arrLayoutBtn[0].onClick.RemoveAllListeners();
        this.arrLayoutBtn[0].onClick.AddListener(() =>
        {
            foreach(var data in this.listStages)
            {
                data.songinfo.IsLayoutA = true;
            }
        });
        this.arrLayoutBtn[1].onClick.RemoveAllListeners();
        this.arrLayoutBtn[1].onClick.AddListener(() =>
        {
            foreach (var data in this.listStages)
            {
                data.songinfo.IsLayoutA = false;
            }
        });
    }

    private bool IsLanguageBtnReady;
    private bool IsLanguageReady;
    public LanguageText languageText;
    public Button[] arrLanguageBtn;

    public string country;

    public void SetLobbyLanguage()
    {
        this.languageText.LanguageTextInit();
        this.languageText.ChangeLobbyLanguage("ko");
    }
    public void LanguageBtn()
    {
        if (!this.IsLanguageBtnReady)
        {
            this.ResetSettingPage();
            this.arrPages[8].transform.position = Vector3.MoveTowards(this.arrPivot[1].position, this.arrPivot[2].position, this.pageSpeed);
            this.IsLanguageBtnReady = true;
        }
        else
        {
            this.ResetSettingPage();
            this.IsLanguageBtnReady = false;
        }
    }
    public void TranslateLanguage()
    {
        if (this.IsLanguageReady) return;
        //한국어
        this.arrLanguageBtn[0].onClick.RemoveAllListeners();
        this.arrLanguageBtn[0].onClick.AddListener(() =>
        {
            this.ChangeLanguage("ko");
        });
        //영어
        this.arrLanguageBtn[1].onClick.RemoveAllListeners();
        this.arrLanguageBtn[1].onClick.AddListener(() =>
        {
            this.ChangeLanguage("en");
        });
        //중국어
        this.arrLanguageBtn[2].onClick.RemoveAllListeners();
        this.arrLanguageBtn[2].onClick.AddListener(() =>
        {
            this.ChangeLanguage("cn");
        });
        this.IsLanguageReady = true;
    }
    public void ChangeLanguage(string country)
    {
        this.country = country;
        this.languageText.ChangeLobbyLanguage(country);
        foreach(var data in this.listStages)
        {
            data.songinfo.country = country;
        }
    }

    private bool IsMarketBtnReady;
    private void MarketBtn()
    {
        if(this.IsMarketBtnReady)
        {
            this.ResetPages();
            this.IsMarketBtnReady = false;
        }
        else
        {
            this.ResetPages();
            this.arrPages[3].transform.position = Vector3.MoveTowards(this.arrPivot[0].position, this.arrPivot[1].position, this.pageSpeed);
            this.IsMarketBtnReady = true;
        }
    }
}
