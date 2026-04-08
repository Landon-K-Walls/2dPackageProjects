using UnityEngine;

namespace TopDownCharacter
{
  public class TopDownCharacter : MonoBehaviour
  {
    public static void Lookat2D(Transform transform, Vector2 target)
    {
      Vector2 direction = target - (Vector2)transform.position;
      transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }
  }
}
