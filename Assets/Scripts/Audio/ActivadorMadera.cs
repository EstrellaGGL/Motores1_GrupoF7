using UnityEngine;

public class ActivadorMadera : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource sonidoMadera;

    [Header("Configuración")]
    [SerializeField] private float intervaloEntreSonidos = 5f;

    private float proximaReproduccion = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < proximaReproduccion) return;

        if (sonidoMadera == null) return;

        sonidoMadera.Play();

        proximaReproduccion = Time.time + intervaloEntreSonidos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (sonidoMadera != null)
        {
            sonidoMadera.Stop();
        }

        proximaReproduccion = 0f;
    }
}