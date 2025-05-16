using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceship : MonoBehaviour
{
    public GameObject ship;
    public Transform[] arrTr;
    public float elapsedTime = 0f;
    public bool IsStop = false;
    public float checkTime = 40f;
    public Transform[] arrPlanet;

    public Transform comet;
    public Transform ddddd;

    private void Start()
    {
        
        //iTween.PutOnPath(this.comet.gameObject, arrTr, 100f);
    }

    private void FixedUpdate()
    {
        this.shipRotation();

        //iTween.MoveTo(this.comet.gameObject, arrTr[0].position, 10f);
        this.elapsedTime = this.elapsedTime + Time.deltaTime;
        //iTween.PutOnPath(this.comet.gameObject, arrTr, this.elapsedTime/ 30f);
        this.arrPlanet[1].transform.RotateAround(arrPlanet[0].position, Vector3.up, 0.5f);
        this.arrPlanet[3].transform.RotateAround(arrPlanet[0].position, Vector3.down, 0.5f);
        this.ddddd.RotateAround(this.transform.root.transform.position ,Vector3.up, 0.3f);
    }

    public void onPath()
    {
        iTween.PutOnPath(this.gameObject, arrTr, this.elapsedTime / this.checkTime);
    }
    public void shipRotation()
    {
        this.transform.Rotate(Vector3.down, 0.03f);
    }

    public void ClickDownEvenet()
    {
        Debug.Log("Button Click");
    }
}
