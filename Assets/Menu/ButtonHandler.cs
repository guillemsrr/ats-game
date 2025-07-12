// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Menu
{
    public class ButtonHandler : MonoBehaviour
    {
        public UnityAction<ButtonHandler> OnClick;
        public UnityAction OnHoverStart;
        public UnityAction OnHoverEnd;

        public bool IsClicked { get; private set; } = false;

        public bool _isActive = true;

        private void Awake()
        {
            UnClick();
        }

        private void OnMouseEnter()
        {
            if (!_isActive)
            {
                return;
            }

            OnHoverStart?.Invoke();
        }

        private void OnMouseExit()
        {
            if (!_isActive)
            {
                return;
            }

            OnHoverEnd?.Invoke();
        }

        private void OnMouseDown()
        {
            if (!_isActive)
            {
                return;
            }

            if (!IsClicked)
            {
                Click();
            }
            else
            {
                UnClick();
            }
        }

        public void Click()
        {
            SetClicked();
            OnClick?.Invoke(this);
        }

        public void UnClick()
        {
            IsClicked = false;
        }

        public void SetClicked()
        {
            IsClicked = true;
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
            UnClick();
        }
    }
}