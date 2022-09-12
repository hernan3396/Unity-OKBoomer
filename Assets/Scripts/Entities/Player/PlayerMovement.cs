using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour, IPauseable
{
    #region Components
    private Player _player;
    #endregion

    private Vector3 _dirInput;
    private float _movementMod = 1;

    private void Awake()
    {
        _player = GetComponent<Player>();

        EventManager.Move += ChangeDirection;
    }

    private void ChangeDirection(Vector2 move)
    {
        _dirInput = move;
    }

    public void ApplyMovement()
    {
        if (_player.Paused) return;
        Vector3 dir = (_player.Transform.right * _dirInput.x + _player.Transform.forward * _dirInput.y).normalized;
        Vector3 rbVelocity = dir * _player.Data.Speed * _movementMod;

        rbVelocity.y = _player.RB.velocity.y; // mantenemos la velocidad en Y que tenia el cuerpo
        _player.RB.velocity = rbVelocity;
    }

    public void OnPause(bool value)
    {
        if (value)
            _dirInput = Vector3.zero;
    }

    public bool IsMoving
    {
        get { return _dirInput.magnitude > 0.01f; }
    }

    public float MovementMod
    {
        get { return _movementMod; }
        set { _movementMod = value; }
    }
}
