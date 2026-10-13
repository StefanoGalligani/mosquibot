using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachine;
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
        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (!Mouse.current.leftButton.IsPressed()) return;

        if (Mouse.current.delta.value.magnitude < 1.5f) return;

        float scale = (_totalZoom - _zoomExtremes.x + 5) / (_zoomExtremes.y - _zoomExtremes.x);

        float horizontal = -Mouse.current.delta.value.x;
        float vertical = -Mouse.current.delta.value.y;

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