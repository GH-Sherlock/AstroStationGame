using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SubWeaponRequest : MonoBehaviour
{
    private string postValue = "000";
    private bool isUsingRepeatRequest = false;
    private float repeatTime = 0.3f;

    public void SubWeaponNotWait()
    {
        if (isUsingRepeatRequest)
            return;

        isUsingRepeatRequest = true;
        repeatTime = 0f;
        postValue = "121";
        StartCoroutine(RepeatRequest());
    }

    public void RailGun()
    {
        if (isUsingRepeatRequest)
            return;

        isUsingRepeatRequest = true;
        postValue = "165";
        repeatTime = 0.3f;
        StartCoroutine(RepeatRequest());
    }

    public void HomingMissile()
    {
        if (isUsingRepeatRequest)
            return;

        isUsingRepeatRequest = true;
        postValue = "155";
        repeatTime = 0f;
        StartCoroutine(RepeatRequest());
    }

    public void DragonBreath()
    {
        if (isUsingRepeatRequest)
            return;

        isUsingRepeatRequest = true;
        postValue = "170";
        repeatTime = 0.3f;
        StartCoroutine(RepeatRequest());
    }

    private IEnumerator RepeatRequest()
    {
        float elapsedTime = 0f;

        UnityWebRequest request = new UnityWebRequest();
        

        while (elapsedTime <= repeatTime)
        {
            request = UnityWebRequest.Get("http://192.168.0.77/" + postValue);
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("성공");
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        isUsingRepeatRequest = false;
    }
}
