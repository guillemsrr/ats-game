// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Pointer _pointer;
        [SerializeField] private InputActionAsset _inputActionAsset;

        private InputAction _selectAction;

        private void OnEnable()
        {
            _inputActionAsset.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            _inputActionAsset.FindActionMap("Player").Disable();
        }

        private void Awake()
        {
            _selectAction = _inputActionAsset.FindAction("Select");
            _selectAction.performed += OnSelectStart;
            _selectAction.canceled += OnSelectReleased;
        }

        private void OnSelectStart(InputAction.CallbackContext obj)
        {
            if (!CheckTarget())
            {
                return;
            }

            _pointer.TargetPointLocation.SpatialButton.Click();
        }

        private void OnSelectReleased(InputAction.CallbackContext obj)
        {
            if (!CheckTarget())
            {
                return;
            }

            //_pointer.TargetPointLocation.Button.UnClick();
        }

        bool CheckTarget()
        {
            if (!_pointer.TargetPointLocation)
            {
                return false;
            }

            if (!_pointer.TargetPointLocation.SpatialButton)
            {
                return false;
            }

            return true;
        }
    }
}