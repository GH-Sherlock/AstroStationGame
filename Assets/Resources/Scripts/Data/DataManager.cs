using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class DataManager
{
    private static DataManager instance;
    private Dictionary<int, sound_data> dicSoundData;
    private Dictionary<string, object> dicStageData;
    private Dictionary<int, comet_data> dicCometData;
    private List<score_data> listScoreData;
    private Dictionary<int, language_data> dicLanguageData;
    public bool initDone;

    public static DataManager GetInstance()
    {
        if (DataManager.instance == null)
        {
            DataManager.instance = new DataManager();
        }
        return DataManager.instance;
    }
    public void DataManagerInit()
    {
        Debug.Log("DataManager");

        //언어 : id가 100000, 십만 부터 , 한국어+영어+중국어 csv파일
        if (this.dicLanguageData == null)
        {
            this.dicLanguageData = new Dictionary<int, language_data>();
        }

        //string jsonPath = Application.persistentDataPath + "/Assets/Resources/Data/language_data.csv";
        //string[] arrlanguage = File.ReadAllLines(jsonPath);
        //string[] arrlanguage = Directory.GetFiles(jsonPath);

        //-------------------------------------------------------------------------------------------------
        /*
        string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Data/language_data.csv");
        WWW reader = new WWW(oriPath);
        while (!reader.isDone) { }

        string jsonPath = Application.persistentDataPath + "/db";
        System.IO.File.WriteAllBytes(jsonPath, reader.bytes);

        string[] arrlanguage = File.ReadAllLines(jsonPath);

        for (int i = 1; i < arrlanguage.Length; i++)
        {
            string[] tempdata = arrlanguage[i].Split(',');
            int id = int.Parse(tempdata[0]);
            string key = tempdata[1];
            string ko = tempdata[2];
            string en = tempdata[3];
            string cn = tempdata[4];
            language_data templan = new language_data(id, key, ko, en, cn);
            this.dicLanguageData.Add(templan.id, templan);
        }
        */
        //-----------------------------------------------------------------------------
        string jsonPath = "Data/language_data";
        TextAsset sourcefile = Resources.Load<TextAsset>(jsonPath);
        StringReader strReader = new StringReader(sourcefile.text);
        //첫째줄 제거, 둘째줄부터 데이터시작
        strReader.ReadLine();
        while (strReader.Peek() > -1)
        {
            string strlanguage = strReader.ReadLine().ToString();

            string[] tempdata = strlanguage.Split(',');
            int id = int.Parse(tempdata[0]);
            string key = tempdata[1];
            string ko = tempdata[2];
            string en = tempdata[3];
            string cn = tempdata[4];
            language_data templan = new language_data(id, key, ko, en, cn);
            this.dicLanguageData.Add(templan.id, templan);
        }
        /*
        //언어데이터 확인
        foreach(var pair in this.dicLanguageData)
        {
            var data = pair.Value;
            Debug.LogFormat("{0},{1},{2},{3}", data.lan_key,data.lan_ko, data.lan_en, data.lan_cn);
        }
        */
        //사운드 : id가 1000, 천 부터 json 파일
        if (this.dicSoundData == null)
        {
            this.dicSoundData = new Dictionary<int, sound_data>();
        }
        jsonPath = "Data/sound_data";
        TextAsset soundPath = Resources.Load(jsonPath) as TextAsset;
        sound_data[] arrSoundData = JsonConvert.DeserializeObject<sound_data[]>(soundPath.text);
        this.dicSoundData = arrSoundData.ToDictionary(x => x.id, x => x);
        /*
        foreach(var pair in this.dicSoundData)
        {
            var data = pair.Value;
            Debug.LogFormat("{0},{1},{2},{3},{4}", data.sound_key, data.sound_bgm_name, data.sound_bgm_difficulty, data.sound_bgm_bpm, data.sound_bgm_artist);
        }
        */
        //운석 : id가 10000, 만 부터 json파일
        if (this.dicCometData == null)
        {
            this.dicCometData = new Dictionary<int, comet_data>();
        }
        if (this.dicStageData == null)
        {
            this.dicStageData = new Dictionary<string, object>();
        }
        foreach(var pair in this.dicSoundData)
        {
            var key = pair.Key;
            var data = pair.Value;
            jsonPath = "Data/comet_data_" + data.sound_key;
            TextAsset cometPath = Resources.Load(jsonPath) as TextAsset;
            comet_data[] arrCometData = JsonConvert.DeserializeObject<comet_data[]>(cometPath.text);
            this.dicCometData = arrCometData.ToDictionary(x => x.id, x => x);
            this.dicStageData.Add(data.sound_key, this.dicCometData);
        }
        /*
        jsonPath = "Data/comet_data_astro_hard";
        TextAsset cometPath = Resources.Load(jsonPath) as TextAsset;
        comet_data[] arrCometData = JsonConvert.DeserializeObject<comet_data[]>(cometPath.text);
        this.dicCometData = arrCometData.ToDictionary(x => x.id, x => x);
        */
        /*
        foreach (var pair in this.dicCometData)
        {
            var data = pair.Value;
            Debug.LogFormat("{0},{1}", data.comet_bar, data.comet_sequence);
        }
        */
        /*
        foreach (var pair in this.dicStageData)
        {   
            var d = pair.Value;
            Debug.LogFormat("{0},{1}", pair.Key, d);
            foreach(var e in d as Dictionary<int, comet_data>)
            {
                var f = e.Value;
                Debug.LogFormat("{0},{1}", f.comet_bar, f.comet_sequence);
            }
        }
        */
        //랭킹 : id없이 이름과 점수만(점수순서대로) json파일
        if (this.listScoreData == null)
        {
            this.listScoreData = new List<score_data>();
        }
        if (File.Exists(Application.streamingAssetsPath + "/Data/score_data1.json"))
        {
            string scoreSource = File.ReadAllText(Application.streamingAssetsPath + "/Data/score_data1.json");
            score_data[] arrScoreData = JsonConvert.DeserializeObject<score_data[]>(scoreSource);
            this.listScoreData.Clear();
            this.listScoreData = arrScoreData.ToList();
        }
        else
        {
            jsonPath = "Data/score_data1";
            TextAsset scorePath = Resources.Load(jsonPath) as TextAsset;
            score_data[] arrScoreData = JsonConvert.DeserializeObject<score_data[]>(scorePath.text);
            this.listScoreData.Clear();
            this.listScoreData = arrScoreData.ToList();
        }
            this.initDone = true;
    }
    public Dictionary<int, language_data> GetLanguageData()
    {
        return this.dicLanguageData;
    }
    public Dictionary<int, sound_data> GetDicSoundData()
    {
        return this.dicSoundData;
    }
    public Dictionary<int, comet_data> GetDicCometData()
    {
        return this.dicCometData;
    }
    public Dictionary<string, object> GetDicStageData()
    {
        return this.dicStageData;
    }
    public List<score_data> GetListScoreData()
    {
        return this.listScoreData;
    }
    public void UpdateScore(IEnumerable<score_data> query)
    {
        this.listScoreData.Clear();
        Debug.Log(this.listScoreData.Count);
        foreach (var data in query)
        {
            this.listScoreData.Add(data);
        }
        this.SaveScore();
    }
    public void SaveScore()
    {
        string jsonData = JsonConvert.SerializeObject(this.listScoreData);
        string path = Application.streamingAssetsPath + "/Data/score_data1.json";
        File.WriteAllText(path, jsonData);
        string scoreSource = File.ReadAllText(path);
        score_data[] arrScoreData = JsonConvert.DeserializeObject<score_data[]>(scoreSource);
        this.listScoreData.Clear();
        this.listScoreData = arrScoreData.ToList();
    }
}
