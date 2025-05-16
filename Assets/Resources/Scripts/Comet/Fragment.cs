using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Fragment : MonoBehaviour
{
    public bool isMove = false;
    private float ranDirection;

    private Vector3 moveDest;
    private float destDistance;

    public void fragmentInit()
    {
        this.ranDirection = Random.Range(0f, 1f);
    }

    private void Update()
    {
        if (this.isMove)
        {
            if (this.ranDirection <= 0.5f)
            {
                this.transform.Translate(new Vector3(1* Random.Range(0f, 1f), -2 * Random.Range(1f, 2f), 0) * TimeManager.instance.GetTimeScale());

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
            else
            {
                this.transform.Translate(new Vector3(-1 * Random.Range(0f, 1f), -2 * Random.Range(1f, 2f), 0) * TimeManager.instance.GetTimeScale());

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

        if (this.transform.position.y < 0.0f)
        {
            this.StopFragment();
            this.gameObject.SetActive(false);
            this.transform.parent.gameObject.SetActive(false);
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
    public void MoveFragment()
    {
        this.isMove = true;
    }
    public void StopFragment()
    {
        this.isMove = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            Transform Anchor = Camera.main.transform.GetChild(1);

            Vector2 a = Random.insideUnitCircle;
            float x = a.x * 1.3f;
            float y = a.y * 1.1f;
           
            Vector3 startPos = new Vector3(x, y, 0f) + Anchor.position;
            Vector3 endPos = new Vector3(0f, 0.2f, 0f) + Camera.main.transform.position;

            var model = Resources.Load<GameObject>("Prefabs/Astroid");
            var astroid = Instantiate<GameObject>(model, startPos, Quaternion.identity);
            astroid.transform.SetParent(this.transform.root.GetChild(1),false);
            astroid.GetComponent<Astroid>().AstroidInit(startPos, endPos);

            QuestController.instance.AllVibration(2f, 0.1f, 0.5f);
            Camera.main.GetComponentInChildren<HmdHitEffect>().HitScreenOn();
        }
    }
}
