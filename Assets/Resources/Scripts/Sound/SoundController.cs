using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    private Dictionary<string, AudioClip> dicBGMaudioClip;
    public AudioSource bgmPlayer;
    public float musicStartTime;
    public bool isPlayBgm;
    public string bgmName;
    public float checkTimer;


    //사운드이펙트
    public int audioSourceCount = 20;
    private AudioSource[] SFXsource;

    // 사운드 파일 이름과 DataManager에 들어가는 사운드정보의 사운드 이름이 같아야 한다.
    // DataManager가 init 된 이후에 SoundController가 init되야 한다.
    public void SoundControllerInit()
    {
        Debug.Log("SoundController");
        if (this.bgmPlayer == null)
        {
            this.bgmPlayer = this.GetComponent<AudioSource>();

            if(this.bgmPlayer == null)
            {
                this.bgmPlayer = this.gameObject.AddComponent<AudioSource>();
            }

            this.bgmPlayer.volume = 0.1f;
        }
        if (this.dicBGMaudioClip == null)
        {
            this.dicBGMaudioClip = new Dictionary<string, AudioClip>();
        }
        AudioClip audioClip = null;
        Object[] arrBGM = Resources.LoadAll("Sound/BGM");
        for (int i = 0; i < arrBGM.Length; i++)
        {
            audioClip = arrBGM[i] as AudioClip;
            this.dicBGMaudioClip.Add(audioClip.name, audioClip);
        }
        this.isPlayBgm = false;
        audioClip = null;

        // sound effect 나 SFX 관련 사운드를 컨트롤해야할 수도 있음
        //sfx 소스 초기화
        SFXsource = new AudioSource[audioSourceCount];

        for (int i = 0; i < SFXsource.Length; i++)
        {
            SFXsource[i] = this.gameObject.AddComponent<AudioSource>();
            SFXsource[i].playOnAwake = false;
            SFXsource[i].loop = false;
            SFXsource[i].volume = 1f;
        }

    }
    private void Update()
    {
        //if (this.isPlayBgm)
        //{
        //    this.checkTimer += Time.deltaTime;
        //    if (this.checkTimer > this.musicStartTime)
        //    {
        //        this.PlaySong();
        //    }
        //}
    }
    public void PlaySong()
    {
        if (!this.bgmPlayer.isPlaying)
        {
            foreach (var pair in this.dicBGMaudioClip)
            {
                var key = pair.Key;
                var data = pair.Value;
                if (this.bgmName == key)
                {
                    this.bgmPlayer.clip = data;
                    this.bgmPlayer.Play();
                    return;
                }
            }
        }
        this.isPlayBgm = false;
    }
    public void PlayBGM(string bgmName)
    {
        this.bgmName = bgmName;
        this.isPlayBgm = true;

        PlaySong();
    }
    public void StopBGM()
    {
        if (this.bgmPlayer.isPlaying)
            this.bgmPlayer.Stop();
    }
    public void PauseBGM()
    {
        if (this.bgmPlayer.isPlaying)
            this.bgmPlayer.Pause();
    }
    public void UnPauseBGM()
    {
        if (!this.bgmPlayer.isPlaying)
            this.bgmPlayer.UnPause();
    }
    public void ChangeBGMVolume(float volume)
    {
        this.bgmPlayer.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }

    public float BGMPlayTime()
    {
        if (bgmPlayer.clip == null)
        {
            Debug.Log("배경음악 없음");
            return 0;
        }
        return bgmPlayer.clip.length;
    }


    //SFX 
    public void PlaySFX(AudioClip clip, bool loop = false, float volume = 1f)
    {
        AudioSource sfx = GetEmptySource();

        sfx.clip = clip;
        sfx.loop = loop;
        sfx.volume = volume;
        sfx.Play();
    }

    public void StopSFXClipName(string name)
    {
        for (int i = 0; i < SFXsource.Length; i++)
        {
            if (SFXsource[i].clip)
            {
                if (SFXsource[i].clip.name == name)
                {
                    SFXsource[i].Stop();
                }
            }
        }
    }


    //비어있는 오디오 소스 반환
    public AudioSource GetEmptySource()
    {
        int largestIndex = 0;
        float largestProgress = 0;

        for (int i = 0; i < SFXsource.Length; i++)
        {
            if (!SFXsource[i].isPlaying)
            {
                return SFXsource[i];
            }

            //만약 비어있는 오디오 소스를 못찿으면 가장 진행도가 높은 오디오 소스 반환(루프중인건 스킵)
            float progress = SFXsource[i].time / SFXsource[i].clip.length;
            if (progress > largestProgress && !SFXsource[i].loop)
            {
                largestIndex = i;
                largestProgress = progress;
            }
        }

        return SFXsource[largestIndex];
    }
}
