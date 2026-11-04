using UnityEngine;
using System.Collections;

public class ReproductorSFX : MonoBehaviour
{
    [SerializeField] private AudioSource sonido;
    [SerializeField] private AudioClip[] variaciones;

    private int ultimoIndice = -1;

    public void Reproducir()
    {
        if (sonido == null || variaciones == null || variaciones.Length == 0)
        {
            Debug.LogWarning("Falta asignar el AudioSource o los clips.");
            return;
        }

        int indice = Random.Range(0, variaciones.Length);

        ultimoIndice = indice;

        sonido.PlayOneShot(variaciones[indice]);

        Debug.Log("INICIO: " + variaciones[indice].length + " segundos");

        StartCoroutine(ComprobarSonido(variaciones[indice].length));
    }

    private IEnumerator ComprobarSonido(float duracion)
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("A los 0.2 segundos - AudioSource activo: " + sonido.isActiveAndEnabled);
        Debug.Log("A los 0.2 segundos - AudioSource reproduciendo: " + sonido.isPlaying);

        yield return new WaitForSeconds(duracion);

        Debug.Log("Finalizó la comprobación.");
    }
}