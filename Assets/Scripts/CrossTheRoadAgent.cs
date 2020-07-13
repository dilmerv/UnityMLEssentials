using TMPro;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CrossTheRoadAgent : BaseAgent
{
    [SerializeField]
    private float speed = 50.0f;

    [SerializeField, Tooltip("This is the offset amount from the local agent position the agent will move on every step")]
    private float stepAmount = 1.0f;

    [SerializeField]
    private TextMeshProUGUI rewardValue = null;

    [SerializeField]
    private TextMeshProUGUI episodesValue = null;

    [SerializeField]
    private TextMeshProUGUI stepValue = null;

    private CrossTheRoadGoal goal = null;

    private float overallReward = 0;

    private float overallSteps = 0;

    private Vector3 moveTo = Vector3.zero;

    private Vector3 originalPosition = Vector3.zero;

    private bool moveInProgress = false;

    private int direction = 0;

    public enum MoveToDirection
    { 
        Idle,
        Left,
        Right,
        Forward
    }

    private MoveToDirection moveToDirection = MoveToDirection.Idle;

    void Awake()
    {
        goal = transform.parent.GetComponentInChildren<CrossTheRoadGoal>();
        originalPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = moveTo = originalPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observations - x, y, z
        sensor.AddObservation(transform.localPosition);

        // 3 observations - x, y, z
        sensor.AddObservation(goal.transform.localPosition);
    }

    void Update()
    {
        if (!moveInProgress)
            return;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveTo, 0.01f);

        if (Vector3.Distance(transform.localPosition, moveTo) <= 0.0001f)
        {
            moveInProgress = false;
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (moveInProgress)
            return;

        direction = Mathf.FloorToInt(vectorAction[0]);

        switch (direction)
        {
            case 0: // idle
                moveTo = transform.localPosition;
                moveToDirection = MoveToDirection.Idle;
                break;
            case 1: // left
                moveTo = new Vector3(transform.localPosition.x - stepAmount, transform.localPosition.y, transform.localPosition.z);
                moveToDirection = MoveToDirection.Left;
                moveInProgress = true;
                break;
            case 2: // right
                moveTo = new Vector3(transform.localPosition.x + stepAmount, transform.localPosition.y, transform.localPosition.z);
                moveToDirection = MoveToDirection.Right;
                moveInProgress = true;
                break;
            case 3: // forward
                moveTo = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + stepAmount);
                moveToDirection = MoveToDirection.Forward;
                moveInProgress = true;
                break;
        }
    }

    public void TakeAwayPoints()
    {
        AddReward(-0.01f);
        
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

        //move forward
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            actionsOut[0] = 3;
        }
    }
}
