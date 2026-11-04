using UnityEngine;

public class ActivadorSFXAltar : MonoBehaviour
{
    [SerializeField] private ReproductorSFX sonidoAltar;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;
            sonidoAltar.Reproducir();
        }
    }
}