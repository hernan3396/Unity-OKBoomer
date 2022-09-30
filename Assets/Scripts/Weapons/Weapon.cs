using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] private TMP_Text _uiIndicator;
    [SerializeField] private WeaponScriptable _data;
    [SerializeField] private Transform[] _shootPos;
    [SerializeField] private int _currentBullets;
    private int _lastShootPos = 0;
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

    public void LoadBullets(int bullets)
    {
        _currentBullets = bullets;
    }

    public Transform GetShootPos()
    {
        Transform pos = _shootPos[_lastShootPos];
        _lastShootPos += 1;

        if (_lastShootPos >= _shootPos.Length)
            _lastShootPos = 0;

        return pos;
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
        get { return _shootPos[0]; }
    }
    #endregion
}
