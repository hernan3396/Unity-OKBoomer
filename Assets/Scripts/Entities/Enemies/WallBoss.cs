using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(UtilTimer))]
public class WallBoss : Enemy
{
    enum LaserPos
    {
        Top,
        Bottom,
        Crazy
    }

    public UnityEvent DeathEvent;
    [SerializeField] private Transform _finalPos;

    [Header("Lasers")]
    [SerializeField] private Transform[] _lasers;
    [SerializeField] private Transform[] _lasersInitPos;
    [SerializeField] private Transform[] _laserEndPos;
    private Tween _crazyTween;
    private float _laserTime;
    private int _maxLaser;

    private PoolManager _explosionPool;
    private int _headDestroyed = 0;
    private Tween _movingTween;

    protected override void Start()
    {
        base.Start();
        _laserTime = _data.VisionRange; // es 10
        _maxLaser = _lasersInitPos.Length - 1;
        _explosionPool = GameManager.GetInstance.GetUtilsPool(0);
    }

    private void OnEnable()
    {
        EventManager.OnStartProgressBar(_data.MaxHealth);
    }

    public override void Attacking()
    {
        return;
    }

    public void StartLaser()
    {
        StartCoroutine("ChargeLaser");
    }

    private IEnumerator ChargeLaser()
    {
        Transform laserSelected = SelectLaser();

        int randPos = Random.Range(0, _maxLaser);

        laserSelected.position = _lasersInitPos[randPos].position;
        laserSelected.gameObject.SetActive(true);

        yield return new WaitForSeconds(_data.AttackRange);
        ShootLasers(laserSelected, randPos);
    }

    public void ShootLasers(Transform laserSelected, int randPos)
    {
        // disparar lasers
        laserSelected.DOMove(_laserEndPos[randPos].position, 10)
        .OnComplete(() => { laserSelected.gameObject.SetActive(false); });

        if (randPos == (int)LaserPos.Crazy)
        {
            _crazyTween.Play();
            _crazyTween = laserSelected.DOMoveY(laserSelected.position.y - 15, 1.5f)
           .SetLoops(-1, LoopType.Yoyo)
           .OnComplete(() => { _crazyTween.Kill(); });
        }
    }

    private Transform SelectLaser()
    {
        for (int i = 0; i < _lasers.Length; i++)
            if (!_lasers[i].gameObject.activeInHierarchy)
                return _lasers[i];

        return null;
    }

    public void HeadDestroyed()
    {
        _headDestroyed += 1;
        _laserTime -= 1;

        if (_headDestroyed >= 4) _maxLaser = _lasersInitPos.Length;
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        EventManager.OnUpdateProgressBar(_data.MaxHealth - _currentHp);
    }

    public void Dying()
    {
        EventManager.OnBulletTime(0.5f);

        if (_audio != null)
            _audio.PlaySound((int)SFX.Death);

        _headMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.InSine);

        _mainMat.DOFloat(1, "_DissolveValue", _data.DeathDur)
        .SetEase(Ease.InSine)
        .OnComplete(() =>
        {
            EventManager.OnResetTime();
            DeathEvent?.Invoke();
        });
    }

    public void CreateExplosion()
    {
        GameObject explosion = _explosionPool.GetPooledObject();

        if (!explosion) return;
        Vector3 explosionPos = _headPos.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        if (explosion.TryGetComponent(out Explosion explosionScript))
        {
            explosion.transform.position = explosionPos;
            explosion.SetActive(true);
            explosionScript.HarmlessExplosion(_data.DodgeRange, _data.DodgeSpeed);
        }
    }

    protected override void Death()
    {
        if (_isDead) return;

        _isDead = true;
    }

    protected override void Respawn()
    {
        _currentHp = _data.MaxHealth;
        EventManager.OnDeactivateProgressBar();
        gameObject.SetActive(false);
        _headDestroyed = 0;
        _laserTime = _data.VisionRange;
        _maxLaser = _lasersInitPos.Length - 1;

        foreach (Transform laser in _lasers)
            laser.gameObject.SetActive(false);

        if (_crazyTween != null) _crazyTween.Kill();
    }

    public float LaserTime
    {
        get { return _laserTime; }
    }
}
