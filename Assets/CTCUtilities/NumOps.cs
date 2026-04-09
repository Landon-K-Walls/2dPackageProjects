using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil
{
  public static class NumOps
  {
    public static readonly float LbsKg = 2.205f;
    public static readonly float CmMCube = 1000000f;

    public static bool InRange(float f, float low, float high)
    {
      return f >= low && f <= high;
    }

    public static float Wrap(float value, float min, float max)
    {
      float range = max - min;
      if (range == 0) return min;
      return ((value - min) % range + range) % range + min;
    }
    public static int Wrap(int value, int min, int max)
    {
      return (int)Wrap((float)value, min, max);
    }
    public static double Wrap(double value, double min, double max)
    {
      return (double)Wrap((float)value, min, max);
    }

    public static void SetRotationX(float angle, Transform transform) =>
        transform.rotation = Quaternion.Euler(
        angle,
        transform.rotation.eulerAngles.y,
        transform.rotation.eulerAngles.z);

    public static void SetRotationY(float angle, Transform transform) =>
        transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x,
        angle,
        transform.rotation.eulerAngles.z);

    public static void SetRotationZ(float angle, Transform transform) =>
        transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x,
        transform.rotation.eulerAngles.y,
        angle);

    public static Vector3 BezierV3(Vector3 start, Vector3 control, Vector3 end, float t) =>
    Vector3.Lerp(
        Vector3.Lerp(start, control, t),
        Vector3.Lerp(control, end, t),
        t);

    public static Vector3 BezierV3(Vector3[] pathPoints, float t)
    {
      if (pathPoints.Length < 3)
        return Vector3.zero;

      List<Vector3> points = new List<Vector3>(pathPoints);

      for (int i = 0; i < pathPoints.Length; i++)
      {
        Vector3[] nextPoints = new Vector3[pathPoints.Length - i - 1];

        for (int x = 0; x < nextPoints.Length; x++)
        {
          nextPoints[x] = Vector3.Lerp(points[x], points[x + 1], t);
        }

        points = new List<Vector3>(nextPoints);

        if (points.Count == 2)
          return Vector3.Lerp(points[0], points[1], t);
      }

      return Vector3.zero;
    }

    public static Vector3 SphereToCartesian(float radius, float polar, float elevation)
    {
      float a = radius * Mathf.Cos(elevation * Mathf.Deg2Rad);

      return new Vector3(
          a * Mathf.Cos(polar * Mathf.Deg2Rad),
          radius * Mathf.Sin(elevation * Mathf.Deg2Rad),
          a * Mathf.Sin(polar * Mathf.Deg2Rad)
          );
    }

    public static Vector2 CircleToCoordinates(float angle, float radius)
    {
      return new Vector2(
        Mathf.Cos(angle * Mathf.Deg2Rad),
        Mathf.Sin(angle * Mathf.Deg2Rad)
      ) * radius;
    }

    public static float FlatMagnitude(Vector3 start, Vector3 end)
    {
      return (new Vector2(
        end.x,
        end.z
      ) - new Vector2(
        start.x,
        start.z
      )).magnitude;
    }

    public static void Lookat2D(Transform transform, Vector2 target)
    {
      Vector2 direction = target - (Vector2)transform.position;
      transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }
  }
}
