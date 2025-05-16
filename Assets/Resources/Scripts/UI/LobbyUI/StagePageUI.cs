using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePageUI : MonoBehaviour
{
    public Text btnText;
    public Button stageBtn;
    private List<StagePageUI> listStages;
    private sound_data data;
    public SongPageUI songinfo;
    private Transform[] arrT;
    private RankPageUI rank;
    public bool IsShow;
    public bool IsHide;

    public void StagePageInit(sound_data data, Transform[] arrT, GameObject parent)
    {
        this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Debug.Log("스테이지");
        if (this.IsShow) return;
        this.data = data;
        this.arrT = arrT;
        this.btnText.text = this.data.sound_bgm_name;
        var path = "Prefabs/UI/SongPageUI_2";
        var model = Resources.Load(path) as GameObject;
        var page = Instantiate<GameObject>(model, arrT[0].position, Quaternion.identity);
        this.songinfo = page.GetComponent<SongPageUI>();
        this.songinfo.transform.SetParent(parent.transform);
        this.songinfo.transform.localScale = new Vector3(1, 1, 1);
        this.songinfo.transform.position = arrT[0].position;
        this.songinfo.transform.rotation = new Quaternion(0f,0f,0f,0f);
        this.songinfo.SongPageInit(data, arrT);
        //this.SetListStages(this.listStages, this.rank.transform);

    }
    public void SetListStages(List<StagePageUI> stage, Transform tr)
    {
        
        this.listStages = new List<StagePageUI>();
        this.listStages = stage;
        this.rank = tr.GetComponent<RankPageUI>();

        this.stageBtn.onClick.RemoveAllListeners();
        this.stageBtn.onClick.AddListener(() =>
        {   
            //this.btnEvent();
        });
    }
    public void btnEvent()
    {   
        if (!this.IsShow)
        {   
            this.ResetStagePage();
            //this.songinfo.transform.position = Vector3.MoveTowards(arrT[1].position, arrT[2].position, 10f);
            this.songinfo.transform.position = arrT[2].position;
            this.rank.RankPageInit();
            //this.rank.transform.position = Vector3.MoveTowards(arrT[2].position, arrT[3].position, 10f);
            this.rank.transform.position = arrT[3].position;
            this.IsShow = true;
            this.IsHide = false;
        }
        else
        {
           
            if (!this.IsHide)
            {
                this.ResetStagePage();
            }
            else
            {
                
                this.ResetStagePage();
                this.songinfo.transform.position = Vector3.MoveTowards(arrT[1].position, arrT[2].position, 10f);
                this.rank.transform.position = Vector3.MoveTowards(arrT[2].position, arrT[3].position, 10f);
                this.songinfo.transform.position = arrT[2].position;
                this.rank.transform.position = arrT[3].position;
                this.IsHide = false;
            }
        }
    }

    public void ResetStagePage()
    {
        foreach (var x in this.listStages)
        {
            x.songinfo.transform.position = arrT[0].position;
            x.rank.transform.position = arrT[0].position;
            x.IsHide = true;
        }
    }
}
