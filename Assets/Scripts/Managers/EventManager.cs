using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
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
}
