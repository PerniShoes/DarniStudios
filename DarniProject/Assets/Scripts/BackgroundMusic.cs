using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume;
    public bool playMusicOnAwake;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        if (playMusicOnAwake)
        {
            audioSource.Play();
        }
    }
}
