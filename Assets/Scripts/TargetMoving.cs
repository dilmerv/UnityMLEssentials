using DilmerGames.Core;
using UnityEngine;

public class TargetMoving : MonoBehaviour
{
    [SerializeField]
    private AgentAvoidance agent = null;

    [SerializeField]
    private float targetSpeed = 0.5f;

    [SerializeField]
    private float maxDistance = 10.0f;

    private Vector3 originalPosition;

    void Awake()
    {
        originalPosition = transform.localPosition;    
    }

    void Update()
    {
        // if we are beyond the max distance restart the position
        if (transform.localPosition.z <= (originalPosition.z - maxDistance))
        {
            transform.localPosition = originalPosition;
        }
        else
        {
            // move towards max distance
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
                transform.localPosition.z - (Time.deltaTime * targetSpeed));
        }
    }

    public void ResetTarget()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.ToLower() == "player")
        {
            Debug.Log("Points taken away");
            agent.TakeAwayPoints();
        }
        else if (collision.transform.tag.ToLower() == "wall")
        {
            Debug.Log("Points gained");
            agent.GivePoints();
        }
    }
}
