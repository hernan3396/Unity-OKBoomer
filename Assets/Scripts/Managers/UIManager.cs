using UnityEngine;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public enum Element
    {
        Hp,
        Bullets,
        Timer
    }

    #region UiElements
    [SerializeField] private UiElement[] _uiElements;
    #endregion

    public void UpdateUIValue(Element element, int value)
    {
        _uiElements[(int)element].UpdateElement(value);
    }

    public void UpdateUIText(Element element, string value)
    {
        _uiElements[(int)element].UpdateElement(value);
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
        Element.text = value;
    }
}