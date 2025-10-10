using UnityEngine;

public class SurpriseBox : MonoBehaviour
{
    public Transform tableCenter;
    public float rotationSpeed = 50f;
    public bool isSpinning = true;

    [Header("Manija")]
    public int handleValue = 0;
    public int handleLimit = 20;
    public bool isOpen = false;

    void Update()
    {
        if (isSpinning && !isOpen && tableCenter != null)
        {
            transform.RotateAround(tableCenter.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (handleValue >= handleLimit && !isOpen)
        {
            OpenBox();
        }
    }

    public void ModifyHandle(int amount)
    {
        handleValue += amount;
        if (handleValue < 0) handleValue = 0;
        if (handleValue >= handleLimit) OpenBox();
    }

    void OpenBox()
    {
        isOpen = true;
        isSpinning = false;
    }
}
