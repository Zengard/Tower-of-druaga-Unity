using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputMaster _inputMaster;

    [SerializeField] private float _speed;
    Vector2 _inputVector;

    private Animator _animator;
    private bool _isMoving;
   

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _inputMaster = new InputMaster();
        _inputMaster.Player.Enable();
        //_inputMaster.Player.Movement.performed += Movement_performed;

    }

    private void Update()
    {
        _inputVector = _inputMaster.Player.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_inputVector.x, 0, _inputVector.y).normalized * _speed * Time.fixedDeltaTime;

        AnimationMovement();
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        transform.position += new Vector3(inputVector.x, 0, inputVector.y) * _speed * Time.deltaTime;
    }

    private void AnimationMovement()
    {
        if (_inputVector.magnitude > 0.1f || _inputVector.magnitude < -0.1f) 
            _isMoving = true;
        else
            _isMoving = false;

        if(_isMoving) 
        {
            _animator.SetFloat("X", _inputVector.x);
            _animator.SetFloat("Y", _inputVector.y);
        }

        _animator.SetBool(AnimatorHashes.MovingStateHash, _isMoving);
    }
}
