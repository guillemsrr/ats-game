// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _maximumPointDistance = 10f;
        [SerializeField] private float _scaleRatio = 0.25f;
        [SerializeField] private float _scaleSpeed = 3f;

        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private Material _defaultMaterial;

        [SerializeField] private Material _scanningMaterial;
        [SerializeField] private Material _unscanningMaterial;

        private PointLocation[] _pointLocations;
        private Vector3 _velocity = Vector3.zero;
        private Camera _camera;

        private const float HEIGHT = 2f;

        private float _scaleTarget = 1;

        public PointLocation TargetPointLocation { get; private set; }

        private bool _shouldMoveToMouse;

        private void Start()
        {
            _camera = Camera.main;

            SetDefaultMaterial();
        }

        public void FindPointLocationsArround(Vector3 position)
        {
            PointLocation[] allPointLocations =
                Object.FindObjectsByType<PointLocation>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<PointLocation> filteredLocations = new System.Collections.Generic.List<PointLocation>();

            foreach (PointLocation location in allPointLocations)
            {
                float distance = Vector3.Distance(position, location.transform.position);
                if (distance <= _maximumPointDistance)
                {
                    filteredLocations.Add(location);
                }
            }

            _pointLocations = filteredLocations.ToArray();
        }

        private void Update()
        {
            if (_shouldMoveToMouse)
            {
                MoveToMouse();
            }
            else
            {
                MoveToClosestPoint();
            }

            UpdateScale();
        }

        private void UpdateScale()
        {
            float scale = Mathf.Lerp(transform.localScale.x, _scaleTarget, Time.deltaTime * _scaleSpeed);
            transform.localScale = Vector3.one * scale;
        }

        public void SetMoveToMouse(bool moveToMouse)
        {
            _shouldMoveToMouse = moveToMouse;
            TargetPointLocation = null;
        }

        private void MoveToMouse()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPoint = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            worldPoint.y = HEIGHT;
            transform.position = Vector3.SmoothDamp(transform.position, worldPoint, ref _velocity, _smoothTime);

            Vector3 direction = worldPoint - transform.position;
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }

        private void MoveToClosestPoint()
        {
            if (_pointLocations.Length == 0)
            {
                return;
            }

            TargetPointLocation = FindClosestTargetPosition();
            if (TargetPointLocation == null)
            {
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position, TargetPointLocation.transform.position,
                ref _velocity, _smoothTime);

            var targetRotation = Quaternion.LookRotation(TargetPointLocation.transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }

        private PointLocation FindClosestTargetPosition()
        {
            PointLocation closestPointLocation = null;
            float minDistance = float.MaxValue;

            foreach (PointLocation location in _pointLocations)
            {
                if (!location.isActiveAndEnabled)
                {
                    continue;
                }

                Vector3 pointPosition = location.transform.position;
                Vector3 p1Screen = _camera.WorldToScreenPoint(pointPosition);

                Vector2 mousePos2D = Input.mousePosition;
                float distance = Vector2.Distance(mousePos2D, p1Screen);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPointLocation = location;
                }
            }

            return closestPointLocation;
        }

        public void Scale(float cameraOrthographicSize)
        {
            if (cameraOrthographicSize <= 0f)
            {
                _scaleTarget = 1f;
                return;
            }

            _scaleTarget = cameraOrthographicSize * _scaleRatio;
        }

        public void SetScanningMaterial()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.material = _scanningMaterial;
            }
        }

        public void SetDefaultMaterial()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.material = _defaultMaterial;
            }
        }

        public void SetUnscanningMaterial()
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.material = _unscanningMaterial;
            }
        }
    }
}