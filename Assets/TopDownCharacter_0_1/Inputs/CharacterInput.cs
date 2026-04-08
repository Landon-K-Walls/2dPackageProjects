using System;
using UnityEngine;

namespace TopDownCharacter
{
  public class CharacterInput
  {
    public static ICharacterInput Provider { get; private set; }

    public static event Action<ICharacterInput> OnProviderSet;

    public static void Set(ICharacterInput input)
    {
      Provider = input;
      OnProviderSet?.Invoke(input);
    }
  }

  public interface ICharacterInput
  {
    Vector2 MousePosition { get; }
    Vector2 MovementInput { get; }
    bool IsSprinting { get; }
    bool IsAiming { get; }
  }
}