using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CCUtil.DebugUI
{
    public class DebugUITextBox : MonoBehaviour
    {
        [SerializeField]TextMeshProUGUI _textbox;

        List<TextBoxMessage> _messages = new List<TextBoxMessage>();
        CountdownTimer _messageSwapper = new CountdownTimer(3f, false, true);

        int i = 0;

        private void Awake()
        {
            _messageSwapper.RegisterEvent(() =>
            {
                ShowNextMessage();
            });
        }

        private void ShowNextMessage()
        {
            if (_messages.Count == 0)
            {
                _textbox.text = "";
            }
            if (_messages.Count == 1)
            {
                _textbox.text = _messages[0].text;
                return;
            }
            else
            {
                i++;
                if (i >= _messages.Count)
                    i = 0;

                _textbox.text = _messages[i].text;
            }
        }

        public void Add(TextBoxMessage message)
        {
            if(_messages.Count == 0)
                _textbox.text = message.text;
            _messages.Add(message);
            message.downTimer.RegisterEvent(() =>
            {
                Remove(message);
                message.downTimer.ClearEvents();
            });
        }

        private void Remove(TextBoxMessage message)
        {
            if (_messages.Count == 1)
                _textbox.text = "";
            _messages.Remove(message);
        }
    }

    public class TextBoxMessage
    {
        public string text;
        public CountdownTimer downTimer;

        public TextBoxMessage(string text, float time)
        {
            this.text = text;
            downTimer = new CountdownTimer(time);
        }
    }
}
