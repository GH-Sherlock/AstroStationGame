using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : Singleton<QuestController>
{
    private bool isVibration = false;

    
    public float LeftTriggerInputValue()
    {
        return OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
    }
    public float RightTriggerInputValue()
    {
        return OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
    }

    public bool RightTriggerDown()
    {
        return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
    }

    public bool LeftTriggerDown()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
    }

    public bool RightGripDown()
    {
        return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
    }

    public bool LeftGripDown()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);
    }

    /// <summary>
    /// time = 진동지속시간, frequency = 진동주파수, strength = 진동진폭, controller = RTouch 또는 LTouch
    /// </summary>
    public void Vibration(float time, float frequency, float strength, OVRInput.Controller controller)
    {
        if (isVibration)
            return;

        StartCoroutine(SetVibation(time, frequency, strength, controller));
    }

    private IEnumerator SetVibation(float waitTime, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        isVibration = true;

        yield return new WaitForSeconds(waitTime);

        OVRInput.SetControllerVibration(0, 0, controller);
        isVibration = false;
    }

    public void AllVibration(float time, float frequency, float strength)
    {
        if (isVibration)
            return;

        StartCoroutine(SetAllVibation(time, frequency, strength));
    }

    private IEnumerator SetAllVibation(float waitTime, float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);
        isVibration = true;

        yield return new WaitForSeconds(waitTime);

        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        isVibration = false;
    }

    //----------------

    public bool FourButtonDown()
    {
        return OVRInput.GetDown(OVRInput.Button.Four);
    }


}
