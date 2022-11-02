using UnityEngine;

public class MoveUtils : MonoBehaviour
{
    // creado a ultimo momento
    [SerializeField] private Transform _object;
    private Vector3 _initPos;

    private void Awake()
    {
        _initPos = _object.position;
    }

    public void Move()
    {
        _object.position = _initPos;
    }
}
