// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Menu
{
    public class ButtonHandler: MonoBehaviour
    {
        [SerializeField] private GameObject _button;
        [SerializeField] private MeshRenderer _meshRenderer;

        public UnityAction<ButtonHandler> OnClick;
        
        public bool IsClicked { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            IsClicked = !IsClicked;
            if (IsClicked)
            {
                _button.SetActive(false);
                //TODO: update material
            }
            else
            {
                _button.SetActive(true);
            }
            
            OnClick?.Invoke(this);
        }
    }
}