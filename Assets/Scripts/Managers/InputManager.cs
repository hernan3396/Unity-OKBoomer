using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    public bool CanMove = false;
    public Vector2 Move;
    #endregion

    #region Look
    [Header("Look")]
    public bool CanLook = false;
    public Vector2 Look;
    #endregion

    #region Jumping
    [Header("Jumping")]
    public bool Jump = false;
    #endregion

    #region Shooting
    [Header("Shooting")]
    public bool Shoot = false;
    public bool SpecialShoot = false;
    #endregion

    #region Pause
    [Header("Pause")]
    public bool Pause = false;
    #endregion

    #region Crouch
    public bool Crouch;
    #endregion

    private void Awake()
    {
        EventManager.GameStart += OnGameStart;
        EventManager.ResumeMenu += OnPause;
    }

    private void OnGameStart()
    {
        CanMove = true;
        CanLook = true;
    }

    private void OnGameOver()
    {
        CanMove = false;
        CanLook = false;
    }

    #region MovementMethods
    public void OnMove(InputValue value)
    {
        if (!CanMove) return;
        // en el caso que necesitemos procesar esta
        // data ajustar esto, de momento creo que
        // funciona asi como esta
        Move = value.Get<Vector2>();
        EventManager.OnMove(Move);
    }
    #endregion

    #region LookMethods
    public void OnLook(InputValue value)
    {
        if (!CanLook) return;

        // en el caso que necesitemos procesar esta
        // data ajustar esto, de momento creo que
        // funciona asi como esta
        Look = value.Get<Vector2>();
        EventManager.OnLook(Look);
    }
    #endregion

    #region JumpingMethods
    public void OnJump(InputValue value)
    {
        if (!CanMove) return;

        Jump = value.isPressed;
        EventManager.OnJump(Jump);
    }
    #endregion

    #region ShootingMethods
    public void OnShoot(InputValue value)
    {
        if (!CanLook) return;

        Shoot = value.isPressed; // de momento no se usa para nada
        EventManager.OnShoot(Shoot);
    }

    public void OnSpecialShoot(InputValue value)
    {
        if (!CanLook) return;

        SpecialShoot = value.isPressed;
        EventManager.OnSpecialShoot(SpecialShoot);
    }
    #endregion

    #region ChangeWeaponMethods
    public void OnChangeWeapon(InputValue value)
    {
        if (!CanLook) return;

        float newValue = value.Get<float>();
        if (newValue > 0)
            EventManager.OnChangeWeapon(1);
        else if (newValue < 0)
            EventManager.OnChangeWeapon(-1);
    }
    #endregion

    #region PauseMethods
    public void OnPause()
    {
        Pause = !Pause;
        CanMove = !Pause;
        CanLook = !Pause;

        EventManager.OnPause(Pause);
    }
    #endregion

    #region CrouchMethods
    public void OnCrouch(InputValue value)
    {
        if (!CanMove) return;

        Crouch = value.isPressed;
        EventManager.OnCrouch(Crouch);
    }
    #endregion

    #region MeleeMethods
    public void OnMelee()
    {
        if (!CanMove) return;
        EventManager.OnMelee();
    }
    #endregion

    private void OnDestroy()
    {
        EventManager.GameStart -= OnGameStart;
        EventManager.ResumeMenu -= OnPause;
    }
}