using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CarObstacle;

public class CarSpots : MonoBehaviour
{
    [SerializeField]
    private GameObject carGoalPrefab;

    private IEnumerable<CarObstacle> parkedCars;

    private GameObject carGoal = null;

    public CarGoal CarGoal { get; private set; }

    public void Setup()
    {
        parkedCars = GetComponentsInChildren<CarObstacle>(true)
            .Where(c => c.CarObstacleTypeValue == CarObstacleType.Car);

        int parkedCarsCount = parkedCars.Count();
        int carTohide = Random.Range(0, parkedCarsCount);
        int carCounter = 0;

        foreach (var car in parkedCars)
        {
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
