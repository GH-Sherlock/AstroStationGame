using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HmdAngleTracker : MonoBehaviour
{
    float[] angles = new float[4];

    public GameObject ship;
    public Transform cam;

    private float preRotY = 0f;
    private float moveAngleY = 0f;
    private float fMoveDir = 0f;

    //Get,Set
    private Vector3 moveX = Vector3.zero;
    private float fMoveX = 0f;

    private void Awake()
    {
        preRotY = cam.localEulerAngles.y;
    }


    // Update is called once per frame
    void Update()
    {
        float curRotY = cam.eulerAngles.y;
        Vector3 v3MoveDir = Vector3.zero;
        fMoveX = 0f;

        angles[0] = 360 - preRotY + curRotY;
        angles[1] = 360 - curRotY + preRotY;
        angles[2] = preRotY - curRotY;
        angles[3] = curRotY - preRotY;

        float minAngle = angles[0];
        int minIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            //angles[i] = Mathf.Round(angles[i]);
            if (minAngle > angles[i] && angles[i] >= 0)
            {
                minAngle = angles[i];
                minIndex = i;
            }
        }

        if (minIndex == 0 || minIndex == 3)
        {
            v3MoveDir = Vector3.right;
            fMoveDir = 1f;
        }
        else
        {
            v3MoveDir = Vector3.left;
            fMoveDir = -1f;
        }

        moveAngleY = minAngle;


        moveAngleY = moveAngleY / 360f;

        moveX = v3MoveDir * moveAngleY;
        fMoveX = minAngle * fMoveDir;

        preRotY = curRotY;
    }

    public Vector3 MoveX
    {
        get { return moveX; }
        set { moveX = value; }
    }

    public float FloatMoveX
    {
        get { return fMoveX; }
        set { fMoveX = value; }
    }
}
