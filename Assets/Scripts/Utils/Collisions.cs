using UnityEngine;
using UnityEngine.Events;

public class Collisions : MonoBehaviour
{
    // intento de script generico para colisiones
    public UnityEvent _onEnter;
    public UnityEvent _onExit;

    [SerializeField] private bool _useOneTime = false;

    [SerializeField] private string _tag;

    #region TriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (_useOneTime) gameObject.SetActive(false);
        if (other.gameObject.CompareTag(_tag))
            _onEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_tag))
            _onExit?.Invoke();
    }
    #endregion

    #region CollisionEnter    
    private void OnCollisionEnter(Collision other)
    {
        if (_useOneTime) gameObject.SetActive(false);
        if (other.gameObject.CompareTag(_tag))
            _onEnter?.Invoke();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(_tag))
            _onExit?.Invoke();
    }
    #endregion
}
