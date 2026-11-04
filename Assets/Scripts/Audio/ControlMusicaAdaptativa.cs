
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class ControlMusicaAdaptativa : MonoBehaviour
{
    [Header("Canciones")]
    [SerializeField] private AudioSource comienzo;
    [SerializeField] private AudioSource tramoFinal;
    [SerializeField] private AudioSource cima;

    [Header("Snapshots")]
    [SerializeField] private AudioMixerSnapshot snapshotComienzo;
    [SerializeField] private AudioMixerSnapshot snapshotTramoFinal;
    [SerializeField] private AudioMixerSnapshot snapshotCima;

    [Header("Transiciones")]
    [SerializeField] private float duracionTransicion = 2f;

    private AudioSource musicaActual;
    private Coroutine transicionActual;

    void Start()
    {
        ReproducirEstado(comienzo, snapshotComienzo);
    }

    public void ActivarTramoFinal()
    {
        ReproducirEstado(tramoFinal, snapshotTramoFinal);
    }

    public void ActivarCima()
    {
        ReproducirEstado(cima, snapshotCima);
    }

    private void ReproducirEstado(
        AudioSource nuevaMusica,
        AudioMixerSnapshot snapshot)
    {
        snapshot.TransitionTo(duracionTransicion);

        if (transicionActual != null)
            StopCoroutine(transicionActual);

        transicionActual = StartCoroutine(
            CambiarMusica(nuevaMusica)
        );
    }

    private IEnumerator CambiarMusica(AudioSource nuevaMusica)
    {
        if (!nuevaMusica.isPlaying)
        {
            nuevaMusica.volume = 0f;
            nuevaMusica.Play();
        }

        float tiempo = 0f;
        AudioSource anterior = musicaActual;

        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracionTransicion;

            nuevaMusica.volume = progreso;

            if (anterior != null)
                anterior.volume = 1f - progreso;

            yield return null;
        }

        if (anterior != null && anterior != nuevaMusica)
            anterior.Stop();

        nuevaMusica.volume = 1f;
        musicaActual = nuevaMusica;
        transicionActual = null;
    }
    
}
