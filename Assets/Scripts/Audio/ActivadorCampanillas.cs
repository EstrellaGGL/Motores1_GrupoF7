using UnityEngine;

public class ActivadorCampanillas : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource sonidoCampanillas;

    [Header("Configuración")]
    [SerializeField] private float intervaloEntreSonidos = 5f;

    private float proximaReproduccion = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < proximaReproduccion) return;

        if (sonidoCampanillas == null) return;

        sonidoCampanillas.Play();

        proximaReproduccion = Time.time + intervaloEntreSonidos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (sonidoCampanillas != null)
        {
            sonidoCampanillas.Stop();
        }

        proximaReproduccion = 0f;
    }
}