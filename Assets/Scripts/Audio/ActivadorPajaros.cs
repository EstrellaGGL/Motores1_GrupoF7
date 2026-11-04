using UnityEngine;

public class ActivadorPajaros : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource sonidoPajaros;

    [Header("Configuración")]
    [SerializeField] private float intervaloEntreSonidos = 8f;

    private float proximaReproduccion = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < proximaReproduccion) return;

        if (sonidoPajaros == null) return;

        sonidoPajaros.Play();

        proximaReproduccion = Time.time + intervaloEntreSonidos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (sonidoPajaros != null)
        {
            sonidoPajaros.Stop();
        }

        proximaReproduccion = 0f;
    }
}
