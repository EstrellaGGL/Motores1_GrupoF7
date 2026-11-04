using UnityEngine;

public class SonidoFogata : MonoBehaviour
{
    private AudioSource sonidoFogata;

    private void Start()
    {
        sonidoFogata = GetComponent<AudioSource>();
        sonidoFogata.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!sonidoFogata.isPlaying)
            {
                sonidoFogata.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sonidoFogata.Stop();
        }
    }
}
