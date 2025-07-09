using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 20f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 2f;
    [SerializeField] private float _maxZoom = 50f;
    [SerializeField] private float _followSpeed = 5f;

    private Camera _camera;
    private float _targetOrthographicSize;
    
    [SerializeField] private Transform _pointer;
    public Transform Center { get; set; }

    private float _offset = 0f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetOrthographicSize = _camera.orthographicSize;
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float zoom = _targetOrthographicSize - scrollInput * _scrollSpeed;
        SetTargetZoom(zoom);

        _camera.orthographicSize =  Mathf.Lerp(_camera.orthographicSize, _targetOrthographicSize, Time.deltaTime * _zoomSpeed);
    }

    private void LateUpdate()
    {
        if (_pointer == null)
        {
            return;
        }
        
        Vector3 positionOffset = _pointer.position - Center.position;
        positionOffset *= _offset;
        positionOffset.y = Center.position.y;
        transform.position = Vector3.Lerp(transform.position, Center.position + positionOffset, _followSpeed * Time.deltaTime);
    }

    public void SetTargetZoom(float zoom)
    {
        zoom = Mathf.Clamp(zoom, _minZoom, _maxZoom);
        _targetOrthographicSize = zoom;
    }
    
    public void SetCameraOffset(float offset)
    {
        _offset = offset;
    }
}