using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerLook : MonoBehaviour
{
    #region Components
    private Player _player;
    #endregion

    private Vector2 _rotations = new Vector2(0, 90);
    private Vector2 _look;

    private void Start()
    {
        _player = GetComponent<Player>();

        EventManager.Look += LookAtMouse;
    }

    private void Update()
    {
        // SwayWeapon();
    }

    private void LookAtMouse(Vector2 look)
    {
        // lo divido entre 10 para que quede un numero mas lindo
        // en el inspector (2 en vez de 0.2 por ejemplo)
        _look = look * 0.1f;

        // up & down
        _rotations.x -= _look.y;
        _rotations.x = Mathf.Clamp(_rotations.x, _player.Data.LookLimits.x, _player.Data.LookLimits.y);

        // Sideways
        _rotations.y += _look.x;

        Quaternion headRotation = Quaternion.AngleAxis(_rotations.x, Vector3.right);
        Quaternion bodyRotation = Quaternion.AngleAxis(_rotations.y, Vector3.up);

        // estas es solo para ver como el brazo se mueve en el editor,
        // realmente es al pedo lo puse para probar nomas
        // sacar luego (?)
        _player.OverlayCamera.localRotation = headRotation;
        _player.Arm.localRotation = headRotation;

        _player.SlideCamera.localRotation = headRotation;
        _player.FpCamera.localRotation = headRotation;

        _player.Body.localRotation = bodyRotation;

        SwayWeapon();
    }

    private void SwayWeapon()
    {
        // sway
        // no queda super lindo aca pero de la forma que esta hecho el proyecto es lo mejor que se me ocurrio
        Quaternion xRotation = Quaternion.AngleAxis(-_look.y * _player.Data.SwayMultiplier, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(_look.x * _player.Data.SwayMultiplier, Vector3.up);
        Quaternion targetRotation = xRotation * yRotation;

        _player.Arm.localRotation = Quaternion.Slerp(_player.Arm.localRotation, targetRotation, _player.Data.SwaySmoothness * Time.deltaTime);
    }

    private void OnDestroy()
    {
        EventManager.Look -= LookAtMouse;
    }
}
