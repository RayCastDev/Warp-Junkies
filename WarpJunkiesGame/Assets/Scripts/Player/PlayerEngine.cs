using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(IGroundChecker), typeof(CollisionReactor))]
public class PlayerEngine : MonoBehaviour
{
    [SerializeField] private float _originalSpeed = 10.0f;
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _gravity = 12.0f;
    [SerializeField] private float _speedIncreaseAmount = 0.2f;
    [SerializeField] private float _speedIncreaseTime = 2.5f;

                     private const float LANE_WIDTH = 3.0f;
                     private float _speed;
                     private float _speedIncreaseLastTick;
                     private bool  _isRunning = false;
                     private bool  _isGrounded;
                     private int   _desiredLane = 1;

    private Vector3 _targetPosition;
    private Vector3 _moveVector;
    private float   _vertivalVelocity;

    private MobileInput mobileInput;
    private GameController gameController;
    private IGroundChecker _groundChecker;
    private CharacterController characterController;
    //private CollisionReactor obstaclesReactor;

    public event Action Runned = delegate { };
    public event Action<bool> Grounded = delegate { };
    public event Action Jumped = delegate { };
    public event Action<CharacterController> Slided = delegate { };
    public event Action Falled = delegate { };

    void Start()
    {
        mobileInput = MobileInput.Instance;
        gameController = GameController.Instance;

        characterController = GetComponent<CharacterController>();
        _groundChecker = GetComponent<IGroundChecker>();  
        _speed = _originalSpeed;      
    }

    void Update()
    {
        if (!_isRunning) return;
    
        IncreaseSpeed();

        _isGrounded = _groundChecker.CheckIsGrounded(characterController);
        Grounded(_isGrounded);

        if(mobileInput.SwipeLeft)
        {
            MoveLane(goingRight:false);
        }
        if(mobileInput.SwipeRight)
        {
            MoveLane(goingRight:true);
        }

        _targetPosition = transform.position.z * Vector3.forward;

        ChangeLane();

        _moveVector = Vector3.zero;

        if(_isGrounded)
        {
            _vertivalVelocity = -0.1f;
            if(mobileInput.SwipeUp)
            {
                _vertivalVelocity = _jumpForce;
                Jumped();
            }
            else if(mobileInput.SwipeDown)
            {
                Slided(characterController);
            }
        }
        else
        {
            _vertivalVelocity -= (_gravity * Time.deltaTime);
            if(mobileInput.SwipeDown)
            {
                _vertivalVelocity = -_jumpForce*2;
                Falled();
            }
        }

        EnableMovind();
    }

    public void StartRuninig()
    {
        _isRunning = true;
    }

    public void Death()
    {
        _isRunning = false;
        //gameController.IsDead = true;
        //gameController.isGameStarted = false;
        //Dead();
    }

    private void IncreaseSpeed()
    {
        if (Time.time - _speedIncreaseLastTick > _speedIncreaseTime)
        {
            _speedIncreaseLastTick = Time.time;
            _speed += _speedIncreaseAmount;           
            _gravity += 0.2f;
            _jumpForce += 0.06f;
            gameController.UpdateModifier(_speed - _originalSpeed);
        }
    }

    private void MoveLane(bool goingRight)
    {
        _desiredLane += (goingRight) ? 1 : -1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);
    }

    private void ChangeLane()
    {      
        if (_desiredLane == 0)
        {
            _targetPosition += Vector3.left * LANE_WIDTH;
        }
        else if (_desiredLane == 2)
        {
            _targetPosition += Vector3.right * LANE_WIDTH;
        }
    }

    private void EnableMovind()
    {
        _moveVector.z = _speed;
        _moveVector.y = _vertivalVelocity;
        _moveVector.x = (_targetPosition - transform.position).normalized.x * _speed;
        characterController.Move(_moveVector * Time.deltaTime);

        Vector3 dir = characterController.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, 0.05f);
        }
    }

    public void MultiplySpeed(float k)
    {
        _speed *=k;
    }
    public void DivideSpeed(float k)
    {
        _speed /= k;
    }

    private void OnDestroy()
    {
        //obstaclesReactor.Faced -= Death;
    }

}


