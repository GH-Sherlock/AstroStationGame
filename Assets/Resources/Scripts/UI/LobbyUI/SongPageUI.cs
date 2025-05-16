using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongPageUI : MonoBehaviour
{
    public Text[] arrSongText;
    public Button startBtn;
    public Text btnText;
    private string key;
    public string country;
    public bool IsLayoutA;

    public void SongPageInit(sound_data data, Transform[] arrT)
    {
        Debug.Log("송페이지 체크");
        //this.transform.position = arrT[2].position;
        this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        this.arrSongText[0].text = data.sound_bgm_name;
        this.arrSongText[1].text = data.sound_bgm_artist;
        this.arrSongText[2].text = data.sound_bgm_bpm.ToString();
        this.arrSongText[3].text = data.sound_bgm_difficulty;
        this.key = data.sound_key;
        this.IsLayoutA = true;
        //this.btnText.text = "게임시작";
        this.startBtn.onClick.RemoveAllListeners();
        this.startBtn.onClick.AddListener(() =>
        {
            this.btnEvent();
        });
    }
    public void btnEvent()
    {
        if (this.arrSongText[0] == null) return;
        SoundController.instance.bgmPlayer.loop = false;
        SoundController.instance.StopBGM();
        var operation = SceneManager.LoadSceneAsync("SampleScene");
        operation.completed += (a) =>
        {
            var tempIngame = GameObject.FindObjectOfType<GameManager>();
            // GameManagerInit() 함수에 data.sound_bgm_name을 파라미터로 보낸다
            // ex) tempIngame.GameManagerInit(data.sound_bgm_name)
            tempIngame.GameManagerInit(this.key, this.country, this.IsLayoutA);
        };
    }
}
