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

    public static float CalculateDistanceNoHeight(Vector3 origin, Vector3 b)
    {
        return (new Vector3(origin.x, 0, origin.z) - new Vector3(b.x, 0, b.z)).magnitude;
    }

    public static string FloatToTime(float time)
    {
        float min;
        float sec;
        float msec;

        msec = (int)((time - (int)time) * 100);
        sec = (int)(time % 60);
        min = (int)(time / 60 % 60);

        return string.Format("{0:00}:{1:00}:{2:00}", min, sec, msec);
    }

    public static bool RayHit(Vector3 origin, Vector3 b, string tag, float range, LayerMask layer)
    {
        Debug.DrawRay(origin, b - origin, Color.blue);

        if (Physics.Raycast(origin, b - origin, out RaycastHit hit, range, layer))
            if (hit.transform.CompareTag(tag))
                return true;

        return false;
    }
}