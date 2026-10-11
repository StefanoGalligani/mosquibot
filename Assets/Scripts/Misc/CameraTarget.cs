using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachine;
    [SerializeField] private float _edgeSize = 15f;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 2f;
    private Vector3 _cameraDirection;
    private float _totalZoom;
    private Vector2 _zoomExtremes = new Vector2(-8, 15);

    void Start()
    {
        _cameraDirection = _cinemachine.GetComponent<CinemachineFollow>().FollowOffset;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;
        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {

        Vector2 mouse = Mouse.current.position.ReadValue();

        float scale = (_totalZoom - _zoomExtremes.x + 5) / (_zoomExtremes.y - _zoomExtremes.x);

        float horizontal = 0f;
        float vertical = 0f;

        if (mouse.x < _edgeSize)
            horizontal = -1f;
        else if (mouse.x > Screen.width - _edgeSize)
            horizontal = 1f;

        if (mouse.y < _edgeSize)
            vertical = -1f;
        else if (mouse.y > Screen.height - _edgeSize)
            vertical = 1f;

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement.sqrMagnitude > 0f) {
            Vector3 newPosition =
                transform.position + movement.normalized * scale * _moveSpeed * Time.deltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, -20 , 20);
            newPosition.z = Mathf.Clamp(newPosition.z, -20 , 20);
            transform.position = newPosition;
        }
    }

    private void ZoomCamera()
    {
        float zoomAmount = Mouse.current.scroll.ReadValue().y * Time.deltaTime * _zoomSpeed;

        if (_totalZoom > _zoomExtremes.x && zoomAmount > 0 ||
            _totalZoom < _zoomExtremes.y && zoomAmount < 0)
        {
            _totalZoom += -_cameraDirection.magnitude * zoomAmount;
            transform.position += -_cameraDirection * zoomAmount;
        }
    }
}