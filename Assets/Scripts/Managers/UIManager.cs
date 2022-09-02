using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public enum Element
    {
        Hp,
        Weapon,
        Timer,
        Bullets,
        GodMode
    }

    #region UiElements
    [SerializeField] private UiElement[] _uiElements;
    #endregion

    private void Awake()
    {
        EventManager.UpdateUIValue += UpdateUIValue;
        EventManager.UpdateUIText += UpdateUIText;
        EventManager.GodMode += OnGodMode;
    }

    public void UpdateUIValue(Element element, int value)
    {
        _uiElements[(int)element].UpdateElement(value);
    }

    public void UpdateUIText(Element element, string value)
    {
        _uiElements[(int)element].UpdateElement(value);
    }

    private void OnGodMode(bool value)
    {
        _uiElements[(int)Element.GodMode].Element.gameObject.SetActive(value);
    }

    private void OnDestroy()
    {
        EventManager.UpdateUIValue -= UpdateUIValue;
        EventManager.UpdateUIText -= UpdateUIText;
        EventManager.GodMode -= OnGodMode;
    }
}

[Serializable]
public class UiElement
{
    public TMP_Text Element;
    public string BaseText;

    public bool AutoHide = false;
    public CanvasGroup CG;
    public float FadeDur = 1;

    public void UpdateElement(int value)
    {
        if (AutoHide) Show();
        Element.text = $"{BaseText}{value}";
    }

    public void UpdateElement(string value)
    {
        if (AutoHide) Show();
        Element.text = $"{BaseText}{value}";
    }

    private void Show()
    {
        CG.DOFade(1, FadeDur)
        .OnComplete(() => Hide());
    }

    private void Hide()
    {
        CG.DOFade(0, FadeDur);
    }
}