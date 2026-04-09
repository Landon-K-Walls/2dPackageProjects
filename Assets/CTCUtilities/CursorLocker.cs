using UnityEngine;

public class CursorLocker : MonoBehaviour
{
  void Awake()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }
}
