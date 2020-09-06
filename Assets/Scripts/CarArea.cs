using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarArea : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraView;

    [SerializeField]
    private GameObject carView;

    public GameObject CameraView { get => cameraView; set => cameraView = value; }
    
    public GameObject CarView { get => carView; set => carView = value; }
}
