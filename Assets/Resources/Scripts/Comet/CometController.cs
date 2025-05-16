using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CometController : Singleton<CometController>
{
    public Transform cometGroup;
    public Transform AstGroup;

    public Vector3 cometPosition;
    public Dictionary<int, comet_data> dicCometData;
    public List<GameObject> listComet;
    public List<GameObject> listFragment;

    private string key;

    public void CometControllerInit(string key)
    {
        Debug.Log("CometController");

        this.cometPosition = Vector3.zero;
        this.key = key;

        if (this.dicCometData == null)
        {
            this.dicCometData = new Dictionary<int, comet_data>();
        }
        this.dicCometData.Clear();

        if (this.listComet == null)
        {
            this.listComet = new List<GameObject>();
        }
        else
        {
            foreach (var data in this.listComet)
            {
                Destroy(data);
            }
        }
        this.listComet.Clear();

        this.SetComet(this.key);
    }

    private void SetComet(string bgmName)
    {
        TimeManager.instance.TimeManagerInit(bgmName);
        /*
        var tempdic = DataManager.GetInstance().GetDicStageData();
        foreach(var pair in tempdic)
        {
            var key = pair.Key;
            var data = pair.Value as Dictionary<int, comet_data>;
            if(key == bgmName )
            {
                this.dicCometData = data;
            }
        }
        */
        foreach (var pair in DataManager.GetInstance().GetDicStageData())
        {
            var d = pair.Value;
            //Debug.LogFormat("{0},{1}", pair.Key, d);
            if (pair.Key == bgmName)
            {
                var a = d as Dictionary<int, comet_data>;
                /*
                foreach (var e in a)
                {
                    var f = e.Value;
                    Debug.LogFormat("{0},{1}", f.comet_bar, f.comet_sequence);
                }
                */
                this.dicCometData = a;
                break;
            }
        }
        int id = 10000;
        for (int i = 0; i < this.dicCometData.Count; i++)
        {
            var path = "Prefabs/Comet";
            var model = Resources.Load(path) as GameObject;
            var tempComet = this.dicCometData[id + i];

            this.cometPosition = new Vector3(3 * tempComet.comet_angle, 32, 0);

            var comet = Instantiate<GameObject>(model, this.cometPosition, Quaternion.identity);

            comet.transform.SetParent(this.cometGroup, false);
            var cometScript = comet.GetComponent<Comet>();
            cometScript.CometInit(
                tempComet.id,
                tempComet.comet_scale,
                tempComet.comet_speed,
                tempComet.comet_angle,
                tempComet.comet_bar,
                tempComet.comet_sequence);
            this.listComet.Add(comet);
            this.listFragment.Add(comet.transform.GetChild(1).gameObject);
            cometScript.fragment.SetActive(false);
            cometScript.cometModel.SetActive(false);
        }
        TimeManager.instance.Resume();
        SoundController.instance.PlayBGM(bgmName);
        foreach(var data in listComet)
        {
            data.GetComponent<Comet>().TurnOnComet();
        }
    }
    public List<GameObject> GetComet()
    {
        return this.listComet;
    }
    public List<GameObject> GetFragment()
    {
        return this.listFragment;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var data in this.listComet)
            {
                data.GetComponent<Comet>().TurnOnComet();
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var data in this.listComet)
            {
                GameManager.instance.CalculateRank();
            }
        }
    }

    public void AllDisableFragment()
    {
        foreach (GameObject f in listFragment)
        {
            if (f.activeSelf)
            {
                f.SetActive(false);
            }
        }
    }

    public List<Transform> GetActiveComet()
    {
        List<Transform> result = new List<Transform>();

        foreach (GameObject c in listComet)
        {
            if (c.GetComponent<Comet>().cometModel.activeSelf)
                result.Add(c.GetComponent<Comet>().cometModel.transform);
        }

        return result;
    }

    public List<Transform> GetActiveFragment()
    {
        List<Transform> result = new List<Transform>();

        foreach (GameObject f in listFragment)
        {
            if (f.gameObject.activeSelf)
                result.Add(f.transform);
        }

        return result;
    }

    public void ResetComet(string key)
    {
        foreach(var data in this.listComet)
        {
            Destroy(data);
        }
        foreach(var data in this.listFragment)
        {
            Destroy(data);
        }
        this.listComet.Clear();
        this.listFragment.Clear();
        this.SetComet(key);
    }
}
