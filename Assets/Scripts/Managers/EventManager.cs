using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Fade in/out
    public static event UnityAction<CanvasElement, CanvasElement, int> MenuFadeOut;
    public static void OnMenuFadeOut(CanvasElement ce, CanvasElement nextCe, int timer) => MenuFadeOut?.Invoke(ce, nextCe, timer);

    public static event UnityAction<CanvasElement, int> MenuFadeIn;
    public static void OnMenuFadeIn(CanvasElement ce, int timer) => MenuFadeIn?.Invoke(ce, timer);
    #endregion
}
