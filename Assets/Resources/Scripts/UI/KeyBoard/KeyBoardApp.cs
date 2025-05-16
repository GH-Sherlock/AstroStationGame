using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardApp : MonoBehaviour
{
    public GameObject KeyBoard;
    public GameObject stick;

    public void TurnOnKeyBoard()
    {
        this.KeyBoard.SetActive(true);
        this.stick.SetActive(true);
    }
    public void TurnOffKeyBoard()
    {
        this.KeyBoard.SetActive(false);
        if (this.stick == null) return;
        this.stick.SetActive(false);
    }
}
