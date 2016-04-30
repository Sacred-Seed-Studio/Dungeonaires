using UnityEngine;
using System.Collections;

public enum SongType
{
    TitleScreen,
    Game, 
    End
}

public class AudioController : MonoBehaviour
{
    public static AudioController controller;

    public AudioClip titleScreenSong, gameSong, endSong;

    AudioSource audioSource;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBackgroundSong(SongType song)
    {
        switch (song)
        {
            case SongType.TitleScreen:audioSource.clip = titleScreenSong; break;
            case SongType.Game: audioSource.clip = gameSong; break;
            case SongType.End: audioSource.clip = endSong; break;
        }

        audioSource.Play();
    }

}
