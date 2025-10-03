using UnityEngine;

public class CartaSeleccion : MonoBehaviour
{
    private SistemaCombate3D combate;
    private int index;

    public void Configurar(SistemaCombate3D sc, int idx)
    {
        combate = sc;
        index = idx;
    }

    void OnMouseDown()
    {
        combate.AlSeleccionarCarta(index);
        Destroy(this); 
    }
}
