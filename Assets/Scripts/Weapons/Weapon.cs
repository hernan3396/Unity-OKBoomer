using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] private TMP_Text _uiIndicator;
    [SerializeField] private WeaponScriptable _data;
    [SerializeField] private Transform _shootPos;
    [SerializeField] private int _currentBullets;
    private Animator _animator;

    private int _changeInHash;
    private int _changeOutHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _changeInHash = Animator.StringToHash("ChangeIn");
        _changeOutHash = Animator.StringToHash("ChangeOut");
    }

    #region Animations
    public void ChangeIn()
    {
        gameObject.SetActive(true);
        _animator.Play(_changeInHash);
    }

    public void ChangeOut()
    {
        _animator.Play(_changeOutHash);
    }
    #endregion

    #region Bullets
    public void UseBullets(int value)
    {
        _currentBullets -= value;
        UpdateBullets();
    }

    public void AddBullets(int addedBullets)
    {
        if (_currentBullets + addedBullets > _data.MaxAmmo)
            _currentBullets = _data.MaxAmmo;
        else
            _currentBullets += addedBullets;

        UpdateBullets();
    }

    public void UpdateBullets()
    {
        if (!_data.UseBullets) return;
        _uiIndicator.text = _currentBullets.ToString();
    }

    public void InitialBullets()
    {
        _currentBullets = _data.MaxAmmo;
    }
    #endregion

    #region Getter/Setter
    public int CurrentBullets
    {
        get { return _currentBullets; }
    }

    public WeaponScriptable Data
    {
        get { return _data; }
    }

    public Transform ShootPos
    {
        get { return _shootPos; }
    }
    #endregion
}
