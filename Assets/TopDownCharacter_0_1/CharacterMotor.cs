using UnityEngine;

namespace TopDownCharacter
{
  public class CharacterMotor : MonoBehaviour
  {
    Rigidbody2D _rigidBody;
    Vector2 _movementInput;
    Vector2 _velocity;
    [SerializeField] bool Aiming = false;
    [SerializeField] bool HaltVelocityTurn = false;
    [SerializeField] float _topSpeed = 3;
    [SerializeField] float _acceleration = 0.05f;

    void Awake()
    {
      _rigidBody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
      Aiming = CharacterInput.Provider.IsAiming;
      HaltVelocityTurn = Aiming;

      _movementInput = CharacterInput.Provider.MovementInput;

      if (Aiming)
        AimAtMouse();
      else if (!HaltVelocityTurn)
        TurnWithVelocity();
    }

    void FixedUpdate()
    {
      Move();
    }

    void Move()
    {
      _velocity = Vector2.MoveTowards(_velocity, _movementInput * _topSpeed, _acceleration);
      _rigidBody.MovePosition(_rigidBody.position + _velocity * Time.fixedDeltaTime);
    }

    void AimAtMouse()
    {
      TopDownCharacter.Lookat2D(transform, Camera.main.ScreenToWorldPoint(CharacterInput.Provider.MousePosition));
    }

    void TurnWithVelocity()
    {
      if (_velocity != Vector2.zero)
      {
        TopDownCharacter.Lookat2D(transform, (Vector2)transform.position + _velocity.normalized);
      }
    }
  }
}
