using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private Rigidbody _rigidbody;

    private InputManager _inputManager;    
    private Vector3 _moveDirection = Vector3.zero;

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
        _moveDirection = _inputManager._playerMovement.action.ReadValue<Vector3>();
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(_moveDirection.x * _speed, _moveDirection.y * _speed, _moveDirection.z * _speed);
    }
}
