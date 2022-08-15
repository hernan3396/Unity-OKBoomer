using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : MonoBehaviour
{
    private bool _isShooting = false;

    void Start()
    {
        EventManager.Shoot += ShootInput;
    }

    private void ShootInput(bool value)
    {
        _isShooting = value;
    }

    private void OnDestroy()
    {
        EventManager.Shoot -= ShootInput;
    }

    public bool IsShooting
    {
        get { return _isShooting; }
    }
}
