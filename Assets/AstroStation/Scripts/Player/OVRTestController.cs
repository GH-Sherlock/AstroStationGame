using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRTestController : MonoBehaviour
{
    float[] angles = new float[4];

    public GameObject ship;
    public Transform cam;

    public float preRotY = 0f;

    private float rotAngleY = 0f;

    private void Awake()
    {
        preRotY = cam.localEulerAngles.y;
    }

    void Update()
    {
        //InputRotation();

        float curRotY = cam.eulerAngles.y;  
        Vector3 moveDir = Vector3.zero;

        angles[0] = 360 - preRotY + curRotY;
        angles[1] = 360 - curRotY + preRotY;
        angles[2] = preRotY - curRotY;
        angles[3] = curRotY - preRotY;

        float minAngle = angles[0];
        int minIndex = 0;
        for(int i = 0; i < 4; i++)
        {
            if(minAngle > angles[i] && angles[i] >= 0)
            {
                minAngle = angles[i];
                minIndex = i;
            }
        }

        if(minIndex == 0 || minIndex == 3)
        {
            moveDir = Vector3.right;
        }
        else
        {
            moveDir = Vector3.left;
        }

        float result = minAngle;


        result = result / 360f * 10f;

        ship.transform.Translate(moveDir * result);

        preRotY = curRotY;
    }

    private void InputRotation()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotAngleY = 1f;
            cam.Rotate(new Vector3(0, rotAngleY, 0f));
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotAngleY = -1f;
            cam.Rotate(new Vector3(0, rotAngleY, 0f));
        }
    }
}