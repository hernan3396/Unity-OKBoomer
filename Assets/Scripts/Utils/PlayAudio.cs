using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private bool _playOnHit = true;
    [SerializeField] private AudioScriptable _audioScript;
    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private string _tag = "Bullet";

    public void PlaySound()
    {
        EventManager.OnPlay3dSound(transform.position, _audioScript, _animCurve);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(_tag) && _playOnHit)
            PlaySound();
    }
}
