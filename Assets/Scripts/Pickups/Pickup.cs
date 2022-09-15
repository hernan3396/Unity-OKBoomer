using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        Health,
        Ammo,
        Weapon
    }

    private Player _player;
    [SerializeField] private int _ammount;
    [SerializeField] private PickupType _pickupType;
    [SerializeField] private WeaponScriptable _weapon;
    [SerializeField] private GameObject _parent;

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // si es muy deforme pasarlo como un evento
        if (other.CompareTag("Player"))
        {
            switch (_pickupType)
            {
                case PickupType.Health:
                    _player.PickUpHealth(_ammount);
                    break;
                case PickupType.Ammo:
                    _player.PickUpAmmo(_ammount);
                    break;
                case PickupType.Weapon:
                    _player.PickUpWeapon();
                    break;
            }

            _parent.SetActive(false);
        }
    }
}
