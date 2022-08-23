using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private Transform _shootingPos;

    protected override void Attacking()
    {
        Debug.Log("Implementame");
    }
}
