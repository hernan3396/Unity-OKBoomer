using UnityEngine;

public class OSTPlayer : MonoBehaviour
{
    [SerializeField] private AudioManager.OST _initialOST;
    private void Start()
    {
        EventManager.OnPlayMusic(_initialOST);
    }
}
