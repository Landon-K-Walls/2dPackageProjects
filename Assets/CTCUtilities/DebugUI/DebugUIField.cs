using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CCUtil.DebugUI
{
    public class DebugUIField : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _field;
        string _text = "";

        public void Set(string s)
        {
            _text = s;
        }

        private void Update()
        {
            _field.text = _text;
        }
    }
}
