using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private Rigidbody _rigidbody;

    private InputManager _inputManager;    
    private Vector2 _moveDirection;

    private void OnEnable()
    {
        //_playerMovement.Enable();
    }

    private void OnDisable()
    {
        //_playerMovement.Disable();
    }

    private void Start()
    {
        _inputManager = ServiceLocator.Instance.GetService<InputManager>();
    }

    private void Update()
    {
        _moveDirection = _inputManager._playerMovement.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(_moveDirection.x * _speed, 0, _moveDirection.y * _speed);
    }
}
