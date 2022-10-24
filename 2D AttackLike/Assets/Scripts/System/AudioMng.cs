using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMng : TSingleton<AudioMng>
{
    private AudioSource background;
    private AudioSource actionSource;
    private AudioSource uiSource;

    private Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();
    private List<AudioSource> audioSources = new List<AudioSource>();

    public void Init()
    {
        background = gameObject.AddComponent<AudioSource>();
        actionSource = gameObject.AddComponent<AudioSource>();
        uiSource = gameObject.AddComponent<AudioSource>();

        SoundProperty(background, true, false);
        SoundProperty(actionSource, false, false);
        SoundProperty(uiSource, false, false);

        LoadBackground();
    }

    public void SoundProperty(AudioSource audio, bool loop = false, bool playOnAwake = false, float volume = 1.0f, AudioClip clip = null)
    {
        audio.volume = volume;
        audio.loop = loop;
        audio.playOnAwake = playOnAwake;
        audio.clip = clip;
    }

    void LoadBackground()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/BGM");
        foreach(AudioClip clip in clips)
        {
            if(audioClipDic.ContainsKey(clip.name)==false)
                audioClipDic.Add(clip.name, clip);
        }
    }

    public void PlayBGM(string name, float volume = 0.1f)
    {
        if(audioClipDic.ContainsKey(name))
        {
            AudioClip clip = audioClipDic[name];
            background.clip = clip;
            background.volume = volume;
            background.Play();
        }
    }

    public void StopBGM(string name, float volume = 0f)
    {
        if(audioClipDic.ContainsKey(name))
        {
            background.clip = null;
            background.volume = volume;
            background.Stop();
        }
    }

    public void CheckPlayBGM()
    {
        if(background.isPlaying) background.Stop();
        switch(SceneManager.GetActiveScene().name)
        {
            case "TitleScene":
                PlayBGM("The Tavern");
                break;
                
            case "TownScene":
                PlayBGM("Temple Guardians (Long Loop)");
                break;

            case "GameScene":
                PlayBGM("battle_05_loop");
                break;

        }
    }

    public void StopAllBGM()
    {

    }

    public AudioClip LoadClip(string path, string name)
    {
        AudioClip clip = null;

        if(audioClipDic.ContainsKey(name))
        {
            clip = audioClipDic[name];
        }
        else
        {
            clip = Resources.Load<AudioClip>(path+name);
            if(clip != null)
                audioClipDic.Add(name, clip);
        }
        return clip;
    }

    private AudioSource Pooling(string audioName = "")
    {
        if (audioName.Equals(string.Empty))
            audioName = "AudioSource";

        AudioSource source = null;
        for(int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].gameObject.activeSelf == false)
            {
                source = audioSources[i];
                source.gameObject.SetActive(true);
            }
        }

        if(source == null)
        {
            GameObject newObject = new GameObject(audioName, typeof(AudioSource));
            newObject.name = audioName;
            newObject.transform.SetParent(transform);
            source = newObject.GetComponent<AudioSource>();
            audioSources.Add(source);
        }
        return source;
    }

    public void PlayUIEffect(string name, float volume = 0.5f)
    {
        //if (uiSource.isPlaying) return;
        AudioClip clip = LoadClip("Sounds/UI/", name);
        uiSource.clip = clip;
        uiSource.volume = volume;
        uiSource.Play();
    }

    public void PlayActionEffect(string name, float volume = 0.5f)
    {
        AudioClip clip = LoadClip("Sounds/ACT/", name);
        AudioSource source = Pooling();
        SoundProperty(source, volume:volume, clip:clip);
        source.Play();
        Deactive(source);
    }

    void Deactive (AudioSource audio)
    {
        StartCoroutine(DeactiveAudio(audio));
    }

    IEnumerator DeactiveAudio(AudioSource audio)
    {
        if(audio != null && audio.clip != null)
        {
            yield return new WaitForSeconds(audio.clip.length);
            audio.gameObject.SetActive(false);
        }
        yield return null;
    }
}
