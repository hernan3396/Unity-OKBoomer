using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Player))]
public class PlayerLook : MonoBehaviour
{
    #region Components
    private Player _player;
    #endregion

    private Vector2 _frameVelocity;
    private float _defaultYPos;
    private Vector3 _rotations;
    private float _timer;

    private Vector2 _dirInput;

    private void Start()
    {
        _player = GetComponent<Player>();

        EventManager.Look += LookAtMouse;
        EventManager.Move += ChangeDirection;
        _defaultYPos = _player.Arm.localPosition.y;
        _rotations = new Vector2(0, _player.Transform.eulerAngles.y);
    }

    private void LateUpdate()
    {
        SwayWeapon();
    }

    private void LookAtMouse(Vector2 look)
    {
        _frameVelocity = Vector2.Scale(look, Vector2.one * _player.Data.MouseSensitivity);

        // up & down
        _rotations.x -= _frameVelocity.y;
        _rotations.x = Mathf.Clamp(_rotations.x, _player.Data.LookLimits.x, _player.Data.LookLimits.y);

        // Sideways
        _rotations.y += _frameVelocity.x;

        UpdateRotation();
    }

    public void AddRecoil(Vector3 force)
    {
        _rotations.x += force.x;
        _rotations.y += force.y;

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Quaternion headRotation = Quaternion.AngleAxis(_rotations.x, Vector3.right);
        Quaternion bodyRotation = Quaternion.AngleAxis(_rotations.y, Vector3.up);
        Quaternion tiltRotation = Quaternion.AngleAxis(_rotations.z, Vector3.forward);

        _player.SlideCamera.localRotation = headRotation;
        _player.FpCamera.localRotation = headRotation * tiltRotation;

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

    public void RotateWeapon()
    {
        _timer += Time.deltaTime * _player.Data.WeaponFrequency; // frecuencia
        Vector3 pos = _player.Arm.transform.localPosition;
        Vector3 finalPos = new Vector3(pos.x, _defaultYPos + Mathf.Sin(_timer) * _player.Data.WeaponAmplitude, pos.z); // amplitud
        _player.Arm.transform.localPosition = Vector3.Lerp(pos, finalPos, Time.deltaTime * 10);
    }

    private void ChangeDirection(Vector2 move)
    {
        _dirInput = move;
    }

    public void TiltCamera()
    {
        int tiltDir = Mathf.RoundToInt(-_dirInput.x);
        int tiltAngle = 5;

        _rotations.z = Mathf.Lerp(_rotations.z, tiltAngle * tiltDir, Time.deltaTime * 5);

        UpdateRotation();
    }

    private void OnDestroy()
    {
        EventManager.Look -= LookAtMouse;
        EventManager.Move -= ChangeDirection;
    }
}
