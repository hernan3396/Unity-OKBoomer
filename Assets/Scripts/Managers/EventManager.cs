using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Fades
    public static event UnityAction<CanvasElement, int> FadeIn;
    public static void OnFadeIn(CanvasElement ce, int speed) => FadeIn?.Invoke(ce, speed);

    public static event UnityAction<CanvasElement, int> FadeOut;
    public static void OnFadeOut(CanvasElement ce, int speed) => FadeOut?.Invoke(ce, speed);
    #endregion

    #region Transitions
    public static event UnityAction<int> StartTransition;
    public static void OnStartTransition(int speed) => StartTransition?.Invoke(speed);
    #endregion

    #region CameraUtils
    public static event UnityAction<int> InfiniteRotate;
    public static void OnInfiniteRotate(int speed) => InfiniteRotate?.Invoke(speed);
    #endregion
}
