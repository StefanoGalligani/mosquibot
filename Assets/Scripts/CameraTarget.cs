using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private float edgeSize = 15f;
    [SerializeField] private float moveSpeed = 10f;

    void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouse = Mouse.current.position.ReadValue();

        float horizontal = 0f;
        float vertical = 0f;

        if (mouse.x < edgeSize)
            horizontal = -1f;
        else if (mouse.x > Screen.width - edgeSize)
            horizontal = 1f;

        if (mouse.y < edgeSize)
            vertical = -1f;
        else if (mouse.y > Screen.height - edgeSize)
            vertical = 1f;

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement.sqrMagnitude > 0f)
        {
            transform.position += movement.normalized
                                  * moveSpeed
                                  * Time.deltaTime;
        }
    }
}