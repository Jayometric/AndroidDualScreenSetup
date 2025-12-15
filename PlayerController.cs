using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _jumpForce;
    [SerializeField] private Vector3 _moveDirection;
    private Rigidbody rb;

    [Header("Camera Values")]
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Camera _UICamera;
    private int _topScreen = 0;
    private int _bottomScreen = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the value here (only executes when the input changes/is active)
        Vector2 inputVector = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        Debug.Log($"Move Performed: {_moveDirection}");
    }

    void FixedUpdate()
    {
        // Apply a continuous force in the stored direction
        if (_moveDirection.magnitude > 0.1f)
        {
            // Use ForceMode.Force for continuous acceleration, or ForceMode.VelocityChange 
            // to immediately reach target velocity (if Drag is 0)
            rb.AddForce(_moveDirection * _speed, ForceMode.Force);
            transform.LookAt(transform.position + _moveDirection);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnJump();
    }

    public void OnJump()
    {
        rb.AddForce(_jumpForce);
    }
}
