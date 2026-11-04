using UnityEngine;

public class ActivadorRocas : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource sonidoRocas;

    [Header("Configuración")]
    [SerializeField] private float intervaloEntreSonidos = 5f;

    private float proximaReproduccion = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < proximaReproduccion) return;

        if (sonidoRocas == null) return;

        sonidoRocas.Play();

        proximaReproduccion = Time.time + intervaloEntreSonidos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (sonidoRocas != null)
        {
            sonidoRocas.Stop();
        }

        proximaReproduccion = 0f;
    }
}
