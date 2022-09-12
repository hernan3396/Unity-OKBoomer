using UnityEngine;
using DG.Tweening;

public class DamageMarker : MonoBehaviour
{
    private Transform _player;
    private RectTransform _hitParent;
    private CanvasGroup _canvasGroup;
    private Vector3 _indicatorPos;
    private bool _updateMarker = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _hitParent = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _player = GameManager.GetInstance.Player.GetComponent<Transform>();
    }

    public void SetData(Vector3 pos)
    {
        _indicatorPos = pos;
    }

    public void ShowMarker()
    {
        _updateMarker = true;

        _canvasGroup.DOFade(1, 1)
        .OnComplete(() => HideMarker());
    }

    private void HideMarker()
    {
        _canvasGroup.DOFade(0, 5)
        .OnComplete(() => DeactivateMarker());
    }

    private void DeactivateMarker()
    {
        _updateMarker = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_updateMarker) return;

        UpdateHitMarker(_indicatorPos);
    }

    private void UpdateHitMarker(Vector3 pos)
    {
        Vector3 dir = Utils.CalculateDirection(_player.position, pos);
        Quaternion quat = Quaternion.LookRotation(dir);

        quat.z = -quat.y;
        quat.x = 0;
        quat.y = 0;

        Vector3 northDir = new Vector3(0, 0, _player.eulerAngles.y);
        _hitParent.localRotation = quat * Quaternion.Euler(northDir);
    }
}
