using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    #region Components
    private Player _player;
    #endregion

    #region Cameras
    private GameObject _fpCamera;
    private GameObject _slideCamera;
    #endregion

    private bool _isCrouching = false;

    private void Start()
    {
        _player = GetComponent<Player>();

        _fpCamera = _player.FpCamera.gameObject;
        _slideCamera = _player.SlideCamera.gameObject;

        EventManager.Crouch += SlideInput;
    }

    private void SlideInput(bool value)
    {
        _isCrouching = value;
    }

    public void Slide()
    {
        _slideCamera.SetActive(true);
        _fpCamera.SetActive(false);

        int speed = _player.Data.CrouchVel;

        _player.StandingHitbox.SetActive(false);
        _player.SlidingHitbox.SetActive(true);

        Vector3 newVel = new Vector3(_player.RB.velocity.x * speed, _player.RB.velocity.y, _player.RB.velocity.z * speed);
        _player.RB.velocity = newVel;
    }

    public void EndSlide()
    {
        _fpCamera.SetActive(true);
        _slideCamera.SetActive(false);

        _player.StandingHitbox.SetActive(true);
        _player.SlidingHitbox.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.Crouch -= SlideInput;
    }

    public bool Crouching
    {
        get { return _isCrouching; }
    }
}
