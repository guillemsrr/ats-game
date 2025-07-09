// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Pointer = Player.Pointer;

namespace Scanner
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private Pointer _pointer;
        [SerializeField] private InputActionAsset _inputActionAsset;

        public UnityAction ScanStart;
        public UnityAction ScanEnd;

        private InputAction _scanAction;

        private void Awake()
        {
            _scanAction = _inputActionAsset.FindAction("Select");
            _scanAction.performed += OnScanStart;
            _scanAction.canceled += OnScanReleased;
        }

        private void OnScanStart(InputAction.CallbackContext obj)
        {
            ScanStart?.Invoke();
        }

        private void OnScanReleased(InputAction.CallbackContext obj)
        {
            ScanEnd?.Invoke();
        }
    }
}