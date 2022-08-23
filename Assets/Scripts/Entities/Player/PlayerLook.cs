using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerLook : MonoBehaviour
{
    #region Components
    private Player _player;
    #endregion

    private Vector2 _rotations = new Vector2(0, 90);
    private Vector2 _frameVelocity;

    private float _timer;
    [SerializeField] private Transform _cameraParent;
    private float _defaultYPos;

    private void Start()
    {
        _player = GetComponent<Player>();

        EventManager.Look += LookAtMouse;
        _defaultYPos = _cameraParent.localPosition.y;
    }

    private void LateUpdate()
    {
        SwayWeapon();
    }

    private void LookAtMouse(Vector2 look)
    {
        Vector2 rawFrameVelocity = Vector2.Scale(look, Vector2.one * _player.Data.MouseSensitivity);
        _frameVelocity = Vector2.Lerp(_frameVelocity, rawFrameVelocity, 20 * Time.deltaTime);

        // up & down
        _rotations.x -= _frameVelocity.y;
        _rotations.x = Mathf.Clamp(_rotations.x, _player.Data.LookLimits.x, _player.Data.LookLimits.y);

        // Sideways
        _rotations.y += _frameVelocity.x;

        Quaternion headRotation = Quaternion.AngleAxis(_rotations.x, Vector3.right);
        Quaternion bodyRotation = Quaternion.AngleAxis(_rotations.y, Vector3.up);

        _player.SlideCamera.localRotation = headRotation;
        _player.FpCamera.localRotation = headRotation;

        _player.Body.localRotation = bodyRotation;
    }

    private void SwayWeapon()
    {
        Vector2 newRotations = _frameVelocity;
        newRotations.x = Mathf.Clamp(newRotations.x, -80, 80);
        newRotations.y = Mathf.Clamp(newRotations.y, -80, 80);

        Quaternion xRotation = Quaternion.AngleAxis(-newRotations.y * _player.Data.SwayMultiplier, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(newRotations.x * _player.Data.SwayMultiplier, Vector3.up);
        Quaternion targetRotation = xRotation * yRotation;

        _player.Arm.localRotation = Quaternion.Slerp(_player.Arm.localRotation, targetRotation, _player.Data.SwaySmoothness * Time.deltaTime);
    }

    private void OnDestroy()
    {
        EventManager.Look -= LookAtMouse;
    }
}
