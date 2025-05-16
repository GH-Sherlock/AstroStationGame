using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public enum ITEM_TYPE
    {
        MainWeapon,
        SubWeapon,
        HP,
        Bomb,
        NONE
    }

    public GameObject[] item;
    private ITEM_TYPE itemType;

    private bool isMove = false;
    private Vector3 moveDest;

    private float moveDistance;
    private float destDistance = 32f;

    private float speed = 2f;

    private void Awake()
    {
        EnableItem();
    }

    // Update is called once per frame
    void Update()
    {

        //이동
        if (moveDistance >= destDistance)
        {
            //isMove = false;
            //MoveTo();
            Destroy(this.gameObject);
            moveDistance = 0f;
            return;
        }

        Vector3 dir = moveDest - this.transform.position;

        //this.transform.Translate(dir.normalized * TimeManager.instance.GetTimeScale() * speed);
        this.transform.Translate(Vector2.down * TimeManager.instance.GetTimeScale() * speed);
        moveDistance += Vector3.Magnitude(Vector2.down * TimeManager.instance.GetTimeScale() * speed);

        float bgBoundhalfSize = GameManager.instance.GetBgBoundSizeX() / 2f;

        if (this.transform.position.x <= GameManager.instance.spaceBg[0].position.x - bgBoundhalfSize)
        {
            float moveX = GameManager.instance.GetBgBoundSizeX() * 3f;

            if (moveDest.x != this.transform.position.x + moveX)
            {
                MoveDestReset(moveX);
            }
            this.transform.Translate(moveX, 0f, 0f);
        }
        else if (this.transform.position.x >= GameManager.instance.spaceBg[2].position.x + bgBoundhalfSize)
        {
            float moveX = GameManager.instance.GetBgBoundSizeX() * 3f;

            if (moveDest.x != this.transform.position.x - moveX)
            {
                MoveDestReset(-moveX);
            }
            this.transform.Translate(-moveX, 0f, 0f);
        }
    }

    public void MoveDestReset(float moveX)
    {
        moveDest.x += moveX;
    }

    public void EnableItem()
    {
        int rnd = Random.Range(0, item.Length);

        switch(rnd)
        {
            case 0:
                item[0].SetActive(true);
                itemType = ITEM_TYPE.MainWeapon;
                break;
            case 1:
                item[1].SetActive(true);
                itemType = ITEM_TYPE.SubWeapon;
                break;
            case 2:
                item[2].SetActive(true);
                itemType = ITEM_TYPE.HP;
                break;
            case 3:
                item[3].SetActive(true);
                itemType = ITEM_TYPE.Bomb;
                break;
        }
    }
}
