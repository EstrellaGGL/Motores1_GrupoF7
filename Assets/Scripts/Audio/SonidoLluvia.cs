using UnityEngine;

public class SonidoLluvia : MonoBehaviour
{
    private AudioSource sonidoLluvia;

    private void Start()
    {
        sonidoLluvia = GetComponent<AudioSource>();
        sonidoLluvia.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!sonidoLluvia.isPlaying)
            {
                sonidoLluvia.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sonidoLluvia.Stop();
        }
    }
}