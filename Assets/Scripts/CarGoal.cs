using UnityEngine;

public class CarGoal : MonoBehaviour
{
    private CarAgent agent = null;

    [SerializeField]
    private GoalType goalType = GoalType.Milestone;

    [SerializeField]
    private float goalReward = 0.1f;

    public enum GoalType
    {
        Milestone,
        FinalDestination
    }

    void Awake()
    {
        // cache agent
        agent = transform.parent.GetComponentInChildren<CarAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            if(goalType == GoalType.Milestone)
                agent.GivePoints(goalReward);
            else
                agent.GivePoints(goalReward, true);
        }
    }
}