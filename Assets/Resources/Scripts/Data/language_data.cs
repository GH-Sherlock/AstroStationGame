using System;
using UnityEngine;

[Serializable]
public class language_data
{
    public int id;
    public string lan_key;
    public string lan_ko;
    public string lan_en;
    public string lan_cn;
    public language_data(int id, string key, string ko, string en, string cn)
    {
        this.id = id;
        this.lan_key = key;
        this.lan_ko = ko;
        this.lan_en = en;
        this.lan_cn = cn;
    }
}
