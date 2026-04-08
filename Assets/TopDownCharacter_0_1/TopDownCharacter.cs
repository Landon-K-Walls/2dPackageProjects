using UnityEngine;

namespace TopDownCharacter
{
  public class TopDownCharacter : MonoBehaviour
  {
    [SerializeField] Transform _cameraTetherTransform;
    [SerializeField] Transform _visualCharacterTransform;

    void Awake()
    {

    }

    void OnEnable()
    {
      _cameraTetherTransform.SetParent(null, true);
    }

    void OnDisable()
    {
      if (_cameraTetherTransform != null)
        _cameraTetherTransform.SetParent(transform, true);
    }

    void Update()
    {
      UpdateCameraTetherPosition();
    }

    public static void Lookat2D(Transform transform, Vector2 target)
    {
      Vector2 direction = target - (Vector2)transform.position;
      transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    private void UpdateCameraTetherPosition()
    {
      float distanceMultiplier = Vector3.Distance(_cameraTetherTransform.position, transform.position - Vector3.forward * 15) + 1;
      _cameraTetherTransform.position = Vector3.MoveTowards(
          _cameraTetherTransform.position,
          transform.position + Vector3.back * 15,
          Time.deltaTime * distanceMultiplier);
    }
  }
}
