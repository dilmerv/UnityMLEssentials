using UnityEngine;

public class CrossTheRoadGoal : MonoBehaviour
{
    [SerializeField]
    private CrossTheRoadAgent agent = null;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            Debug.Log("Points earned as road was crossed");
            agent.GivePoints();
        }
    }
}
