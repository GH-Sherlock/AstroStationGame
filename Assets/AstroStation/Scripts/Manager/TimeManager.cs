using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public float m_mainTimeScale = 1f;
    public float m_tNote;//박자표의 분음
    public float m_tBeat;//박자표의 박자
    public float m_bpm;//bpm;

    private float m_dotTime;

    //보조 무기 타이밍 체크
    private float m_subWeaponTiming;
    private float m_elapsedTime;
    private float m_secondElapsedTime;
    public bool enableSubWeaponFire;

    private float m_frontMissTime;
    private float m_backMisstTime;
    private float m_perfectTime;
    /*
     * 4/4박자 ≒ y/x
     * 1Bar time = (60 * y) / (x / 4 * bpm)
     * 1마디 시간 = (60초 * 박자) / ((마디분음 / 4분음) * 1분비트수)
     */


    //쓸지 안쓸지 모르는 세컨드 타임스케일( 구현 안됨)
    private float m_subTimeScale = 1f;

    private void Awake()
    {
        m_tNote = 8f;
        m_tBeat = 1f;
        m_bpm = 60f;
    }

    private void Update()
    {
        m_elapsedTime += Time.deltaTime * m_mainTimeScale;

        //if(m_elapsedTime >= m_subWeaponTiming)
        //{
        //    m_elapsedTime = 0f;
        //    enableSubWeaponFire = true;
        //}

        if(m_elapsedTime >= m_subWeaponTiming)
        {
            m_secondElapsedTime += Time.deltaTime * m_mainTimeScale;

            if(m_elapsedTime > m_subWeaponTiming + m_backMisstTime)
            {
                m_elapsedTime = m_secondElapsedTime;
                m_secondElapsedTime = 0f;
                enableSubWeaponFire = true;
            }
        }
    }

    public void SetTimeScale(float scale)
    {
        m_mainTimeScale = scale;
    }

    /// <summary>
    /// Return (float)Time.deltaTime * m_mainTimeScale
    /// </summary>
    /// <returns></returns>
    public float GetTimeScale()
    {
        return Time.deltaTime * m_mainTimeScale;
    }
    /// <summary>
    /// Return Bpm base 1/8 Bar time?
    /// </summary>
    /// <returns></returns>

    public float GetDamageOverTime()
    {
        //return (60f * m_tBeat) / (m_tNote / 4f * m_bpm);
        return m_dotTime;
    }
    /// <summary>
    /// m_mainTimeScale set 0
    /// </summary>
    public void Pause()
    {
        m_mainTimeScale = 0f;
    }

    /// <summary>
    /// m_mainTimeScale set 1
    /// </summary>
    public void Resume()
    {
        m_mainTimeScale = 1;
    }


    //------------------아래부터 최승룡 추가------------------

    private float measureTime = 0f;

    // ex) this.TimeManagerInit(DataManager.GetInstance().GetDicSoundData()[1000].sound_bgm_name);

    public void TimeManagerInit(string bmgName)
    {
        var tempData = DataManager.GetInstance().GetDicSoundData();
        foreach (var pair in tempData)
        {
            var key = pair.Key;
            var data = pair.Value;
            if (data.sound_key == bmgName)
            {
                this.m_tNote = data.sound_bgm_note;
                this.m_tBeat = data.sound_bgm_beat;
                this.m_bpm = data.sound_bgm_bpm;
                this.measureTime = (60 * this.m_tNote * this.m_tBeat) / (this.m_bpm * 4);
                //DOT 타임계산
                m_dotTime = measureTime / 8f;
                //보조무기 발사 타임계산
                m_subWeaponTiming = measureTime / 2f;
                m_frontMissTime = m_subWeaponTiming * 0.5f;
                m_backMisstTime = m_subWeaponTiming * 0.4f;
                m_perfectTime = m_subWeaponTiming * 0.2f;
                m_elapsedTime = 0f;
                m_secondElapsedTime = 0f;
                break;
            }
        }
    }
    public float GetMeasureTime()
    {
        return this.measureTime;
    }

    public float SubWeaponTiming
    {
        get { return m_subWeaponTiming; }
        set { m_subWeaponTiming = value; }
    }

    public float FrontMissTime
    {
        get { return m_frontMissTime; }
        set { m_frontMissTime = value; }
    }

    public float BackMissTime
    {
        get { return m_backMisstTime; }
        set { m_backMisstTime = value; }
    }

    public float PerfectTime
    {
        get { return m_perfectTime; }
        set { m_perfectTime = value; }
    }

    public float ElapsedTime
    {
        get { return m_elapsedTime; }
        set { m_elapsedTime = value; }
    }

    public float SecondElapsedTime
    {
        get { return m_secondElapsedTime; }
        set { m_secondElapsedTime = value; }
    }
}
