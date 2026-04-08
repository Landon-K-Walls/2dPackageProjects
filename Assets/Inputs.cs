using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using TopDownCharacter;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour, ICharacterInput
{
  void Awake()
  {
    CharacterInput.Set(this);
  }

  public Vector2 MousePosition => _mousePosition;
  private Vector2 _mousePosition;
  public void OnMousePosition(InputValue val)
  {
    _mousePosition = val.Get<Vector2>();
  }

  public Vector2 MovementInput => _movementInput;
  private Vector2 _movementInput;
  public void OnMove(InputValue val)
  {
    _movementInput = val.Get<Vector2>();
  }

  public bool IsSprinting => _isSprinting;
  private bool _isSprinting;
  public void OnSprint(InputValue val)
  {
    _isSprinting = val.Get<float>() == 1;
    Debug.Log(_isSprinting);
  }

  public bool IsAiming => _isAiming;
  private bool _isAiming;
  public void OnAim(InputValue val)
  {
    _isAiming = val.Get<float>() == 1;
  }
}
