// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using Resume.Base;
using Unity.VisualScripting;
using UnityEngine;

namespace Menu
{
    public class ButtonTextHandler : MonoBehaviour
    {
        [SerializeField] private TextHandler _textHandler;
        [SerializeField] private ButtonHandler _buttonHandler;
        [SerializeField] private MeshRenderer _backgroundRenderer;
        [SerializeField] private GameObject _pointLocation;

        public ButtonHandler Button => _buttonHandler;

        public string Text => _textHandler.Text;

        private void Awake()
        {
            _buttonHandler.OnHoverStart += OnHoverStart;
            _buttonHandler.OnHoverEnd += OnHoverEnd;
            _buttonHandler.OnClick += OnButtonClick;
        }

        public void Show()
        {
            Button.Activate();
            _pointLocation.SetActive(true);
            _textHandler.gameObject.SetActive(true);
            OnHoverEnd();
        }

        public void Hide()
        {
            Button.Deactivate();
            _pointLocation.SetActive(false);
            _textHandler.gameObject.SetActive(false);
        }

        public void SetUnclickable()
        {
            Button.Deactivate();
            _pointLocation.SetActive(false);
            _textHandler.SetColor(Color.gray);
        }

        private void OnHoverStart()
        {
            _textHandler.SetColor(Color.black);
            _backgroundRenderer.gameObject.SetActive(true);
        }

        private void OnHoverEnd()
        {
            _textHandler.SetColor(Color.white);
            _backgroundRenderer.gameObject.SetActive(false);
        }

        private void OnButtonClick(ButtonHandler arg0)
        {
            OnHoverEnd();
        }
    }
}