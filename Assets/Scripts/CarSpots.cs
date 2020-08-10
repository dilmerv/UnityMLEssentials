using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CarObstacle;

public class CachedCar
{
    public Vector3 Position { get; set; }

    public Quaternion Rotation { get; set; }
}

public class CarSpots : MonoBehaviour
{

    [SerializeField]
    private GameObject carGoalPrefab = null;

    private IEnumerable<CarObstacle> parkedCars;

    private Dictionary<int, CachedCar> cachedParkedCars = new Dictionary<int, CachedCar>();

    private GameObject carGoal = null;

    public CarGoal CarGoal { get; private set; }    

    public void Awake()
    {
        parkedCars = GetComponentsInChildren<CarObstacle>(true)
            .Where(c => c.CarObstacleTypeValue == CarObstacleType.Car);

        // cache all car positions and rotations
        foreach(CarObstacle obstacle in parkedCars)
        {
            cachedParkedCars.Add(obstacle.GetInstanceID(), 
            new CachedCar {
                Position = obstacle.transform.position,
                Rotation = obstacle.transform.rotation
            });
        }
    }

    public void Setup()
    {
        int parkedCarsCount = parkedCars.Count();
        int carTohide = Random.Range(0, parkedCarsCount);
        int carCounter = 0;

        foreach (var car in parkedCars)
        {
            // restored cached location
            var cachedParkedCar = cachedParkedCars[car.GetInstanceID()];
            car.GetComponent<Rigidbody>().velocity = Vector3.zero;
            car.transform.SetPositionAndRotation(cachedParkedCar.Position, cachedParkedCar.Rotation);

            if(carCounter == carTohide)
            {
                car.gameObject.SetActive(false);

                if(carGoal != null)
                {
                    Destroy(carGoal);
                }

                carGoal = Instantiate(carGoalPrefab, Vector3.zero, Quaternion.identity);
                CarGoal = carGoal.GetComponent<CarGoal>();
                CarGoal.HasCarUsedIt = false;
                carGoal.transform.parent = transform.parent;
                carGoal.transform.position = car.gameObject.transform.position;
            }    
            else 
            {
                car.gameObject.SetActive(true);
            }

            carCounter++;
        }
    }
}
