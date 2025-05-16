using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public GameObject cube;
    public GameObject cube2;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TimeManager.instance.Pause();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TimeManager.instance.Resume();
        }

        cube.transform.Rotate(Vector3.up, 50f * TimeManager.instance.GetTimeScale());

        cube2.transform.Rotate(Vector3.up, 50f * Time.deltaTime);

        Debug.Log("MainGame Update");
    }

    private void FixedUpdate()
    {
        Debug.Log("MainGame FixedUpdate");
    }
}
