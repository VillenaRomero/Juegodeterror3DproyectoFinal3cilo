using UnityEngine;
public class lookobject : MonoBehaviour
{
    public Transform object3D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake() {
        transform.LookAt(object3D);
    }
}
