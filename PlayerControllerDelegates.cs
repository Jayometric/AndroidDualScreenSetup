using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControllerDelegates : MonoBehaviour
{
    [Header("Input Actions")]
    private PlayerControls controls;

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
        controls = new PlayerControls();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.started += OnJump;
        controls.Player.Attack.performed += OnAttack;

        SetActiveCameras();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.started -= OnJump;
        controls.Player.Attack.performed -= OnAttack;
        controls.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Read the value here (only executes when the input changes/is active)
        Vector2 inputVector = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        Debug.Log($"Move Performed: {_moveDirection}");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset the value when input is released
        _moveDirection = Vector2.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("Move Canceled");
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (_UICamera.gameObject.activeSelf)
        {
            _gameCamera.targetDisplay = (_gameCamera.targetDisplay == _topScreen) ? _bottomScreen : _topScreen;
            _UICamera.targetDisplay = (_UICamera.targetDisplay == _topScreen) ? _bottomScreen : _topScreen;
        }
    }

    private void SetActiveCameras()
    {
        #if UNITY_EDITOR
        Debug.Log("Unity Editor");
        _gameCamera.targetDisplay = _topScreen;
        _UICamera.targetDisplay = _topScreen;

        #elif UNITY_STANDALONE_WIN
            Debug.Log("Windows Build");
            _UICamera.gameObject.SetActive(false);
            _gameCamera.targetDisplay = _topScreen;
            //_UICamera.targetDisplay = _topScreen;

        #elif UNITY_ANDROID
            Debug.Log("Android Build");
            _UICamera.gameObject.SetActive(true);
            _gameCamera.targetDisplay = _topScreen;
            _UICamera.targetDisplay = _bottomScreen;
    
        #endif
    }
}
