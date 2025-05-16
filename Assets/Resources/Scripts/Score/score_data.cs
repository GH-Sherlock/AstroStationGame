using System;
using UnityEngine;

[Serializable]
public class score_data
{
    public string score_name;
    public int score_amount;

    public score_data (string name, int amount)
    {
        this.score_name = name;
        this.score_amount = amount;
    }
}
