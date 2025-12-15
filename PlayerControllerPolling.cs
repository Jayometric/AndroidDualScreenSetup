using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControllerPolling: MonoBehaviour
{
    [Header("Input Actions")]
    private PlayerControls controls;
    private InputAction moveAction;
    private InputAction jumpAction;

    [Header("Player Values")]
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _jumpForce;
    private Rigidbody rb;

    [Header("Camera Values")]
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Camera _UICamera;
    private int _topScreen = 0;
    private int _bottomScreen = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        moveAction = controls.Player.Move;
        jumpAction = controls.Player.Jump;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        if (inputVector.magnitude > 0.05f) // Use a small threshold/deadzone
        {
            Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
            rb.AddForce(movementVector * _speed, ForceMode.Force);
            transform.LookAt(transform.position + movementVector);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }

        if (jumpAction.ReadValue<float>() > 0.5f)
        {
            OnJump();
        }
    }

    public void OnJump()
    {
        rb.AddForce(_jumpForce);
    }
}
