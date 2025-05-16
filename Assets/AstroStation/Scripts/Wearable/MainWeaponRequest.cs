using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainWeaponRequest : MonoBehaviour
{
    private string postValue = "000";
    private bool isUsingOnceRequest = false;

    private int vulcanCycle = 0;

    public void MainWeaponWait()
    {
        if (isUsingOnceRequest)
            return;

        isUsingOnceRequest = true;
        postValue = "001";
        StartCoroutine(OnceRequest());
    }

    public void Vulcan()
    {
        if (isUsingOnceRequest)
            return;

        isUsingOnceRequest = true;
        switch (vulcanCycle)
        {
            case 0:
                postValue = "020";
                break;
            case 1:
                postValue = "020";
                break;
            case 2:
                postValue = "001";
                break;
        }
        vulcanCycle++;
        if (vulcanCycle >= 3)
        {
            vulcanCycle = 0;
        }

        StartCoroutine(OnceRequest());
    }

    public void LaserBeam()
    {
        if (isUsingOnceRequest)
            return;

        isUsingOnceRequest = true;
        postValue = "030";
        StartCoroutine(OnceRequest());
    }


    private IEnumerator OnceRequest()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get("http://192.168.0.77/" + postValue))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("성공");
            }

            yield return new WaitForSeconds(0.1f);
        }

        isUsingOnceRequest = false;
    }
}
