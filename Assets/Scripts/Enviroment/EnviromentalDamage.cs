using UnityEngine;

public class EnviromentalDamage : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int _damage = 20;
    private Player _player;
    private bool _playerInside = false;

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Player>();
    }

    private void Update()
    {
        if (!_playerInside) return;

        _player.TakeDamage(_damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerInside = false;
    }
}
