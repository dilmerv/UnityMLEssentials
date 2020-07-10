using Unity.MLAgents.Sensors;
using UnityEngine;


public class PlayerAgent : BaseAgent
{
    #region Exposed Instance Variables

    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private GameObject target = null;

    [SerializeField]
    private float distanceRequired = 1.5f;

    #endregion

    #region Private Instance Variables

    private Rigidbody playerRigidbody;

    private Vector3 originalPosition;

    private Vector3 originalTargetPosition;

    #endregion

    public override void Initialize()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.localPosition;
        originalTargetPosition = target.transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.LookAt(target.transform);
        target.transform.localPosition = originalTargetPosition;
        transform.localPosition = originalPosition;
        transform.localPosition = new Vector3(Random.Range(-4, 4), originalPosition.y, originalPosition.z);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 3 observations - x, y, z
        sensor.AddObservation(transform.localPosition);

        // 3 observations - x, y, z
        sensor.AddObservation(target.transform.localPosition);

        // 1 observation
        sensor.AddObservation(playerRigidbody.velocity.x);

        // 1 observation
        sensor.AddObservation(playerRigidbody.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var vectorForce = new Vector3();

        vectorForce.x = vectorAction[0];
        vectorForce.z = vectorAction[1];

        playerRigidbody.AddForce(vectorForce * speed);

        var distanceFromTarget = Vector3.Distance(transform.localPosition, target.transform.localPosition);


        // we are doing good
        if (distanceFromTarget < distanceRequired)
        {
            AddReward(1);
            EndEpisode();

            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
        }

        // we are not doing so good
        if (transform.localPosition.y < 0)
        {
            EndEpisode();

            // go back and punish the agent for their performance
            StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal"); // x
        actionsOut[1] = Input.GetAxis("Vertical"); // z
    }
}
