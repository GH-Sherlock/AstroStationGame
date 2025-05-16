using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    public RawImage[] hpIcon;

    public void HpIconUpdate(float hp)
    {
        if (hpIcon.Length <= 0)
            return;

        for(int i = 0; i < hpIcon.Length; i++)
        {
            if(i < hp)
            {
                hpIcon[i].enabled = true;
            }
            else
            {
                hpIcon[i].enabled = false;
            }

        }
    }
}
