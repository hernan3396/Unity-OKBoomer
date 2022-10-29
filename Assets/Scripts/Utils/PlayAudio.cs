using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioScriptable[] _audioScript;

    public void PlaySound(int index = 0)
    {
        EventManager.OnPlay3dSound(transform.position, _audioScript[index]);
    }
}
