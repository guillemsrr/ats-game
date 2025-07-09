// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _maximumDistance = 50f;

        private PointLocation[] _pointLocations;
        private Vector3 _velocity = Vector3.zero;
        private Camera _camera;

        private const float HEIGHT = 2f;

        public PointLocation TargetPointLocation { get; private set; }

        private bool _shouldMoveToMouse;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void FindPointLocationsArround(Vector3 position)
        {
            PointLocation[] allPointLocations =
                Object.FindObjectsByType<PointLocation>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<PointLocation> filteredLocations = new System.Collections.Generic.List<PointLocation>();

            foreach (PointLocation location in allPointLocations)
            {
                float distance = Vector3.Distance(position, location.transform.position);
                if (distance <= _maximumDistance)
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
                if (!location.enabled)
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

        private Vector2 FindClosestPointOnLineSegment(Vector2 segmentStart, Vector2 segmentEnd, Vector2 point)
        {
            var segment = segmentEnd - segmentStart;
            if (segment.sqrMagnitude < 1e-5)
            {
                return segmentStart;
            }

            var t = Vector2.Dot(point - segmentStart, segment) / segment.sqrMagnitude;
            t = Mathf.Clamp01(t);

            return segmentStart + t * segment;
        }
    }
}