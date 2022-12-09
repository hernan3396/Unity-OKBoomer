using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum Element
    {
        Hp,
        Weapon,
        Timer,
        GodMode
    }

    #region UiElements
    [SerializeField] private UiElement[] _uiElements;
    #endregion

    #region HealthImages
    [SerializeField] private CanvasGroup[] _hpCanvas;
    private int _healthImageIndex = 0;
    private int _lastHealth = 0;
    private int _imagesAmount = 0;
    private bool _firstUpdateImage = true;
    #endregion

    #region Vignette
    [SerializeField] private CanvasGroup _vignetteCanvas;
    private Image _vignetteImage;
    private bool _isVignetteFading = false;
    #endregion

    private void Awake()
    {
        _imagesAmount = _hpCanvas.Length;
        _vignetteImage = _vignetteCanvas.GetComponentInChildren<Image>();

        EventManager.UpdateUIValue += UpdateUIValue;
        EventManager.UpdateUIText += UpdateUIText;
        EventManager.GameOver += OnGameOver;
        EventManager.GodMode += OnGodMode;
        EventManager.OnGameStart();
    }

    public void UpdateUIValue(Element element, int value)
    {
        // solo se usa para la vida, ojo si se agrega algo mas
        _uiElements[(int)element].UpdateElement(value);

        if (_firstUpdateImage)
        {
            _healthImageIndex = _imagesAmount - 1 - Mathf.CeilToInt(value * _imagesAmount / 100);
            if (_healthImageIndex < 0) _healthImageIndex = 0;
            if (_healthImageIndex > _imagesAmount) _healthImageIndex = _imagesAmount - 1;

            _hpCanvas[_healthImageIndex].gameObject.SetActive(true);
            _hpCanvas[_healthImageIndex].alpha = 1;

            _firstUpdateImage = false;
            _lastHealth = value;
            return;
        }

        if (element == Element.Hp)
            ManageHealthImage(value);
    }

    private void OnGameOver()
    {
        _firstUpdateImage = true;
    }

    private void ManageHealthImage(int value)
    {
        int totalValue = 100;

        bool isDamage = value < _lastHealth; // si el value es menor a _lastHealth es que te hicieron daño, si es mayor es que te curaste
        _lastHealth = value;

        int aux = 1;
        if (!isDamage)
        {
            // no es daño
            VignetteSFX(false);
            aux = -1;
        }
        else
            VignetteSFX(true);

        if (_healthImageIndex + aux < 0 || _healthImageIndex + aux > _imagesAmount - 1) return;

        bool updateImage = value >= (totalValue / _imagesAmount) * (_imagesAmount - _healthImageIndex - 1);

        if (!isDamage) updateImage = value < (totalValue / _imagesAmount) * (_imagesAmount - _healthImageIndex - 1);
        if (updateImage) return;

        _hpCanvas[_healthImageIndex + aux].DOFade(1, 1f);

        _hpCanvas[_healthImageIndex].DOFade(0, 1f);

        _healthImageIndex += aux;
    }

    private void VignetteSFX(bool isDamage)
    {
        if (_isVignetteFading) return;

        _isVignetteFading = true;

        if (isDamage) _vignetteImage.color = Color.red;
        else _vignetteImage.color = Color.green;

        _vignetteCanvas.DOFade(0.4f, 0.2f)
                       .SetLoops(2, LoopType.Yoyo)
                       .SetUpdate(true)
                       .OnComplete(() => { _isVignetteFading = false; });
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
        EventManager.GameOver -= OnGameOver;
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