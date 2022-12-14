using UnityEngine;
using DG.Tweening;

public class Explosion : MonoBehaviour
{
    private Transform _transform;
    private Material _mainMat;
    private PlayAudio _audio;
    private Player _player;

    private UtilTimer _utilTimer;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _utilTimer = GetComponent<UtilTimer>();
        _mainMat = GetComponent<MeshRenderer>().material;

        if (TryGetComponent(out PlayAudio audio))
            _audio = audio;

        _utilTimer.onTimerCompleted += DisableExplosion;
    }

    public void StartExplosion(float size, float lifeTime, int damage)
    {
        if (_audio != null)
            _audio.PlaySound();

        _mainMat.SetFloat("_DissolveValue", 1);
        _mainMat.DOFloat(0, "_DissolveValue", 1)
        .SetEase(Ease.OutQuint);

        _transform.localScale = new Vector3(size, size, size);

        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _transform.localScale.x * 0.5f); // ve contra que choca la explosion

        foreach (Collider collider in hitColliders)
        {
            Transform otherTransform = collider.transform;

            if (otherTransform.CompareTag("Player"))
                GameManager.GetInstance.Player.GetComponent<Player>().TakeDamage(Mathf.CeilToInt(damage * 0.5f), _transform.position);

            // si el enemigo tiene Rigidbody (en este caso el volador)
            // choca contra el gameobject que lo tenga, por eso parece repetido
            // con el primero
            if (otherTransform.TryGetComponent(out Enemy enemyRB))
                enemyRB.TakeDamage(damage, otherTransform);

            if (otherTransform.TryGetComponent(out Breakable breakable))
                breakable.TakeDamage(damage);

            if (otherTransform.parent == null) return;

            if (otherTransform.parent.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(damage);
        }

        _utilTimer.StartTimer(lifeTime);
    }

    private void DisableExplosion()
    {
        _mainMat.DOFloat(1, "_DissolveValue", 1)
        .SetEase(Ease.OutQuint)
        .OnComplete(() => gameObject.SetActive(false));
    }

    public void HarmlessExplosion(float size, float lifeTime)
    {
        if (_audio != null)
            _audio.PlaySound();

        _mainMat.SetFloat("_DissolveValue", 1);
        _mainMat.DOFloat(0, "_DissolveValue", 1)
        .SetEase(Ease.OutQuint);

        _transform.localScale = new Vector3(size, size, size);
        _utilTimer.StartTimer(lifeTime);
    }

    private void OnDestroy()
    {
        _utilTimer.onTimerCompleted -= DisableExplosion;
    }
}
