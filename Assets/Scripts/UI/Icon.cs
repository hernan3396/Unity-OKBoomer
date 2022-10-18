using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Icon : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _menu;
    private bool _dragging = false;
    private int _mouseBuffer = 0;

    private void Awake()
    {
        if (_menu == null) return;
        _menu.SetActive(false);
    }

    public void OnPointerDown(PointerEventData data)
    {
        _dragging = true;

        if (_menu == null) return;
        _mouseBuffer += 1;

        if (_mouseBuffer >= 2) ShowMenu();

        Invoke("RemoveBuffer", 0.5f);
    }

    public void OnPointerUp(PointerEventData data)
    {
        _dragging = false;
    }

    private void Update()
    {
        if (!_dragging) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = Vector2.Lerp(transform.position, mousePos, Time.deltaTime * 10);
    }

    private void ShowMenu()
    {
        _mouseBuffer = 0;
        _menu.SetActive(true);
        _menu.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }

    public void HideMenu()
    {
        _menu.GetComponent<CanvasGroup>().DOFade(0, 0.5f)
        .OnComplete(() => _menu.SetActive(false));
    }

    private void RemoveBuffer()
    {
        if (_mouseBuffer == 0) return;
        _mouseBuffer -= 1;
    }
}
