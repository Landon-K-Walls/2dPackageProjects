using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CCUtil.PhysicsHelpers
{
  public class ObjectSearcher<T> where T : MonoBehaviour
  {
    public T[] SearchSphere(Vector3 position, float radius)
    {
      List<T> objectList = new List<T>();

      Collider[] colliders = Physics.OverlapSphere(
          position,
          radius);

      foreach (Collider collider in colliders)
      {
        T component = collider.GetComponent<T>();
        if (component != null)
          objectList.Add(component);
      }

      return objectList
      .OrderBy(x => (x.transform.position - position).sqrMagnitude)
      .ToArray();
    }


    public T[] SearchSphere(Vector3 position, float radius, int layerMask)
    {
      List<T> objectList = new List<T>();

      Collider[] colliders = Physics.OverlapSphere(
          position,
          radius,
          layerMask);

      foreach (Collider collider in colliders)
      {
        T component = collider.GetComponent<T>();
        if (component != null)
          objectList.Add(component);
      }

      return objectList
      .OrderBy(x => (x.transform.position - position).sqrMagnitude)
      .ToArray();
    }
  }
}
