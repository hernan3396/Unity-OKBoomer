using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAudio", menuName = "OKBoomer/Create New Audio", order = 0)]
public class AudioScriptable : ScriptableObject
{
    public List<AudioClip> clips;
    public bool IsMusic = false;

    public Vector2 volume = new Vector2(1, 1);
    public Vector2 pitch = new Vector2(1, 1);

    public AudioClip GetAudioClip(int index)
    {
        return clips[index];
    }

    public AudioClip GetRandom()
    {
        return clips[Random.Range(0, clips.Count)];
    }
}