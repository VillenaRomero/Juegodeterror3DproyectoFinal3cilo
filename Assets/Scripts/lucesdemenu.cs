using UnityEngine;
using UnityEngine.Rendering.Universal;

public class lucesdemenu : MonoBehaviour
{

    public Light light3D;
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 0.1f;
    public float offChance = 0.1f;

    private float timer;

    void Start()
    {
        if (light3D == null)
            light3D = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flickerSpeed)
        {
            if (Random.value < offChance)
            {
                light3D.intensity = 0f;
            }
            else
            {
                light3D.intensity = Random.Range(minIntensity, maxIntensity);
            }

            timer = 0f;
        }
    }
}
