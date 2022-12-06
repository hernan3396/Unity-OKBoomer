using UnityEngine;
using UnityEngine.InputSystem;

public class MenuClick : MonoBehaviour
{
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        _source.PlayOneShot(_source.clip);
    }
}
