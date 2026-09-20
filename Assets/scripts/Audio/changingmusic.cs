using UnityEngine;

public class changingmusic : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip1, battleclip1;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip1;
        audioSource.Play();
    }

    public void ChangeMusicToBattle()
    {
        audioSource.Stop();
        audioSource.clip = battleclip1;
        audioSource.Play();
    }

    public void ChangeBattleToMusic()
    {
        audioSource.Stop();
        audioSource.clip = clip1;
        audioSource.Play();
    }

}
