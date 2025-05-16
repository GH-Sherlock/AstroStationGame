using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CometModel : MonoBehaviour
{
    public GameObject root;
    public GameObject crashFX;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 다음식의 코사인 cos 값이 15/16(0.9375)보다 크거나 같을때 파편이 생긴다.
        Vector2 a = collision.transform.position;
        float cos = Vector2.Dot(Vector2.down.normalized, (a - (Vector2)this.transform.position).normalized);

        Comet tempComet = this.root.GetComponent<Comet>();
        bool isFragment = false;

        if (collision.gameObject.CompareTag("MakingFragmentWeapon"))
        {
            tempComet.isMakeFragment = true;
        }
        else if (collision.gameObject.CompareTag("Weapon"))
        {   
            if (cos >= 0.9375f &&  tempComet.scale > 4)
            {
                //Debug.LogFormat("코사인값 : {0}", cos);
                tempComet.isMakeFragment = true;
            }
            else
            {
                tempComet.isMakeFragment = false;
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //GameManager.instance.InGameUI.GetComponent<InGameUI>().OpenDefeatUI();
            collision.GetComponent<CharacterStatus>().Damaged(1);

            tempComet.TurnOffComet();
            if (crashFX)
            {
                GameObject eff = Instantiate(crashFX, null);
                eff.transform.position = tempComet.transform.position;
                Destroy(eff, 2f);
            }

            QuestController.instance.AllVibration(0.2f, 0.1f, 0.5f);
            Camera.main.GetComponentInChildren<HmdHitEffect>().HitScreenOn();
        }
    }
}