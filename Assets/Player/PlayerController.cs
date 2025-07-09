using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActionAsset;

        private InputAction _moveAction;
        private InputAction _sprintAction;
        private InputAction _clickAction;

        [SerializeField] private float _movementSpeed = 20f;
        [SerializeField] private float _acceleration = 1.3f;
        [SerializeField] private float _smoothTime = 0.1f;

        private float _currentAcceleration = 1f;
        private Vector2 _movementInput;

        [SerializeField] private Vector3 _ellipseCenter = Vector3.zero;
        [SerializeField] private float _radiusA = 10f;
        [SerializeField] private float _radiusB = 5f;
        [SerializeField] private float _rotationSpeed = 1f;

        private float _angle = 0f;

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
            //TODO: reuse input?
            _movementInput = _moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            // Increment the angle over time to rotate
            _angle += _rotationSpeed* _currentAcceleration * Time.fixedDeltaTime;

            // Calculate the new position on the ellipse
            float x = _ellipseCenter.x + _radiusA * Mathf.Cos(_angle);
            float z = _ellipseCenter.z + _radiusB * Mathf.Sin(_angle);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);

            // Update the position and look at the center
            transform.position = newPosition;
            transform.LookAt(_ellipseCenter);
        }
    }
}