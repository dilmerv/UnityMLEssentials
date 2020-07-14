using UnityEngine;

public class CrossTheRoadObstacle : MonoBehaviour
{
    public enum ObstacleType
    { 
        Wall,
        Tree
    }

    [SerializeField]
    private ObstacleType obstacleType = ObstacleType.Wall;

    private CrossTheRoadAgent agent = null;

    void Awake()
    {
        // cache agent
        agent = transform.parent.parent.GetComponentInChildren<CrossTheRoadAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            Debug.Log($"Collision with {obstacleType}");
            agent.TakeAwayPoints();
        }
    }
}
