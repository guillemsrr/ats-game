// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using Resume.Base;
using UnityEngine;

namespace Menu
{
    public class ButtonTextHandler: MonoBehaviour
    {
        [SerializeField] private TextHandler _textHandler;
        [SerializeField] private ButtonHandler _buttonHandler;
        [SerializeField] private MeshRenderer _backgroundRenderer;
        
        public ButtonHandler Button => _buttonHandler;

        public string Text => _textHandler.Text;

        private void Awake()
        {
            _buttonHandler.OnHoverStart += OnHoverStart;
            _buttonHandler.OnHoverEnd += OnHoverEnd;
        }

        public void Show()
        {
            Button.Activate();
            gameObject.SetActive(true);
            OnHoverEnd();
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
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
    }
}