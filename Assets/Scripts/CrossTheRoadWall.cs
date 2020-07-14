using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CrossTheRoadWall : MonoBehaviour
{
    private CrossTheRoadAgent agent = null;

    void Awake()
    {
        // cache agent
        agent = transform.parent.GetComponentInChildren<CrossTheRoadAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            Debug.Log("Collision with wall");
            agent.TakeAwayPoints();
        }
    }
}
