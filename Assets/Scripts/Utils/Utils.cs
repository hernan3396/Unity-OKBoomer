using UnityEngine;

public static class Utils
{
    public static Vector3 CalculateDirection(Vector3 origin, Vector3 b)
    {
        return (b - origin).normalized;
    }

    public static float CalculateDistance(Vector3 origin, Vector3 b)
    {
        return (b - origin).magnitude;
    }

    public static bool RayHit(Vector3 origin, Vector3 b, string tag, float range)
    {
        Debug.DrawRay(origin, b - origin, Color.blue);

        if (Physics.Raycast(origin, b - origin, out RaycastHit hit, range))
            if (hit.transform.CompareTag(tag))
                return true;

        return false;
    }
}