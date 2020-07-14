using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CrossTheRoadCar : MonoBehaviour
{
    [SerializeField]
    private CarDirection carDirection = CarDirection.North;

    [SerializeField]
    [Range(0.5f, 25.0f)]
    private float minSpeed = 5.0f;

    [SerializeField]
    [Range(5.0f, 150.0f)]
    private float maxSpeed = 150.0f;

    private float speed = 0;

    [SerializeField]
    private float maxDistance = -3.5f;

    private CrossTheRoadAgent agent = null;

    private Vector3 originalPosition;

    public enum CarDirection
    { 
        South,
        North
    }

    void Awake()
    {
        // cache agent
        agent = transform.parent.GetComponentInChildren<CrossTheRoadAgent>();

        // cache position
        originalPosition = transform.localPosition;

        // randomize car speed
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // if we are beyond the max distance restart the position
        if (transform.localPosition.x <= maxDistance && carDirection == CarDirection.North || 
            transform.localPosition.x >= maxDistance && carDirection == CarDirection.South)
        {
            transform.localPosition = originalPosition;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            // move towards max distance
            transform.localPosition = new Vector3(GetNewCarXPosition(), transform.localPosition.y,
                transform.localPosition.z);
        }
    }

    private float GetNewCarXPosition()
    {
        return carDirection == CarDirection.North ? transform.localPosition.x - (Time.deltaTime * speed) :
            transform.localPosition.x + (Time.deltaTime * speed);
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
            Debug.Log("Points taken away car ran over agent");
            agent.TakeAwayPoints();
        }
    }
}
