using UnityEngine;

namespace TopDownCharacter
{
  public class TopDownVisualCharacter : MonoBehaviour
  {
    TopDownCharacter _boundCharacter;
    float _lookRotation;

    void Update()
    {
      RotateTowardsTarget();
      MoveTowardsTarget();
    }

    void RotateTowardsTarget()
    {
      if (_boundCharacter != null)
      {
        _lookRotation = Mathf.MoveTowardsAngle(_lookRotation, _boundCharacter.TargetLookAngle, Time.deltaTime * 360);
        TopDownCharacter.SetRotationZ(_lookRotation, transform);
      }
    }

    void MoveTowardsTarget()
    {
      if (_boundCharacter != null)
      {
        transform.position = Vector3.MoveTowards(transform.position, _boundCharacter.transform.position, 2 * Time.deltaTime);
      }
    }

    public void BindToCharacter(TopDownCharacter topDownCharacter)
    {
      _boundCharacter = topDownCharacter;
    }
  }
}
