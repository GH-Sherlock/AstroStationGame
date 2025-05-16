using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameStart : MonoBehaviour
{
    private void Awake()
    {
        DataManager.GetInstance().DataManagerInit();
        SoundController.instance.SoundControllerInit();
        GameManager.instance.GameManagerInit("astro_main", "lan_ko", true);
    }
}
