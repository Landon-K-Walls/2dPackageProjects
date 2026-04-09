using CCUtil.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil.DebugUI
{
  public class DebugUI : Singleton<DebugUI>
  {
    [SerializeField] DebugUITextBox _textBox;
    [SerializeField] DebugUIField[] _fields;

    public static void AddBoxMessage(float time, string text)
    {
      if (verify)
        instance._textBox.Add(new TextBoxMessage(text, time));
    }

    public static void AddBoxMessage(float time, object obj)
    {
      if (verify)
        instance._textBox.Add(new TextBoxMessage(obj.ToString(), time));
    }

    public static void SetField(int index, string text)
    {
      if (verify)
        if (NumOps.InRange(index, 0, instance._fields.Length - 1))
          instance._fields[index].Set(text);
    }

    public static void SetField(int index, object obj)
    {
      if (verify)
        if (NumOps.InRange(index, 0, instance._fields.Length - 1))
          instance._fields[index].Set(obj.ToString());
    }
  }
}
