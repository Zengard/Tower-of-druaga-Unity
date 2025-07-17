using System.Collections;
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
    float lastInputX;
    float lastInputY;

    private bool _isAttack;
    private float _timeBeforeMove = 0.3f;
   

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        _inputMaster = new InputMaster();
        _inputMaster.Player.Enable();
        //_inputMaster.Player.Movement.performed += Movement_performed;
        _inputMaster.Player.Attack.performed += Attack;

    }

    private void Update()
    {
        _inputVector = _inputMaster.Player.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (_isAttack == true)
            return;

        transform.position += new Vector3(_inputVector.x, 0, _inputVector.y).normalized * _speed * Time.fixedDeltaTime;

        AnimationMovement();
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        transform.position += new Vector3(inputVector.x, 0, inputVector.y) * _speed * Time.deltaTime;
    }

    private void Attack(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            StartCoroutine(StartAnimationAttack());
        }
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

             lastInputX = _inputVector.x;
             lastInputY = _inputVector.y;
        }

        _animator.SetBool(AnimatorHashes.MovingStateHash, _isMoving);
    }

    private void AnimationAttack() 
    {

        _isAttack = true;

        _animator.SetFloat("X", lastInputX);
        _animator.SetFloat("Y", lastInputY);

        _animator.SetBool(AnimatorHashes.AtackStateHash, _isAttack);
    }

    private IEnumerator StartAnimationAttack() 
    {
        AnimationAttack();
        yield return new WaitForSeconds(_timeBeforeMove);
        _isAttack = false;
        _animator.SetBool(AnimatorHashes.AtackStateHash, _isAttack);
    }
}
