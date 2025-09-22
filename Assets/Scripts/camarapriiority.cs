using Unity.Cinemachine;
using UnityEngine;

public class camarapriiority : MonoBehaviour
{
    public CinemachineCamera currentCamera;
    void Start()
    {
        currentCamera.Priority++;
    }
    public void UpdateCamera(CinemachineCamera target)
    {
        currentCamera.Priority--;
        currentCamera = target;
        currentCamera.Priority++;
        currentCamera.Priority++;
    }
}
