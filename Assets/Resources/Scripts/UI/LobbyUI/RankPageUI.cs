using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPageUI : MonoBehaviour
{
    public Text[] score;
    public Text[] name;

    public void RankPageInit()
    {
        var templist = DataManager.GetInstance().GetListScoreData();
        int i = 0;
        foreach (var data in templist)
        {
            this.score[i].text = data.score_amount.ToString();
            this.name[i].text = data.score_name;
            i++;
            if (i == 6) break;
        }
    }
}
