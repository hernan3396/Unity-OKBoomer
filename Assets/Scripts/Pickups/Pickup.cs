using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private int _weaponIndex; // es el arma "maxima" que da, asi si spameas el mismo pickup de arma recargando el nivel no agarras las siguientes hasta la maxima

    private Material _mat;

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Player>();

        _mat = GetComponentInChildren<MeshRenderer>().material;

        _mat.SetFloat("_DissolveValue", 1);
        _mat.DOFloat(0, "_DissolveValue", 1);
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
                    _player.PickUpWeapon(_weaponIndex);
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
