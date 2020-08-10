using UnityEngine;

public class CarGoal : MonoBehaviour
{
    private CarAgent agent = null;

    [SerializeField]
    private GoalType goalType = GoalType.Milestone;

    [SerializeField]
    private float goalReward = 0.1f;

    // to avoid AI from cheating ;)
    public bool HasCarUsedIt { get; set; } = false;

    public enum GoalType
    {
        Milestone,
        FinalDestination
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player" && !HasCarUsedIt)
        {
            agent = transform.parent.GetComponentInChildren<CarAgent>();

            HasCarUsedIt = true;
            if(goalType == GoalType.Milestone)
            {
                StartCoroutine(agent.GivePoints(goalReward));
            }
            else
            {
                StartCoroutine(agent.GivePoints(goalReward, true, 0.5f));
            }
        }
    }
}