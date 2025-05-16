using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreCalculator : Singleton<ScoreCalculator>
{

    // ScoreCalculator 스크립트는 게임메니저 스트립트와 합쳐져야함

    // 기본점수 1만

    // 남은 생명 1개당 +1천
    // 남은 폭탄 1개당 +5백
    // Main Weapon의 업그래이드가 끝까지 완료된 후 아이템을 먹었을때, 먹은 아이탬 1개당 +1백
    // Sub Weapon의 업그래이드가 끝까지 완료된 후 아이템을 먹었을때, 먹은 아이탬 1개당 +1백

    // 운석을 파괴하지 못하고 피해서 아래로 떨어졌을때, 파괴하지 못한 운석 1개당 -50점

    // 운석에 가한 데미지 1당 +1점
    // 중형 운석 체력 5 >>> 파괴시 5점
    // 소형 운석 체력 1 >>> 파괴시 1점

    // 일단 로컬로 저장해서 1~10등까지 순위를 보여준다.

    private int currentScore = 0;
    private int basicScore = 10000;
    private string playerName = null;
    private List<score_data> listScoreData = null;


    public void ScoreCalculatorInit()
    {
        Debug.Log("ScoreCalculator");
        this.playerName = "unknown";
        if (this.listScoreData == null)
        {
            this.listScoreData = new List<score_data>();
        }
        var tempList = DataManager.GetInstance().GetListScoreData();
        this.listScoreData.Clear();
        foreach (var data in tempList)
        {
            this.listScoreData.Add(data);
        }
        this.currentScore = 0;
        this.basicScore = 10000;
        this.currentScore = this.basicScore;
    }

    public void CalculateScore(int score)
    {
        this.currentScore = this.currentScore + score;
        Debug.Log(this.currentScore);
    }
    public string GetCurrentScore()
    {
        return this.currentScore.ToString();
    }
    public List<score_data> GetListScoreData()
    {
        return this.listScoreData;
    }
    public void CalculateRank()
    {
        //Debug.Log("-------------------------------------------------------------------------");
        score_data tempScore = new score_data(this.playerName, this.currentScore);
        this.listScoreData.Add(tempScore);
        IEnumerable<score_data> query = this.listScoreData.OrderByDescending(x => x.score_amount);
        DataManager.GetInstance().UpdateScore(query);
        /*
        foreach (var data in query)
        {
            Debug.LogFormat("이름 : {0}, 점수 : {1}", data.score_name, data.score_amount);
        }
        */
    }
}
