using UnityEngine;
public enum Palo { Corazon, Trebol, Espada, Rubi }

public class cartas : MonoBehaviour
{
    public Palo palo;
    public int valor;
    public Material material;

    public cartas(Palo palo, int valor, Material material)
    {
        this.palo = palo;
        this.valor = valor;
        this.material = material;
    }
}
