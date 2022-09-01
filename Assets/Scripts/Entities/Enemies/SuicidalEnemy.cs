using UnityEngine;
using DG.Tweening;

public class SuicidalEnemy : Enemy
{
    private PoolManager _explosionPool;
    [SerializeField] private Transform BodyPivot;

    [SerializeField] private float _floatingHeight = 1;
    [SerializeField] private float _floatingSpeed = 1;

    protected override void Start()
    {
        base.Start();
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
        FloatingAnim();
    }

    public void FloatingAnim()
    {
        _transform.DOMoveY(_floatingHeight, _floatingSpeed)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.Linear)
        .SetRelative(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        Death();
    }

    public override void Attacking()
    {
        Vector3 direction = Utils.CalculateDirection(_transform.position, _player.position);
        _rb.velocity = Vector3.Lerp(_rb.velocity, direction * _data.Speed * 2, Time.deltaTime);
    }

    protected override void Death()
    {
        base.Death();

        GameObject explosion = _explosionPool.GetPooledObject();
        if (!explosion) return;

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = _transform.position;
            explosion.SetActive(true);
            explosionScript.StartExplosion(_data.Weapon.Startup, 0.5f, _data.Weapon.Damage);
        }
    }
}