using CCUtil.DebugUI;
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
        _lookRotation = Mathf.MoveTowardsAngle(_lookRotation, _boundCharacter.TargetLookAngle, Time.deltaTime * 360 * 1.2f);
        TopDownCharacter.SetRotationZ(_lookRotation, transform);
      }
    }

    void MoveTowardsTarget()
    {
      if (_boundCharacter != null)
      {
        float distanceScalar = Mathf.Pow(
          Vector3.Distance(
            transform.position,
            _boundCharacter.transform.position
          ), 1.5f
        ) + 1.5f;
        //transform.position = Vector3.MoveTowards(transform.position, _boundCharacter.transform.position, distanceScalar * Time.deltaTime);
        transform.position = _boundCharacter.transform.position;
        DebugUI.SetField(0, distanceScalar);
      }
    }

    public void BindToCharacter(TopDownCharacter topDownCharacter)
    {
      _boundCharacter = topDownCharacter;
    }
  }
}
