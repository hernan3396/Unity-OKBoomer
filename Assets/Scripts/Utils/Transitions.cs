using UnityEngine;
using DG.Tweening;

public class Transitions : MonoBehaviour
{
    [SerializeField] private RectTransform _transitionPanel;
    [SerializeField] private Ease _ease = Ease.OutExpo;

    private void Awake()
    {
        EventManager.StartTransition += TransitionIn;
        EventManager.StartTransitionOut += TransitionOut;
    }

    private void TransitionIn(int speed)
    {
        _transitionPanel.localScale = new Vector3(0, 1, 1);
        _transitionPanel.gameObject.SetActive(true);

        _transitionPanel.DOScaleX(1, speed)
        .SetEase(_ease);
    }

    private void TransitionOut(int speed)
    {
        _transitionPanel.localScale = new Vector3(1, 1, 1);
        _transitionPanel.gameObject.SetActive(true);

        _transitionPanel.DOScaleX(0, speed)
        .SetEase(_ease)
        .OnComplete(() => _transitionPanel.gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        EventManager.StartTransition -= TransitionIn;
        EventManager.StartTransitionOut -= TransitionOut;
    }
}
