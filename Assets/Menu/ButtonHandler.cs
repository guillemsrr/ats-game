// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Menu
{
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private Transform _buttonPivot;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _selectedMaterial;
        [SerializeField] private Material _unSelectedMaterial;
        [SerializeField] private float _offset;

        public UnityAction<ButtonHandler> OnClick;

        public bool IsClicked { get; private set; } = false;

        private void Awake()
        {
            DeactivateButton();
        }

        private void OnMouseDown()
        {
            if (!IsClicked)
            {
                ActivateButton();
            }
            else
            {
                DeactivateButton();
            }
        }

        public void ActivateButton()
        {
            IsClicked = true;
            _buttonPivot.localPosition -= Vector3.up * _offset;
            _meshRenderer.material = _selectedMaterial;

            OnClick?.Invoke(this);
        }

        public void DeactivateButton()
        {
            IsClicked = false;
            _buttonPivot.localPosition = Vector3.zero;
            _meshRenderer.material = _unSelectedMaterial;
        }
    }
}