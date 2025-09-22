using UnityEngine;

public class CameraFirstPerson : MonoBehaviour
{
    public float sensitivity = 200f;
    private float yaw = 0f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
       //Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        yaw += mouseX;

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
