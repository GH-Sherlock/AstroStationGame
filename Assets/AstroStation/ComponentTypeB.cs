using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTypeB : MonoBehaviour
{
    private void Update()
    {
        Ray ray = new Ray() ;
        RaycastHit hit;
        ray.origin = this.transform.position;
        ray.direction = Vector3.forward;
        if (Physics.Raycast(ray, out hit))
        {
            hit.transform.GetComponent<ComponentTypeA>().f -= 1f;
        }
    }
}
