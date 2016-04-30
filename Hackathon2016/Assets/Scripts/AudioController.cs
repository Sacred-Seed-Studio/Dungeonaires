using UnityEngine;
using System.Collections;

public enum SongType
{
    TitleScreen,
    Game,
    End
}

public enum SoundType
{
    FighterAttack,
    FighterDefend,
    MageAttack,
    MageDefend,
    RogueAttack,
    RogueDefend,
    ArcherAttack,
    ArcherDefend,
    Death
}
public class AudioController : MonoBehaviour
{
    public static AudioController controller;

    public AudioClip titleScreenSong, gameSong, endSong;
    public AudioClip fighterAttack, fighterDefend, mageAttack, mageDefend, rogueAttack, rogueDefend, archerAttack, archerDefend, death;

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
            case SongType.TitleScreen: audioSource.clip = titleScreenSong; break;
            case SongType.Game: audioSource.clip = gameSong; break;
            case SongType.End: audioSource.clip = endSong; break;
        }

        audioSource.Play();
    }

    public void PlaySound(SoundType sound)
    {
        AudioClip clip = null;
        switch (sound)
        {
            case SoundType.FighterAttack: clip = fighterAttack; break;
            case SoundType.FighterDefend: clip = fighterDefend; break;
            case SoundType.MageAttack: clip = mageAttack; break;
            case SoundType.MageDefend: clip = mageDefend; break;
            case SoundType.RogueAttack: clip = rogueAttack; break;
            case SoundType.RogueDefend: clip = rogueDefend; break;
            case SoundType.ArcherAttack: clip = archerAttack; break;
            case SoundType.ArcherDefend: clip = archerDefend; break;
            case SoundType.Death: clip = death; break;
        }

        audioSource.PlayOneShot(clip);
    }

    public void PlaySound(PlayerClass pClass, bool attack = true)
    {
        //attack = false => defend
        if (attack)
        {
            switch (pClass)
            {
                case PlayerClass.Fighter: PlaySound(SoundType.FighterAttack); break;
                case PlayerClass.Mage: PlaySound(SoundType.MageAttack); break;
                case PlayerClass.Rogue: PlaySound(SoundType.RogueAttack); break;
                case PlayerClass.Archer: PlaySound(SoundType.ArcherAttack); break;
            }
        }
        else
        {
            switch (pClass)
            {
                case PlayerClass.Fighter: PlaySound(SoundType.FighterDefend); break;
                case PlayerClass.Mage: PlaySound(SoundType.MageDefend); break;
                case PlayerClass.Rogue: PlaySound(SoundType.RogueDefend); break;
                case PlayerClass.Archer: PlaySound(SoundType.ArcherDefend); break;
            }
        }
    }
}
