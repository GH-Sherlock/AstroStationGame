using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public List<Text> listLobbyLanguageText;
    public List<Text> listIngameLanguageText;
    private Dictionary<int, language_data> dicLanguageData;

    private string country;

    public void LanguageTextInit()
    {
        if(this.dicLanguageData == null)
        {
            this.dicLanguageData = DataManager.GetInstance().GetLanguageData();
        }
    }

    public void ChangeLobbyLanguage(string country)
    {
        Debug.Log(this.listLobbyLanguageText.Count);
        this.country = country;
        switch(this.country)
        {
            case "ko":
                {
                    foreach(var pair in this.dicLanguageData)
                    {
                        var data = pair.Value;
                        for (int i = 0; i < this.listLobbyLanguageText.Count; i++)
                        {
                            if(this.listLobbyLanguageText[i].name == data.lan_key)
                            {
                                this.listLobbyLanguageText[i].text = data.lan_ko;
                            }
                        }
                    }
                    break;
                }
            case "en":
                {
                    foreach (var pair in this.dicLanguageData)
                    {
                        var data = pair.Value;
                        for (int i = 0; i < this.listLobbyLanguageText.Count; i++)
                        {
                            if (this.listLobbyLanguageText[i].name == data.lan_key)
                            {
                                this.listLobbyLanguageText[i].text = data.lan_en;
                            }
                        }
                    }
                    break;
                }
            case "cn":
                {
                    foreach (var pair in this.dicLanguageData)
                    {
                        var data = pair.Value;
                        for (int i = 0; i < this.listLobbyLanguageText.Count; i++)
                        {
                            if (this.listLobbyLanguageText[i].name == data.lan_key)
                            {
                                this.listLobbyLanguageText[i].text = data.lan_cn;
                            }
                        }
                    }
                    break;
                }
        }
    }

    public void ChangeIngameLanguage(string country)
    {
        Debug.Log(this.listIngameLanguageText.Count);
        this.country = country;
        switch (this.country)
        {
            case "ko":
                {
                    foreach (var pair in this.dicLanguageData)
                    {
                        var data = pair.Value;
                        for (int i = 0; i < this.listIngameLanguageText.Count; i++)
                        {
                            if (this.listIngameLanguageText[i].name == data.lan_key)
                            {
                                this.listIngameLanguageText[i].text = data.lan_ko;
                            }
                        }
                    }
                    break;
                }
            case "en":
                {
                    foreach (var pair in this.dicLanguageData)
                    {
                        var data = pair.Value;
                        for (int i = 0; i < this.listIngameLanguageText.Count; i++)
                        {
                            if (this.listIngameLanguageText[i].name == data.lan_key)
                            {
                                this.listIngameLanguageText[i].text = data.lan_en;
                            }
                        }
                    }
                    break;
                }
            case "cn":
                {
                    foreach (var pair in this.dicLanguageData)
                    {
                        var data = pair.Value;
                        for (int i = 0; i < this.listIngameLanguageText.Count; i++)
                        {
                            if (this.listIngameLanguageText[i].name == data.lan_key)
                            {
                                this.listIngameLanguageText[i].text = data.lan_cn;
                            }
                        }
                    }
                    break;
                }
        }
    }
}
