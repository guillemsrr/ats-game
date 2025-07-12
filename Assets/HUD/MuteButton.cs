// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class MuteButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                OnButtonClick();
            }
        }

        void OnButtonClick()
        {
            AudioManager.Instance.ToggleMute();

            if (AudioManager.Instance.IsMuted)
            {
                _text.text = "Music Off (M)";
            }
            else
            {
                _text.text = "Music On (M)";
            }
        }
    }
}