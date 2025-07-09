// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Resume
{
    public class PaperHandler : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _correctMaterial;
        [SerializeField] private Material _incorrectMaterial;

        public void SetDefault()
        {
            _meshRenderer.material = _defaultMaterial;
        }

        public void SetCorrect()
        {
            _meshRenderer.material = _correctMaterial;
        }

        public void SetIncorrect()
        {
            _meshRenderer.material = _incorrectMaterial;
        }
    }
}