using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TargetMoving : MonoBehaviour
{
    [SerializeField]
    private AgentAvoidance agent = null;

    [SerializeField]
    [Range(0.5f, 25.0f)]
    private float minSpeed = 5.0f;

    [SerializeField]
    [Range(5.0f, 150.0f)]
    private float maxSpeed = 150.0f;

    private float speed = 0;

    [SerializeField]
    private float maxDistance = -3.5f;

    private Vector3 originalPosition;

    void Awake()
    {
        originalPosition = transform.localPosition;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // if we are beyond the max distance restart the position
        if (transform.localPosition.z <= maxDistance)
        {
            transform.localPosition = originalPosition;
        }
        else
        {
            // move towards max distance
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
                transform.localPosition.z - (Time.deltaTime * speed));
        }
    }

    public void ResetTarget()
    {
        speed = Random.Range(minSpeed, maxSpeed);
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
