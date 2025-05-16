using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    public GameObject[] cometModelTypes;

    public GameObject cometModel;
    public GameObject fragment;

    public CharacterStatus cometStatus;
    public CharacterStatus fragmentStatus;

    private int id;
    public int scale;
    private int speed;
    private int angle;
    private int bar;
    private int sequence;

    public bool isMove;
    public float timer;
    public float checkTimer;

    private Vector3 moveDest;
    private float destDistance;

    private bool isDestroy;
    public bool isFragment;
    public bool isMakeFragment;
    public GameObject destroyFx;

    [Header("Sound")]
    public AudioClip destroySFX;


    public void CometInit(int id, int comet_scale, int comet_speed, int comet_angle, int comet_bar, int comet_sequence)
    {
        int typeLen = cometModelTypes.Length;
        int typeRndNumber = Random.Range(0, typeLen);

        cometModel = cometModelTypes[typeRndNumber];
        cometStatus = cometModelTypes[typeRndNumber].GetComponent<CharacterStatus>();

        this.id = id;
        this.scale = comet_scale;
        this.speed = comet_speed;
        this.angle = comet_angle;
        this.bar = comet_bar;
        this.sequence = comet_sequence;
        //this.status.hp = (float)this.scale;
        //this.cometStatus.hp = (float)Random.Range(1.0f, 5.0f);
        this.cometStatus.hp = (float)this.scale;
        this.fragmentStatus.hp = (float)this.scale;

        float tikTime = TimeManager.instance.GetMeasureTime();

        this.timer = (((float)this.bar) * (tikTime)) + ((((float)this.sequence) / (16f)) * (tikTime));
        isMakeFragment = false;
        isDestroy = false;
    }

    private void Update()
    {
        DamagedComet();

        if (this.isMove)
        {
            this.checkTimer += Time.deltaTime;

            if (this.checkTimer > this.timer)
            {
                if (this.isFragment == false) this.cometModel.SetActive(true);
                else this.cometModel.SetActive(false);

                //운석 이동
                this.transform.Translate(Vector3.down * TimeManager.instance.GetTimeScale() * this.speed * 5);
                cometModel.transform.Rotate(Vector3.right);

                // 배경 이동 보정
                float bgBoundhalfSize = GameManager.instance.GetBgBoundSizeX() / 2f;

                if (this.transform.position.x <= GameManager.instance.spaceBg[0].position.x - bgBoundhalfSize)
                {
                    float moveX = GameManager.instance.GetBgBoundSizeX() * 3f;

                    if (this.moveDest.x != this.transform.position.x + moveX)
                    {
                        this.MoveDestReset(moveX);
                    }
                    this.transform.Translate(moveX, 0f, 0f);
                }
                else if (this.transform.position.x >= GameManager.instance.spaceBg[2].position.x + bgBoundhalfSize)
                {
                    float moveX = GameManager.instance.GetBgBoundSizeX() * 3f;

                    if (this.moveDest.x != this.transform.position.x - moveX)
                    {
                        this.MoveDestReset(-moveX);
                    }
                    this.transform.Translate(-moveX, 0f, 0f);
                }
            }
        }

        if (this.gameObject != null && this.transform.position.y < 0)
        {
            GameManager.instance.CalculateScore(-50);
            this.TurnOffComet();
        }
    }

    public void MoveTo()
    {
        this.isMove = true;
        this.moveDest = GameManager.instance.GetDestPosition();
        this.destDistance = Vector3.Distance(this.transform.position, this.moveDest);
    }
    public void MoveDestReset(float moveX)
    {
        this.moveDest.x += moveX;
    }
    public void PauseComet()
    {
        this.isMove = false;
    }
    public void TurnOnComet()
    {
        this.isMove = true;
    }
    public void TurnOffComet()
    {
        this.isMove = false;
        this.checkTimer = 0f;
        this.gameObject.SetActive(false);
        cometModel.SetActive(false);
        fragment.SetActive(false);
    }

    public void DamagedComet()
    {
        if (isDestroy)
            return;

        if (this.cometStatus.hp <= 0)
        {
            isDestroy = true;

            if (isMakeFragment == true)
            {
                this.MakeFragment();
            }
            else
            {
                this.DestroyComet();
            }

            if (destroyFx)
            {
                GameObject eff = Instantiate(destroyFx, null);
                eff.transform.position = this.transform.position;
                Destroy(eff, 2f);
            }

            if(destroySFX)
            {
                SoundController.instance.PlaySFX(destroySFX);
            }
        }
    }
    public void DestroyComet()
    {
        this.isMove = false;
        GameManager.instance.CalculateScore(Random.Range(0, 100));
        this.checkTimer = 0f;
        this.TurnOffComet();
    }

    public void MakeFragment()
    {
        this.isMove = false;
        this.checkTimer = 0f;
        this.cometModel.SetActive(false);
        this.isFragment = true;
        this.fragment.gameObject.SetActive(true);
        this.fragment.GetComponent<Fragment>().fragmentInit();
        this.fragment.GetComponent<Fragment>().isMove = true;
        GameManager.instance.CalculateScore(Random.Range(0, 100));
    }
}
