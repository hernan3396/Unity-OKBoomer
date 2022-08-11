using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Sound
    public static event UnityAction<AudioManager.AudioList> PlayMusic;
    public static void OnPlayMusic(AudioManager.AudioList music) => PlayMusic?.Invoke(music);
    #endregion

    #region Utils
    public static event UnityAction<CanvasElement, int> FadeIn;
    public static void OnFadeIn(CanvasElement ce, int speed) => FadeIn?.Invoke(ce, speed);

    public static event UnityAction<CanvasElement, int> FadeOut;
    public static void OnFadeOut(CanvasElement ce, int speed) => FadeOut?.Invoke(ce, speed);

    public static event UnityAction<int> StartTransition;
    public static void OnStartTransition(int speed) => StartTransition?.Invoke(speed);

    public static event UnityAction<int> InfiniteRotate;
    public static void OnInfiniteRotate(int speed) => InfiniteRotate?.Invoke(speed);
    #endregion

    #region Saves
    public static event UnityAction<SavesData[]> LoadTimer;
    public static void OnLoadTimer(SavesData[] times) => LoadTimer?.Invoke(times);

    public static event UnityAction<string, float> SaveTime;
    public static void OnSaveTime(string valueString, float valueInt) => SaveTime?.Invoke(valueString, valueInt);
    #endregion

    #region Levels
    public static event UnityAction NextLevel;
    public static void OnNextLevel() => NextLevel?.Invoke();
    #endregion

    #region GameState
    public static event UnityAction<bool> Pause;
    public static void OnPause(bool value) => Pause?.Invoke(value);

    public static event UnityAction MainMenu;
    public static void OnMainMenu() => MainMenu?.Invoke();
    #endregion

    #region PlayerInputs
    public static event UnityAction<Vector2> Move;
    public static void OnMove(Vector2 move) => Move?.Invoke(move);

    public static event UnityAction<Vector2> Look;
    public static void OnLook(Vector2 look) => Look?.Invoke(look);

    public static event UnityAction<bool> Jump;
    public static void OnJump(bool jump) => Jump?.Invoke(jump);

    public static event UnityAction<bool> Shoot;
    public static void OnShoot(bool shoot) => Shoot?.Invoke(shoot);

    public static event UnityAction<bool> SpecialShoot;
    public static void OnSpecialShoot(bool specialShoot) => SpecialShoot?.Invoke(specialShoot);

    public static event UnityAction<int> ChangeWeapon;
    public static void OnChangeWeapon(int side) => ChangeWeapon?.Invoke(side);

    public static event UnityAction<bool> Crouch;
    public static void OnCrouch(bool crouch) => Crouch?.Invoke(crouch);

    public static event UnityAction Melee;
    public static void OnMelee() => Melee?.Invoke();
    #endregion
}
