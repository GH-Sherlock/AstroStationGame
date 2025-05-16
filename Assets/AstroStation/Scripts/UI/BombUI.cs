using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour
{
    public RawImage[] bombIcon;

    public void BombIconUpdate(float hp)
    {
        if (bombIcon.Length <= 0)
            return;

        for (int i = 0; i < bombIcon.Length; i++)
        {
            if (i < hp)
            {
                bombIcon[i].enabled = true;
            }
            else
            {
                bombIcon[i].enabled = false;
            }

        }
    }
}
