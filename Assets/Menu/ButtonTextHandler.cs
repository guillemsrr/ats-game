// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using GameJamBase.UI.View;
using UnityEngine;

namespace Menu
{
    public class ButtonTextHandler : MonoBehaviour
    {
        [SerializeField] private TextHandler _textHandler;
        [SerializeField] private SpatialButtonView SpatialButtonView;
        [SerializeField] private MeshRenderer _backgroundRenderer;
        [SerializeField] private GameObject _pointLocation;

        public SpatialButtonView SpatialButton => SpatialButtonView;

        public string Text => _textHandler.Text;

        private void Awake()
        {
            SpatialButtonView.OnHoverStart += OnHoverStart;
            SpatialButtonView.OnHoverEnd += OnHoverEnd;
            SpatialButtonView.OnClick += OnButtonClick;
        }

        public void Show()
        {
            SpatialButton.Activate();
            _pointLocation.SetActive(true);
            _textHandler.gameObject.SetActive(true);
            OnHoverEnd();
        }

        public void Hide()
        {
            SpatialButton.Deactivate();
            _pointLocation.SetActive(false);
            _textHandler.gameObject.SetActive(false);
        }

        public void SetUnclickable()
        {
            SpatialButton.Deactivate();
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

        private void OnButtonClick(SpatialButtonView arg0)
        {
            OnHoverEnd();
        }
    }
}