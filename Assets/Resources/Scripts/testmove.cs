using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmove : MonoBehaviour
{
    public Transform[] arrPos;
    public TrailRenderer trailRenderer;
    public Transform player;
    public float time;
    public bool isMove;
    // Start is called before the first frame update
    
    public void MoveBezierCurve()
    {
        this.time += Time.deltaTime;
        this.player.position = this.CalculateBezierPos(arrPos[0].position, arrPos[1].position, arrPos[2].position, arrPos[3].position, arrPos[4].position, this.time * 0.2f);
    }
    public Vector3 CalculateBezierPos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        Vector3 v0 = Vector3.Lerp(p0, p1, t);
        Vector3 v1 = Vector3.Lerp(p1, p2, t);
        Vector3 v2 = Vector3.Lerp(p2, p3, t);
        Vector3 v3 = Vector3.Lerp(p3, p4, t);

        Vector3 v4 = Vector3.Lerp(v0, v1, t);
        Vector3 v5 = Vector3.Lerp(v1, v2, t);
        Vector3 v6 = Vector3.Lerp(v2, v3, t);

        Vector3 v7 = Vector3.Lerp(v4, v5, t);
        Vector3 v8 = Vector3.Lerp(v5, v6, t);

        return Vector3.Lerp(v7, v8, t);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if (!this.isMove)
                this.isMove = true;
            else
                this.isMove = false;
        }
        if(this.isMove)
        {
            this.MoveBezierCurve();
        }
    }
}
