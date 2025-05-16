using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform playerShip;

    public Transform[] spaceBg;

    public EnermyPool enermyPool;

    private float spriteBoundSizeX;

    public float MainWeaponMaxLevel = 4f;
    public float SubWeaponMaxLevel = 3f;

    public string soundKey;
    public string country;

    public GameObject InGameUI;

    private float gameEndTime;
    private float elapsedGamePlayTime;

    /*
    // Start is called before the first frame update
    void Start()
    {
        spriteBoundSizeX = spaceBg[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        //임시
        DataManager.GetInstance().DataManagerInit();
        SoundController.instance.SoundControllerInit();
        SoundController.instance.PlayBGM(DataManager.GetInstance().GetDicSoundData()[1000].sound_bgm_name);
        TimeManager.instance.TimeManagerInit(DataManager.GetInstance().GetDicSoundData()[1000].sound_bgm_name);
        Debug.Log(TimeManager.instance.GetMeasureTime());
        CometController.instance.CometControllerInit();
        this.ScoreCalculatorInit();
    }
    */
    public void GameManagerInit(string key, string country, bool layoutA)
    {
        Debug.Log("GameManager");
        this.soundKey = key;
        this.country = country;
        spriteBoundSizeX = spaceBg[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        //DataManager.GetInstance().DataManagerInit();
        //SoundController.instance.SoundControllerInit();
        TimeManager.instance.TimeManagerInit(this.soundKey);
        Debug.Log(TimeManager.instance.GetMeasureTime());
        CometController.instance.CometControllerInit(this.soundKey);
        this.ScoreCalculatorInit();
        this.InGameUI.GetComponent<InGameUI>().InGameUIinit(this.country);
        this.playerShip.GetComponent<PlayerShipController>().IsLayoutA = layoutA;

        //게임 종료시간
        elapsedGamePlayTime = 0f;
        gameEndTime = SoundController.instance.BGMPlayTime();

    }

    private void Update()
    {
        if (elapsedGamePlayTime > gameEndTime)
        {
            Debug.Log("게임종료");
            GameManager.instance.InGameUI.GetComponent<InGameUI>().OpenVictoryUI();
            return;
        }

        elapsedGamePlayTime += TimeManager.instance.GetTimeScale();
    }

    public void BGLeftSort()
    {
        spaceBg[2].Translate(new Vector3(-spriteBoundSizeX * 3f, 0f, 0f));

        Transform tmp = spaceBg[2];
        spaceBg[2] = spaceBg[1];
        spaceBg[1] = spaceBg[0];
        spaceBg[0] = tmp;
    }

    public void BGRightSort()
    {
        spaceBg[0].Translate(new Vector3(spriteBoundSizeX * 3f, 0f, 0f));

        Transform tmp = spaceBg[0];
        spaceBg[0] = spaceBg[1];
        spaceBg[1] = spaceBg[2];
        spaceBg[2] = tmp;
    }

    public void PlayerShipPositionCheck()
    {
        if (playerShip.position.x <= spaceBg[1].position.x - spriteBoundSizeX / 2)
        {
            enermyPool.EnermyPositionSort(spaceBg[2].position.x - (spriteBoundSizeX / 2),
                spaceBg[2].position.x + (spriteBoundSizeX / 2),
                -spriteBoundSizeX * 3f);
            BGLeftSort();

        }

        if (playerShip.position.x >= spaceBg[1].position.x + spriteBoundSizeX / 2)
        {
            enermyPool.EnermyPositionSort(spaceBg[0].position.x - (spriteBoundSizeX / 2),
                spaceBg[0].position.x + (spriteBoundSizeX / 2),
                spriteBoundSizeX * 3f);
            BGRightSort();
        }
    }


    public float GetBgBoundSizeX()
    {
        return spriteBoundSizeX;
    }

    /// <summary>
    /// 오브젝트 이동이 필요할때 목적지 값을 리턴
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDestPosition()
    {
        float x = Random.Range(spaceBg[0].transform.position.x - (spriteBoundSizeX / 2),
               spaceBg[2].transform.position.x + (spriteBoundSizeX / 2));

        float y = Random.Range(10f, 32f);

        return new Vector2(x, y);
    }

    public void PlayerReset()
    {
        playerShip.GetComponent<PlayerShipController>().PlayerReset();
    }


    //------------------아래부터 최승룡 추가------------------

    /*
     ScoreCalculator 스크립트를 GameManager에 합치는중

     반드시 DataManager를 init 한 후에 ScoreCalculator를 init한다.

     기본점수 1만

     남은 생명 1개당 +1천
     남은 폭탄 1개당 +5백
     Main Weapon의 업그래이드가 끝까지 완료된 후 아이템을 먹었을때, 먹은 아이탬 1개당 +1백
     Sub Weapon의 업그래이드가 끝까지 완료된 후 아이템을 먹었을때, 먹은 아이탬 1개당 +1백

     운석을 파괴하지 못하고 피해서 아래로 떨어졌을때, 파괴하지 못한 운석 1개당 -50점

     운석에 가한 데미지 1당 +1점
     중형 운석 체력 5 >>> 파괴시 5점
     소형 운석 체력 1 >>> 파괴시 1점

     일단 로컬로 저장해서 1~10등까지 순위를 보여준다.
    */

    private int currentScore = 0;
    private int basicScore = 10000;
    private string playerName = null;
    private List<score_data> listScoreData = null;

    //매판 시작직전 초기화 해주자
    public void ScoreCalculatorInit()
    {
        Debug.Log("ScoreCalculator");

        this.playerName = "ABC"; // 언제 플레이어의 이름을 입력할지 생각해보자

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

    // 점수 계산용 함수
    public void CalculateScore(int score)
    {
        this.currentScore = this.currentScore + score;
        //Debug.Log(this.currentScore);
    }

    // UI출력용 함수
    public string GetCurrentScore()
    {   
        return this.currentScore.ToString();
    }

    // 랭킹 계산용 함수
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
    }
    public string SetPlayerName(string name)
    {
        this.playerName = name;
        return this.playerName;
    }
    public void ResetScore()
    {
        this.currentScore = 10000;
    }
    
}
