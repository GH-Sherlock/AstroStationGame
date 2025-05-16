using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanPrint : MonoBehaviour
{
    public Transform Center;
    float preAngle = 0f;
    float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");

    }

    private void FixedUpdate()
    {
        Debug.Log("fixedUpdate");
    }
}
