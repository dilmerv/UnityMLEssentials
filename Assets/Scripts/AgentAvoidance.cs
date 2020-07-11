using TMPro;
using Unity.MLAgents;
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

    [SerializeField]
    private TextMeshProUGUI rewardValue = null;

    [SerializeField]
    private TextMeshProUGUI episodesValue = null;

    [SerializeField]
    private TextMeshProUGUI stepValue = null;

    private TargetMoving targetMoving = null;

    private float overallReward = 0;

    private float overallSteps = 0;

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
        int direction = Mathf.FloorToInt(vectorAction[0]);
        switch(direction)
        {
            case 0:
                transform.localPosition = idlePosition;
                break;
            case 1:
                transform.localPosition = leftPosition;
                break;
            case 2:
                transform.localPosition = rightPosition;
                break;
        }
    }

    public void TakeAwayPoints()
    {
        AddReward(-0.01f);
        targetMoving.ResetTarget();
        
        UpdateStats();

        EndEpisode();
        StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
    }

    private void UpdateStats()
    {
        overallReward += this.GetCumulativeReward();
        overallSteps += this.StepCount;
        rewardValue.text = $"{overallReward.ToString("F2")}";
        episodesValue.text = $"{this.CompletedEpisodes}";
        stepValue.text = $"{overallSteps}";
    }

    public void GivePoints()
    {
        AddReward(1.0f);
        targetMoving.ResetTarget();
        
        UpdateStats();

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
            actionsOut[0] = 1;
        }

        //move right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            actionsOut[0] = 2;
        }
    }
}
