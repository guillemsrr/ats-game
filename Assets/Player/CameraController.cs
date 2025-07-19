// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using Player;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 2f;
    [SerializeField] private float _maxZoom = 50f;
    [SerializeField] private float _followSpeed = 5f;

    [SerializeField] private float _minDistance = 5f;

    private Camera _camera;
    private float _targetOrthographicSize;

    public Transform Center { get; set; }

    private float _cameraZ;

    private bool _isAttachedToCenter = true;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetOrthographicSize = _camera.orthographicSize;
    }

    private void Start()
    {
        _cameraZ = transform.position.z;
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float zoom = _targetOrthographicSize - scrollInput * _scrollSpeed;
        SetTargetZoom(zoom);

        _camera.orthographicSize =
            Mathf.Lerp(_camera.orthographicSize, _targetOrthographicSize, Time.deltaTime * _zoomSpeed);
    }

    private void LateUpdate()
    {
        if (_isAttachedToCenter)
        {
            HandleCenterAttachment();
        }
        else
        {
            FollowMouse();
        }
    }

    public float GetCameraZoom()
    {
        return _camera.orthographicSize;
    }

    private void HandleCenterAttachment()
    {
        if (Center == null)
        {
            return;
        }

        // Get mouse world position on the same Y plane as Center
        Vector3 mouseWorldPosition =
            _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                _camera.nearClipPlane));
        mouseWorldPosition.y = Center.position.y;

        // Vector from center to mouse
        Vector3 offset = mouseWorldPosition - Center.position;

        if (offset.magnitude < _minDistance)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(Center.position.x, Center.position.y, _cameraZ),
                Time.deltaTime * _followSpeed);
            return;
        }

        // You can scale this to control how far the camera moves toward the mouse
        float followStrength = 0.2f; // 0 = no movement, 1 = full mouse offset

        Vector3 targetPos = Center.position + offset * followStrength;
        targetPos.z = _cameraZ;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _followSpeed);
    }

    public void SetTargetZoom(float zoom)
    {
        zoom = Mathf.Clamp(zoom, _minZoom, _maxZoom);
        _targetOrthographicSize = zoom;
    }

    public void AttachToCenter()
    {
        _isAttachedToCenter = true;
    }

    public void DettachFromCenter()
    {
        _isAttachedToCenter = false;
    }

    private void FollowMouse()
    {
        Vector3 mouseWorldPosition =
            _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                _camera.nearClipPlane));
        mouseWorldPosition.z = _cameraZ;

        transform.position = Vector3.Lerp(transform.position, mouseWorldPosition, Time.deltaTime * _followSpeed);
    }
}