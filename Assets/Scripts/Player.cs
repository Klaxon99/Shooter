using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _strafeSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _verticalMinAngle;
    [SerializeField] private float _verticalMaxAngle;
    [SerializeField] private float _verticalSensivity;
    [SerializeField] private float _horizontalSensivity;
    [SerializeField] private float _gravityFactor;

    [Header("Shoot")]
    [SerializeField] private Shotgun _shotgun;

    private CharacterController _characterController;
    private float _verticalCameraAngle;
    private Vector3 _verticalVelocity;

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _verticalCameraAngle = _cameraTransform.localEulerAngles.x;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _shotgun.Shoot(_cameraTransform.position, _cameraTransform.forward);
        }

        Movement();
    }

    private void Movement()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        Vector3 playerMove = forward * Input.GetAxis("Vertical") * _movementSpeed + right * Input.GetAxis("Horizontal") * _strafeSpeed;

        _verticalCameraAngle -= Input.GetAxis("Mouse Y") * _verticalSensivity;
        _verticalCameraAngle = Mathf.Clamp(_verticalCameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransform.localEulerAngles = Vector3.right * _verticalCameraAngle;

        _transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _horizontalSensivity);

        if (_characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalVelocity = Vector3.up * _jumpForce;
            }
            else
            {
                _verticalVelocity = Vector3.down;
            }

            _characterController.Move((playerMove + _verticalVelocity) * Time.deltaTime);
        }
        else
        {
            Vector3 horizontalVelocity = _characterController.velocity;
            horizontalVelocity.y = 0;
            _verticalVelocity += Physics.gravity * _gravityFactor * Time.deltaTime;
            _characterController.Move((horizontalVelocity + _verticalVelocity) * Time.deltaTime);
        }
    }
}
