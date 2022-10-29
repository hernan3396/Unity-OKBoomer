using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Sound
    public static event UnityAction<AudioManager.OST> PlayMusic;
    public static void OnPlayMusic(AudioManager.OST music) => PlayMusic?.Invoke(music);

    public static event UnityAction<AudioManager.SFX> PlaySound;
    public static void OnPlaySound(AudioManager.SFX sfx) => PlaySound?.Invoke(sfx);

    public static event UnityAction<Vector3, AudioScriptable> Play3dSound;
    public static void OnPlay3dSound(Vector3 pos, AudioScriptable audioScript) => Play3dSound?.Invoke(pos, audioScript);

    public static event UnityAction<AudioSource, AudioScriptable> PlayOwn3dSound;
    public static void OnPlayOwn3dSound(AudioSource source, AudioScriptable audioScript) => PlayOwn3dSound?.Invoke(source, audioScript);
    #endregion

    #region Utils
    public static event UnityAction<int> StartTransition;
    public static void OnStartTransition(int speed) => StartTransition?.Invoke(speed);

    public static event UnityAction<int> StartTransitionOut;
    public static void OnStartTransitionOut(int speed) => StartTransitionOut?.Invoke(speed);

    public static event UnityAction<int> InfiniteRotate;
    public static void OnInfiniteRotate(int speed) => InfiniteRotate?.Invoke(speed);

    public static event UnityAction<Vector3> SpawnPickable;
    public static void OnSpawnPickable(Vector3 pos) => SpawnPickable?.Invoke(pos);

    public static event UnityAction<Vector3, int> SpawnSpecificPickable;
    public static void OnSpawnSpecificPickable(Vector3 pos, int value) => SpawnSpecificPickable?.Invoke(pos, value);
    #endregion

    #region Saves
    public static event UnityAction<SaveData> GameLoaded;
    public static void OnGameLoaded(SaveData data) => GameLoaded?.Invoke(data);

    public static event UnityAction Checkpoint;
    public static void OnCheckpoint() => Checkpoint?.Invoke();
    #endregion

    #region Levels
    public static event UnityAction NextLevel;
    public static void OnNextLevel() => NextLevel?.Invoke();

    public static event UnityAction<string> ChangeLevel;
    public static void OnChangeLevel(string sceneName) => ChangeLevel?.Invoke(sceneName);

    public static event UnityAction WaveUpdated;
    public static void OnWaveUpdated() => WaveUpdated?.Invoke();
    #endregion

    #region GameState
    public static event UnityAction<bool> Pause;
    public static void OnPause(bool value) => Pause?.Invoke(value);

    public static event UnityAction MainMenu;
    public static void OnMainMenu() => MainMenu?.Invoke();

    public static event UnityAction GameOver;
    public static void OnGameOver() => GameOver?.Invoke();

    public static event UnityAction GameStart;
    public static void OnGameStart() => GameStart?.Invoke();

    public static event UnityAction ResumeMenu;
    public static void OnResumeMenu() => ResumeMenu?.Invoke();

    public static event UnityAction<bool> GodMode;
    public static void OnGodMode(bool value) => GodMode?.Invoke(value);
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

    public static event UnityAction<int> PickUpWeapon;
    public static void OnPickUpWeapon(int value) => PickUpWeapon?.Invoke(value);

    public static event UnityAction<bool> Crouch;
    public static void OnCrouch(bool crouch) => Crouch?.Invoke(crouch);

    public static event UnityAction Melee;
    public static void OnMelee() => Melee?.Invoke();
    #endregion

    #region UI
    public static event UnityAction<UIManager.Element, int> UpdateUIValue;
    public static void OnUpdateUI(UIManager.Element element, int value) => UpdateUIValue?.Invoke(element, value);

    public static event UnityAction<UIManager.Element, string> UpdateUIText;
    public static void OnUpdateUIText(UIManager.Element element, string value) => UpdateUIText?.Invoke(element, value);

    public static event UnityAction<Vector3> PlayerHit;
    public static void OnPlayerHit(Vector3 pos) => PlayerHit?.Invoke(pos);
    #endregion
}
