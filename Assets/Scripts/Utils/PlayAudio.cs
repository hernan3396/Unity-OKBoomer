using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioScriptable[] _audioScript;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (TryGetComponent(out AudioSource _audioSrc))
            _audioSource = _audioSrc;
    }

    public void PlaySound(int index = 0)
    {
        EventManager.OnPlay3dSound(transform.position, _audioScript[index]);
    }

    public void PlayOwnSound(int index = 0)
    {
        if (_audioSource == null) return;
        EventManager.OnPlayOwn3dSound(_audioSource, _audioScript[index]);
    }
}
