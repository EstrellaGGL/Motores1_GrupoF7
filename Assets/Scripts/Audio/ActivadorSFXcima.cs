using UnityEngine;

public class ActivadorSFXCima : MonoBehaviour
{
    [SerializeField] private ReproductorSFX sonidoCima;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;
            sonidoCima.Reproducir();
        }
    }
}
