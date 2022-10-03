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

    [SerializeField] private bool _respawn = false;

    private Material _mat;

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Player>();
        EventManager.GameStart += Respawn;
    }

    private void OnEnable()
    {
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

    private void Respawn()
    {
        if (_respawn)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);

        /*
        esta hecho para que si queres un pickup en el mundo
        independiente de un breakable que respawnee al lanzar OnGameStart,
        en el caso de que spawnee desde un breakable lo que queres es que despawnee
        al lanzar OnGameStart, porque sino podrias tener un monton juntos y no es la idea
        ademas de que como esta con un PoolManager podrias superar el limite de objtos y no spawnear mas
        */
    }

    private void OnDestroy()
    {
        EventManager.GameStart -= Respawn;
    }
}
