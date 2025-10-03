using UnityEngine;
using System.Collections;

public class Carta3D : MonoBehaviour
{
    public cartas datosCarta;
    public Material reversoMaterial;
    private MeshRenderer rend;
    private bool revelada = false;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material = reversoMaterial;
    }

    public void ConfigurarCarta(cartas carta, Material reverso)
    {
        datosCarta = carta;
        reversoMaterial = reverso;
        rend.material = reversoMaterial;
    }

    public void Revelar()
    {
        if (!revelada)
        {
            revelada = true;
            StartCoroutine(RotarCarta());
        }
    }

    private IEnumerator RotarCarta()
    {
        float tiempo = 0f;
        Quaternion inicio = transform.rotation;
        Quaternion medio = inicio * Quaternion.Euler(0, 90, 0);
        Quaternion final = inicio * Quaternion.Euler(0, 180, 0);

        // 1ª mitad
        while (tiempo < 0.2f)
        {
            transform.rotation = Quaternion.Slerp(inicio, medio, tiempo / 0.2f);
            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.rotation = medio;

        // Cambiar material
        rend.material = datosCarta.material;

        // 2ª mitad
        tiempo = 0f;
        while (tiempo < 0.2f)
        {
            transform.rotation = Quaternion.Slerp(medio, final, tiempo / 0.2f);
            tiempo += Time.deltaTime;
            yield return null;
        }
        transform.rotation = final;
    }
}
