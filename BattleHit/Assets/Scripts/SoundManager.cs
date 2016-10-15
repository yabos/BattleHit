using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private static GameObject container;
    public static SoundManager Instance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "SoundManager";
            instance = container.AddComponent(typeof(SoundManager)) as SoundManager;
        }
        return instance;
    }

    public enum eBGMType
    {
        eBGM_Title,
        eBGM_BaseVill,
        eBGM_StoneCave,
    }

    public enum eBattleBGM
    {
        eBattleBGM_Normal,
        eBattleBGM_Boss,
        eBattleBGM_Epic,
    }

    string[] m_stBGMPath = new string[] 
    {
        "TitleBGM",
        "BaseVillBGM",
        "StoneCaveBGM",
    };

    string[] m_stBattleBGMPath = new string[]
    {
        "BattleBGMNormal",
        "BattleBGMBoss",
        "BattleBGMEpic",
    };

    public AudioClip[] audioSources;
    public GameObject audioPrefabSource;
    public Dictionary<string, AudioClip> audioClips;
    static GameObject audioPrefab;
    static AudioSource musicPlayer;
    Dictionary<string, Audio> aliveSounds;
    AudioListener al;

    public eBGMType m_eCurBGM = eBGMType.eBGM_Title;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate GameMain");
        }

        al = GetComponent<AudioListener>();
        audioClips = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in audioSources)
        {
            audioClips.Add(a.name, a);
        }

        audioPrefab = audioPrefabSource;
        musicPlayer = GetComponent<AudioSource>();
        aliveSounds = new Dictionary<string, Audio>();

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //if (!GameSetting.hasMusic)
        //{
        //    musicPlayer.Pause();
        //}
        //else
        //{
        //    if (!musicPlayer.isPlaying)
        //    {
        //        musicPlayer.Play();
        //    }
        //}

        //if (!gamesetting.hassound && aliveSounds.count > 0)
        if (aliveSounds == null) return;

        if (aliveSounds.Count > 0)
        {
            foreach (Audio a in aliveSounds.Values)
            {
                a.StopSound();
            }
            aliveSounds.Clear();
        }

        if (!al.enabled)
        {
            al.enabled = true;
        }
    }

    public void PlaySoundOnce(string name)
    {
        //if (!GameSetting.hasSound)
        //{
        //    return;
        //}

        if (!audioClips.ContainsKey(name))
        {
            return;
        }
        GameObject go = GameObject.Instantiate(audioPrefab) as GameObject;
        go.transform.parent = instance.transform;
        Audio a = go.GetComponent<Audio>();
        a.PlaySoundOnce(audioClips[name]);        
    }


    public void PlayBGM(eBGMType eBGMType)
    {
        //if (!GameSetting.hasMusic)
        //{
        //    return;
        //}

        m_eCurBGM = eBGMType;

        string stName = m_stBGMPath[(int)m_eCurBGM];
        if (musicPlayer.clip == null || musicPlayer.clip.name != stName)
        {
            musicPlayer.clip = VResources.Load<AudioClip>("Sounds/" + stName);
            musicPlayer.Stop();
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
        else
        {
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }

    public void PlayBattleBGM(eBattleBGM eType)
    {
        //if (!GameSetting.hasMusic)
        //{
        //    return;
        //}

        string stName = m_stBattleBGMPath[(int)eType];
        if (musicPlayer.clip == null || musicPlayer.clip.name != stName)
        {
            musicPlayer.clip = VResources.Load<AudioClip>("Sounds/" + stName);
            musicPlayer.Stop();
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
        else
        {
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }
    public void PlayCurrentBGM()
    {
        string stName = m_stBGMPath[(int)m_eCurBGM];
        if (musicPlayer.clip == null || musicPlayer.clip.name != stName)
        {
            musicPlayer.clip = VResources.Load<AudioClip>("Sounds/" + stName);
            musicPlayer.Stop();
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }
}