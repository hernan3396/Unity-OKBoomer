using UnityEngine;
using UnityEngine.Events;

public class EventsUtils : MonoBehaviour
{
    public UnityEvent Event;

    public void LaunchEvent()
    {
        Event?.Invoke();
    }
}
