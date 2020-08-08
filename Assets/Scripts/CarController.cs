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

    [SerializeField]
    private Direction currentDirection = Direction.Idle;

    private Rigidbody carRigidbody;

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
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        if(carRigidbody.velocity.magnitude <= minSpeedBeforeIdle)
        {
            ApplyAnimatorState(Direction.Idle);
        }    
    }
    
    void FixedUpdate()
    {

        if(Input.GetKey(KeyCode.UpArrow))
        {
            ApplyAnimatorState(Direction.MoveForward);
            carRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            ApplyAnimatorState(Direction.MoveBackward);
            carRigidbody.AddForce(-transform.forward * speed, ForceMode.Acceleration);
        }

        if(Input.GetKey(KeyCode.LeftArrow) && canApplyTorque())
        {
            ApplyAnimatorState(Direction.TurnLeft);
            carRigidbody.AddTorque(transform.up * -torque);
        }

        if(Input.GetKey(KeyCode.RightArrow) && canApplyTorque())
        {
            ApplyAnimatorState(Direction.TurnRight);
            carRigidbody.AddTorque(transform.up * torque);
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

    bool canApplyTorque()
    {
        Vector3 velocity = carRigidbody.velocity;
        return Mathf.Abs(velocity.x) >= minSpeedBeforeTorque || Mathf.Abs(velocity.z) >= minSpeedBeforeTorque;
    }
}
