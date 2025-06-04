using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float _speed = 3.0f;
    private float _horizontalInput;
    private float _forwardInput;
    
    void Update()
    {
        _forwardInput = Input.GetAxis("Vertical");
        _horizontalInput =Input.GetAxis("Horizontal");       

        transform.Translate(Vector3.forward * Time.deltaTime * _speed * _forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * _speed * _horizontalInput);
    }
}
