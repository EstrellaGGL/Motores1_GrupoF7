using UnityEngine;

public class ActivadorSFXPlataforma : MonoBehaviour
{
    [SerializeField] private ReproductorSFX sonidoPlataforma;
    [SerializeField] private float tiempoEnfriamiento = 3f;

    private float proximaActivacion = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < proximaActivacion) return;

        if (sonidoPlataforma == null) return;

        sonidoPlataforma.Reproducir();

        proximaActivacion = Time.time + tiempoEnfriamiento;
    }
}
