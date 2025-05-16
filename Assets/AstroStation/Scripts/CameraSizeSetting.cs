using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeSetting : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = this.GetComponent<Camera>();

        cam.orthographicSize = (Screen.height / 31f) / 2f;
    }
}
