using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour, IDamageable
{
    #region Components
    protected Transform _transform;
    #endregion

    #region Parameters
    [SerializeField] protected int _currentHp;
    protected bool _isInmune = false;
    protected int _invulnerability;
    #endregion

    public virtual void TakeDamage(int value)
    {
        if (_isInmune) return;
        _isInmune = true;

        _currentHp -= value;

        StartCoroutine("InmuneReset");

        if (_currentHp <= 0)
            Death();
    }

    protected IEnumerator InmuneReset()
    {
        yield return new WaitForSeconds(_invulnerability);
        _isInmune = false;
    }

    protected abstract void Death();
}
