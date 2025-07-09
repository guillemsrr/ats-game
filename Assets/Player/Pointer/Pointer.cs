// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private float _areaOffset = 2f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _maximumDistance = 50f;

        private PointLocation[] _pointLocations;
        private Vector3 _velocity = Vector3.zero;
        private Camera _camera;

        public PointLocation TargetPointLocation { get; private set; }

        private bool _shouldMoveToMouse;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void FindPointLocationsArround(Vector3 position)
        {
            PointLocation[] allPointLocations = FindObjectsOfType<PointLocation>();
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
            var mousePos = Input.mousePosition;
            var worldPoint = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            transform.position = Vector3.SmoothDamp(transform.position, worldPoint, ref _velocity, _smoothTime);

            var direction = worldPoint - transform.position;
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
                Debug.Log("null target");
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

            foreach (var location in _pointLocations)
            {
                Vector3 p1World = location.transform.position;
                Vector3 p2World = p1World + location.transform.right * _areaOffset;

                Vector3 p1Screen = _camera.WorldToScreenPoint(p1World);
                Vector3 p2Screen = _camera.WorldToScreenPoint(p2World);

                Vector2 mousePos2D = (Vector2) Input.mousePosition;
                Vector2 closestPoint2D = FindClosestPointOnLineSegment(p1Screen, p2Screen, mousePos2D);
                float distance = Vector2.Distance(mousePos2D, closestPoint2D);
                Debug.Log("distance" + distance);

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