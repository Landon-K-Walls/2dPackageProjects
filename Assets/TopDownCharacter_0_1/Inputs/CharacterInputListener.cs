using TopDownCharacter;
using UnityEngine;

public class CharacterInputListener : MonoBehaviour
{
  void OnEnable()
  {
    CharacterInput.OnProviderSet += HandleProviderSet;
  }

  void OnDisable()
  {
    CharacterInput.OnProviderSet -= HandleProviderSet;
  }

  void HandleProviderSet(ICharacterInput provider)
  {
    Register(provider);
  }

  void Register(ICharacterInput provider)
  {

  }
}
