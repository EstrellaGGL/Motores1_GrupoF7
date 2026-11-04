using UnityEngine;

public class ActivadorVientoEstructura : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource sonidoVientoEstructura;

    [Header("Configuración")]
    [SerializeField] private float intervaloEntreSonidos = 5f;

    private float proximaReproduccion = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < proximaReproduccion) return;

        if (sonidoVientoEstructura == null) return;

        sonidoVientoEstructura.Play();

        proximaReproduccion = Time.time + intervaloEntreSonidos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (sonidoVientoEstructura != null)
        {
            sonidoVientoEstructura.Stop();
        }

        proximaReproduccion = 0f;
    }
}