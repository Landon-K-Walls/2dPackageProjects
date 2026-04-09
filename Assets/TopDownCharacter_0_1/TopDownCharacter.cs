using UnityEngine;

namespace TopDownCharacter
{
  public class TopDownCharacter : MonoBehaviour
  {
    [SerializeField] Transform _cameraTetherTransform;
    public Transform CameraTether => _cameraTetherTransform;
    [SerializeField] TopDownVisualCharacter _visualCharacter;
    public TopDownVisualCharacter VisualCharacter => _visualCharacter;
    CharacterMotor _motor;

    public float TargetLookAngle;

    void Awake()
    {
      _cameraTetherTransform.SetParent(null, true);
      _visualCharacter.transform.SetParent(null, true);
      _visualCharacter.BindToCharacter(this);

      _motor = GetComponent<CharacterMotor>();
    }

    private void UpdateCameraTetherPosition()
    {
      float distanceMultiplier = Vector3.Distance(_cameraTetherTransform.position, transform.position - Vector3.forward * 15) + 1;
      _cameraTetherTransform.position = Vector3.MoveTowards(
          _cameraTetherTransform.position,
          transform.position + Vector3.back * 15,
          Time.deltaTime * distanceMultiplier);
    }

    void Update()
    {
      UpdateCameraTetherPosition();
      TargetLookAngle = transform.rotation.eulerAngles.z;
      if (CharacterInput.Provider.MovementInput == Vector2.zero)
        transform.position = Vector3.MoveTowards(transform.position, _visualCharacter.transform.position, Time.deltaTime * 3);
    }

    public static void Lookat2D(Transform transform, Vector2 target)
    {
      Vector2 direction = target - (Vector2)transform.position;
      transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }
    public static void SetRotationZ(float angle, Transform transform) =>
        transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x,
        transform.rotation.eulerAngles.y,
        angle);
  }
}
