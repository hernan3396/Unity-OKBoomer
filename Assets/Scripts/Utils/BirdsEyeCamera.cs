using UnityEngine;

public class BirdsEyeCamera : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    [SerializeField] private int _speed;
    private Vector3 _dirInput;
    private Rigidbody _rb;
    private bool _jumping;
    private bool _crouching;
    #endregion

    #region Camera
    [Header("Camera")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float _sensitivity;
    private Vector2 _frameVelocity;
    private Vector2 _rotations;
    #endregion

    #region Components
    private Transform _transform;
    private GameObject _player;
    #endregion

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _rotations = new Vector2(0, _transform.eulerAngles.y);

        EventManager.Move += ChangeDirection;
        EventManager.Look += LookAtMouse;
        EventManager.Jump += Jump;
        EventManager.Crouch += Crouch;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        Jumping();
        Crouching();
    }

    private void ChangeDirection(Vector2 move)
    {
        _dirInput = move;
    }

    private void LookAtMouse(Vector2 look)
    {
        _frameVelocity = Vector2.Scale(look, Vector2.one * _sensitivity);

        _rotations.x -= _frameVelocity.y;
        _rotations.x = Mathf.Clamp(_rotations.x, -90, 90);

        _rotations.y += _frameVelocity.x;

        Quaternion headRotation = Quaternion.AngleAxis(_rotations.x, Vector3.right);
        Quaternion bodyRotation = Quaternion.AngleAxis(_rotations.y, Vector3.up);

        _camera.localRotation = headRotation;
        _transform.localRotation = bodyRotation;
    }

    private void ApplyMovement()
    {
        Vector3 dir = (_transform.right * _dirInput.x + _transform.forward * _dirInput.y).normalized;
        Vector3 rbVelocity = (dir + _camera.forward * _dirInput.y) * _speed;

        _rb.velocity = rbVelocity;
    }

    private void Jump(bool value)
    {
        _jumping = value;
    }

    private void Jumping()
    {
        if (_jumping)
            _rb.velocity += new Vector3(0, _speed, 0);
    }

    private void Crouch(bool value)
    {
        _crouching = value;
    }

    private void Crouching()
    {
        if (_crouching)
            _rb.velocity -= new Vector3(0, _speed, 0);
    }

    private void OnDestroy()
    {
        EventManager.Move -= ChangeDirection;
        EventManager.Look -= LookAtMouse;
        EventManager.Jump -= Jump;
        EventManager.Crouch -= Crouch;
    }
}
