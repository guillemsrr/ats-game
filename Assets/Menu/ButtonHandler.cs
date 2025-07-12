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

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _clickClip;

        public bool _isActive = true;

        private void OnMouseEnter()
        {
            if (!_isActive)
            {
                return;
            }

            _audioSource.PlayOneShot(_hoverClip);
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

            Click();
        }

        public void Click()
        {
            _audioSource.PlayOneShot(_clickClip);
            OnClick?.Invoke(this);
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }
    }
}