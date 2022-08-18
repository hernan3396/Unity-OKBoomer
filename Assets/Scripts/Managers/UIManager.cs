using UnityEngine;
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

    private void OnGodMode()
    {
        _uiElements[(int)Element.GodMode].Element.gameObject.SetActive(true);
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

    public void UpdateElement(int value)
    {
        Element.text = $"{BaseText}{value}";
    }

    public void UpdateElement(string value)
    {
        Element.text = $"{BaseText}{value}";
    }
}