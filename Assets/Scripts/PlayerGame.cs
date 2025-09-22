using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal"); // A / D
        float moveZ = Input.GetAxis("Vertical");   // W / S

        Vector3 move = new Vector3(moveX, 0f, moveZ) * speed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
}
