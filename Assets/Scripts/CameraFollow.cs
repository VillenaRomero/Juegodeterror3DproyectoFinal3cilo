using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   
    public float sensitivity = 200f; 
    public float distance = 5f; 
    public float height = 2f;   

    private float currentYaw = 0f; 

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentYaw += mouseX;

        Vector3 offset = Quaternion.Euler(0f, currentYaw, 0f) * Vector3.back * distance;
        Vector3 targetPos = player.position + Vector3.up * height + offset;

        transform.position = targetPos;

        transform.LookAt(player.position + Vector3.up * height * 0.5f);
    }
}
