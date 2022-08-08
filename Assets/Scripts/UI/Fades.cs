using UnityEngine;
using DG.Tweening;

public class Fades : MonoBehaviour
{
    private void Awake()
    {
        EventManager.MenuFadeOut += FadeOut;
        EventManager.MenuFadeIn += FadeIn;
    }

    public void FadeOut(CanvasElement ce, CanvasElement nextCe, int timer)
    {
        ce.CanvasGroup.DOFade(0, timer)
        .OnComplete(() =>
        {
            ce.DeactivateCG();
            // se podria hacer con otro evento
            // pero de momento creo que no es necesario
            FadeIn(nextCe, timer);
        });
    }

    public void FadeIn(CanvasElement ce, int timer)
    {
        ce.CanvasGroup.DOFade(1, timer)
        .OnComplete(() => ce.ActivateCG());
    }
}