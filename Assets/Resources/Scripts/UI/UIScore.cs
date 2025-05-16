using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public Text text;

    void Update()
    {
        this.text.text = GameManager.instance.GetCurrentScore();
    }
}
