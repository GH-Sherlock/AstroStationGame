using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Astroid : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private TrailRenderer trailRenderer;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 dir;
    private float dis;
    private float currentDis;
    private bool isMove;
    private float elapsedTime;

    public void AstroidInit(Vector3 start, Vector3 end)
    {
        this.startPos = start;
        this.endPos = end + (Random.insideUnitSphere*0.3f);
        
        this.lineRenderer = this.GetComponent<LineRenderer>();
        //this.trailRenderer = this.GetComponent<TrailRenderer>();
        this.lineRenderer.enabled = true;
        //this.trailRenderer.enabled = false;
        
        this.lineRenderer.startWidth = 0.1f;
        this.lineRenderer.endWidth = 0.1f;
        //this.trailRenderer.startWidth = 0.01f;
        //this.trailRenderer.endWidth = 0.01f;

        this.lineRenderer.SetPosition(0, this.startPos);
        this.lineRenderer.SetPosition(1, this.endPos);
        

        this.dis = Vector3.Distance(this.startPos, this.endPos);
        this.dir = (this.endPos - this.startPos).normalized;
        this.elapsedTime = 0f;
        this.isMove = true;
    }
    private void Update()
    {
        if (this.isMove)
        {
            this.elapsedTime = this.elapsedTime + Time.deltaTime;
            this.currentDis = Vector3.Distance(this.transform.position, this.endPos);
            this.transform.Translate(this.dir * TimeManager.instance.GetTimeScale() * CalculateSpeed(this.currentDis));
        }

        if(elapsedTime >= 3f)
        {
            this.lineRenderer.enabled = false;
        }

        if (this.elapsedTime >= 7f)
        {
            this.isMove = false;
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
    private float CalculateSpeed(float x)
    {
        float speed = Mathf.Pow(2f, (this.dis - x) / 2f) - 0.5f;
        return speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.lineRenderer.enabled = false;
            GameManager.instance.playerShip.GetComponent<CharacterStatus>().Damaged(5);
            QuestController.instance.AllVibration(1f, 0.1f, 0.5f);
            Destroy(this.gameObject);
        }
    }
}
