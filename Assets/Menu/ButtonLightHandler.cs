// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;

namespace Menu
{
    public class ButtonLightHandler : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _incorrectMaterial;
        [SerializeField] private Material _correctMaterial;

        private void Start()
        {
            SetDefault();
        }

        public void SetDefault()
        {
            _meshRenderer.material = _defaultMaterial;
        }

        public void SetIncorrect()
        {
            _meshRenderer.material = _incorrectMaterial;
        }

        public void SetCorrect()
        {
            _meshRenderer.material = _correctMaterial;
        }
    }
}