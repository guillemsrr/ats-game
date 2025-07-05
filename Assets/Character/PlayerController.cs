using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActionAsset;

        private InputAction _moveAction;
        private InputAction _sprintAction;
        private InputAction _clickAction;

        Camera _camera;

        [SerializeField] private float _movementSpeed = 20f;
        [SerializeField] private float _acceleration = 1.3f;
        [SerializeField] private float _smoothTime = 0.1f;

        private float _currentAcceleration = 1f;
        private Vector3 _currentVelocity;
        private Vector3 _smoothVelocity;

        private Vector2 _movementInput;

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
            _moveAction = InputSystem.actions.FindAction("Move");
            _sprintAction = InputSystem.actions.FindAction("Sprint");
            //_clickAction = InputSystem.actions.FindAction("Click");

            _sprintAction.performed += OnSprintPressed;
            _sprintAction.canceled += OnSprintReleased;

            _camera = Camera.main;
        }

        private void OnSprintReleased(InputAction.CallbackContext obj)
        {
            _currentAcceleration = 1f;
        }

        private void OnSprintPressed(InputAction.CallbackContext context)
        {
            _currentAcceleration = _acceleration;
        }

        private void Update()
        {
            _movementInput = _moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            Walk();
            FaceDirection();
        }

        private void Walk()
        {
            Vector3 cameraForward = _camera.transform.forward;
            Vector3 cameraRight = _camera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 targetVelocity = (cameraRight * _movementInput.x + cameraForward * _movementInput.y) *
                                     _movementSpeed * _currentAcceleration;
            _currentVelocity = Vector3.SmoothDamp(_currentVelocity, targetVelocity, ref _smoothVelocity, _smoothTime);

            transform.Translate(_currentVelocity * Time.fixedDeltaTime, Space.World);
        }

        private void FaceDirection()
        {
            if (_movementInput != Vector2.zero)
            {
                float angle = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }
}