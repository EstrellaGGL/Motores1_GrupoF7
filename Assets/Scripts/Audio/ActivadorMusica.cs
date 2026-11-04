using UnityEngine;

public class ActivadorMusica : MonoBehaviour
{
    public enum EstadoMusical
    {
        TramoFinal,
        Cima
    }

    [SerializeField] private EstadoMusical estado;
    [SerializeField] private ControlMusicaAdaptativa controlMusica;

    private void OnTriggerEnter(Collider otro)
    {
        if (!otro.CompareTag("Player"))
            return;

        if (estado == EstadoMusical.TramoFinal)
        {
            controlMusica.ActivarTramoFinal();
        }
        else if (estado == EstadoMusical.Cima)
        {
            controlMusica.ActivarCima();
        }
    }
}