using UnityEngine;

public static class Utils
{
    public static Vector3 CalculateDirection(Vector3 origin, Vector3 b)
    {
        return (b - origin).normalized;
    }
}