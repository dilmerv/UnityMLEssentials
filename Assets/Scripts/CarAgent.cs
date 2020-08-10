using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;

public class CarAgent : BaseAgent
{
    private Vector3 originalPosition;

    private BehaviorParameters behaviorParameters;

    private CarController carController;

    private CarSpots carSpots;
    
    private CarGoal goal;

    void Awake()
    {
        originalPosition = transform.localPosition;
        behaviorParameters = GetComponent<BehaviorParameters>();
        carController = GetComponent<CarController>();
        carSpots = transform.parent.GetComponentInChildren<CarSpots>();
    }

    public override void OnEpisodeBegin()
    {
        ResetParkingLotArea();
    }

    private void ResetParkingLotArea()
    {
        // reset which cars show or not show
        carSpots.Setup();
        
        // important to set car to automonous during default behavior
        carController.IsAutonomous = behaviorParameters.BehaviorType == BehaviorType.Default;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        carController.CarRigidbody.velocity = Vector3.zero;
    }

    void Update()
    {
        if(transform.localPosition.y <= 0)
        {
            TakeAwayPoints();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observations - x, y, z
        sensor.AddObservation(transform.localPosition);

        // 3 observations - x, y, z
        sensor.AddObservation(carSpots.CarGoal.transform.localPosition);

        // 3 observations - x, y, z
        sensor.AddObservation(carController.CarRigidbody.velocity);
    }
    
    public override void OnActionReceived(float[] vectorAction)
    {
        var direction = Mathf.FloorToInt(vectorAction[0]);
        Debug.Log("Direction: " + direction);
        switch (direction)
        {
            case 0: // idle
                carController.CurrentDirection = Direction.Idle;
                break;
            case 1: // forward
                carController.CurrentDirection = Direction.MoveForward;
                break;
            case 2: // backward
                carController.CurrentDirection = Direction.MoveBackward;
                break;
            case 3: // turn left
                carController.CurrentDirection = Direction.TurnLeft;
                break;
            case 4: // turn right
                carController.CurrentDirection = Direction.TurnRight;
                break;
        }
    }

    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if(isFinal)
        {
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
        }
    }

    public void TakeAwayPoints()
    {
        AddReward(-0.001f);
        
        EndEpisode();
        StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }

        if(Input.GetKey(KeyCode.LeftArrow) && carController.canApplyTorque())
        {
            actionsOut[0] = 3;
        }

        if(Input.GetKey(KeyCode.RightArrow) && carController.canApplyTorque())
        {
            actionsOut[0] = 4;
        }
    }
}
