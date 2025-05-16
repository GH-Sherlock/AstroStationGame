using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HmdHitEffect : MonoBehaviour
{
    private RawImage screenHitEffect;

    private void Awake()
    {
        screenHitEffect = this.GetComponentInChildren<RawImage>();
    }

    public void HitScreenOn()
    {
        StartCoroutine(EffectStart());
    }

    private IEnumerator EffectStart()
    {
        screenHitEffect.enabled = true;

        yield return new WaitForSeconds(0.25f);

        screenHitEffect.enabled = false;
    }
}
