using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentAvoidance : BaseAgent
{
    [SerializeField]
    private Vector3 idlePosition = Vector3.zero;

    [SerializeField]
    private Vector3 leftPosition = Vector3.zero;

    [SerializeField]
    private Vector3 rightPosition = Vector3.zero;

    private TargetMoving targetMoving;

    void Awake() 
    {
        targetMoving = transform.parent.GetComponentInChildren<TargetMoving>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = idlePosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(targetMoving.transform.localPosition);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        int direction = (int)vectorAction[0];

        switch(direction)
        {
            case 0:
                transform.localPosition = idlePosition;
                break;
            case -1:
                transform.localPosition = leftPosition;
                break;
            case 1:
                transform.localPosition = rightPosition;
                break;
        }
    }

    public void TakeAwayPoints()
    {
        AddReward(-0.01f);
        targetMoving.ResetTarget();
        EndEpisode();
        StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
    }

    public void GivePoints()
    {
        AddReward(1.0f);
        targetMoving.ResetTarget();
        EndEpisode();
        StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
    }

    public override void Heuristic(float[] actionsOut)
    {
        //idle
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            actionsOut[0] = 0;
        }

        //move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            actionsOut[0] = -1;
        }

        //move right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            actionsOut[0] = 1;
        }
    }
}
