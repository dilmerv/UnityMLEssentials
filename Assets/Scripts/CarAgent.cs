using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;

public class CarAgent : BaseAgent
{
    [SerializeField]
    private GameObject goal;

    private Vector3 originalPosition;
    
    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        CarController.Instance.CarRigidbody.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observations - x, y, z
        sensor.AddObservation(transform.localPosition);

        // 3 observations - x, y, z
        sensor.AddObservation(goal.transform.localPosition);
    }
    
    public override void OnActionReceived(float[] vectorAction)
    {
        var direction = Mathf.FloorToInt(vectorAction[0]);

        switch (direction)
        {
            case 0: // idle
                CarController.Instance.CurrentDirection = Direction.Idle;
                break;
            case 1: // forward
                CarController.Instance.CurrentDirection = Direction.MoveForward;
                break;
            case 2: // backward
                CarController.Instance.CurrentDirection = Direction.MoveBackward;
                break;
            case 3: // turn left
                CarController.Instance.CurrentDirection = Direction.TurnLeft;
                break;
            case 4: // turn right
                CarController.Instance.CurrentDirection = Direction.TurnRight;
                break;
        }
    }

    public void GivePoints()
    {
        AddReward(1.0f);
        EndEpisode();
        StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
    }

    public void TakeAwayPoints()
    {
        AddReward(-0.025f);
        EndEpisode();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        CarController.Instance.CurrentDirection = Direction.Idle;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }

        if(Input.GetKey(KeyCode.LeftArrow) && CarController.Instance.canApplyTorque())
        {
            actionsOut[0] = 3;
        }

        if(Input.GetKey(KeyCode.RightArrow) && CarController.Instance.canApplyTorque())
        {
            actionsOut[0] = 4;
        }
    }
}
