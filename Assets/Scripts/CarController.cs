using System.Collections;
using System.Collections.Generic;
using DilmerGames.Core;
using UnityEngine;

public class CarController : Singleton<CarController>
{
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float torque = 1.0f;

    [SerializeField]
    private float minSpeedBeforeTorque = 0.3f;

    [SerializeField]
    private float minSpeedBeforeIdle = 0.2f;

    [SerializeField]
    private Animator carAnimator;

    public Direction CurrentDirection { get; set; } = Direction.Idle;

    public bool IsAutonomous { get; set; } = false;

    public Rigidbody CarRigidbody { get; set; }

    public enum Direction
    {
        Idle,
        MoveForward,
        MoveBackward,
        TurnLeft,
        TurnRight
    }

    void Awake()
    {
        CarRigidbody = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        if(CarRigidbody.velocity.magnitude <= minSpeedBeforeIdle)
        {
            CurrentDirection = Direction.Idle;
            ApplyAnimatorState(Direction.Idle);
        }    
    }
    
    void FixedUpdate() => ApplyMovement();

    public void ApplyMovement()
    {
        if(Input.GetKey(KeyCode.UpArrow) || (CurrentDirection == Direction.MoveForward && IsAutonomous))
        {
            ApplyAnimatorState(Direction.MoveForward);
            CarRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }

        if(Input.GetKey(KeyCode.DownArrow) || (CurrentDirection == Direction.MoveBackward && IsAutonomous))
        {
            ApplyAnimatorState(Direction.MoveBackward);
            CarRigidbody.AddForce(-transform.forward * speed, ForceMode.Acceleration);
        }

        if((Input.GetKey(KeyCode.LeftArrow) && canApplyTorque()) || (CurrentDirection == Direction.TurnLeft && canApplyTorque() && IsAutonomous))
        {
            ApplyAnimatorState(Direction.TurnLeft);
            CarRigidbody.AddTorque(transform.up * -torque);
        }

        if(Input.GetKey(KeyCode.RightArrow) && canApplyTorque() || (CurrentDirection == Direction.TurnRight && canApplyTorque() && IsAutonomous))
        {
            ApplyAnimatorState(Direction.TurnRight);
            CarRigidbody.AddTorque(transform.up * torque);
        }
    }

    void ApplyAnimatorState(Direction direction)
    {   
        carAnimator.SetBool(direction.ToString(), true);

        switch(direction)
        {
            case Direction.Idle:
                carAnimator.SetBool(Direction.MoveBackward.ToString(), false);
                carAnimator.SetBool(Direction.MoveForward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
            break;
            case Direction.MoveForward:
                carAnimator.SetBool(Direction.Idle.ToString(), false);
                carAnimator.SetBool(Direction.MoveBackward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
            break;
            case Direction.MoveBackward:
                carAnimator.SetBool(Direction.Idle.ToString(), false);
                carAnimator.SetBool(Direction.MoveForward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
            break;
            case Direction.TurnLeft:
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
            break;
            case Direction.TurnRight:
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
            break;
        }
    }

    public bool canApplyTorque()
    {
        Vector3 velocity = CarRigidbody.velocity;
        return Mathf.Abs(velocity.x) >= minSpeedBeforeTorque || Mathf.Abs(velocity.z) >= minSpeedBeforeTorque;
    }
}
