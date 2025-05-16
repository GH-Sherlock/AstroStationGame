using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text text;

    void Update()
    {
        this.text.text = GameManager.instance.GetCurrentScore();
    }
}
