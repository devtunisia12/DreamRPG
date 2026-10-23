
using UnityEngine;

public class changingmusic : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip clip1, battleclip1, deadclip, clip2, clip3, clip4;

    public string currentMusicName;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        string savedMusic = PlayerPrefs.GetString("CurrentMusic", "clip1");

        switch (savedMusic)
        {
            case "clip2":
                PlayMusic(clip2, "clip2");
                break;

            case "clip3":
                PlayMusic(clip3, "clip3");
                break;

            case "clip4":
                PlayMusic(clip4, "clip4");
                break;

            default:
                PlayMusic(clip1, "clip1");
                break;
        }
    }

    void PlayMusic(AudioClip clip, string musicName)
    {
        if (clip == null) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

        currentMusicName = musicName;
    }

    void SaveNormalMusic(string musicName)
    {
        PlayerPrefs.SetString("CurrentMusic", musicName);
        PlayerPrefs.Save();
    }

    public void ChangeMusicToBattle()
    {
        PlayMusic(battleclip1, "battleclip1");
    }

    public void ChangeBattleToMusic()
    {
        PlayMusic(clip1, "clip1");
        SaveNormalMusic("clip1");
    }

    public void ChangeBattleToMusic2()
    {
        PlayMusic(clip2, "clip2");
        SaveNormalMusic("clip2");
    }

    public void ChangeBattleToMusic3()
    {
        PlayMusic(clip3, "clip3");
        SaveNormalMusic("clip3");
    }

    public void ChangeBattleToMusic4()
    {
        PlayMusic(clip4, "clip4");
        SaveNormalMusic("clip4");
    }

    public void ChangeMusicToDead()
    {
        PlayMusic(deadclip, "deadclip");
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
