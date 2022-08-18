using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        Health,
        Ammo
    }

    [SerializeField] private int _ammount;
    [SerializeField] private PickupType _pickupType;

    private void OnTriggerEnter(Collider other)
    {
        // padre padre? funciona supongo
        // si es muy deforme pasarlo como un evento
        if (other.transform.parent.parent.TryGetComponent(out Player player))
        {
            switch (_pickupType)
            {
                case PickupType.Health:
                    player.PickUpHealth(_ammount);
                    break;
                case PickupType.Ammo:
                    player.PickUpAmmo(_ammount);
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
