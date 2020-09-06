using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraComponent;

    [SerializeField]
    private CarArea[] carAreas;

    private int selectedIndex = 0;
    
    void Update()
    {
        // oculus quest 'B' button
        if(OVRInput.GetUp(OVRInput.Button.Two))
        {
            MoveCamera();
        }
        
        // oculus quest 'A' button
        if(OVRInput.GetUp(OVRInput.Button.One))
        {
            MoveCamera(false);
        }
    }

    private void MoveCamera(bool cameraView = true)
    {
        if (selectedIndex == carAreas.Length) selectedIndex = 0;
        cameraComponent.transform.parent = cameraView ? carAreas[selectedIndex].CameraView.transform : carAreas[selectedIndex].CarView.transform;
        cameraComponent.transform.localPosition = Vector2.zero;
        cameraComponent.transform.localRotation = Quaternion.identity;
        selectedIndex++;
    }
}
