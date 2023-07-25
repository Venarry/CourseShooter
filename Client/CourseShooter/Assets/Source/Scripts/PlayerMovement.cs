using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _jumpStrength = 0.5f;

    private IInputsHandler _inputsHandler;
    private CharacterController _characterController;

    private Vector3 _moveDirection;
    private float _gravityForce;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void FixedUpdate()
    {
        SetMoveDirection();

        _characterController.Move(_moveDirection);
        ReduceGravityForce();

        MovementData movementData = new(transform.position, _characterController.velocity.normalized * _speed);
        MultiplayerHolder.Instance.SendPlayerPosition("move", movementData);
    }

    private void Update()
    {
        TryJump();
    }

    public void Init(IInputsHandler inputsHandler)
    {
        _inputsHandler = inputsHandler;
    }

    private void SetMoveDirection()
    {
        _moveDirection = _inputsHandler.MoveDirection;
        _moveDirection = _moveDirection.normalized;
        _moveDirection *= _speed;
        _moveDirection.y = _gravityForce;
    }

    private void TryJump()
    {
        if (_inputsHandler.IsPressedKeyJump && _characterController.isGrounded)
        {
            _gravityForce = _jumpStrength;
        }
    }

    private void ReduceGravityForce()
    {
        float gravityReduceSpeed = 0.03f;
        float groudGravity = -1f;

        if (_characterController.isGrounded)
            _gravityForce = groudGravity;
        else
            _gravityForce -= gravityReduceSpeed;
    }
}
