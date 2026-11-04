using UnityEngine;
using System.Collections;

public class HuevoSonido : MonoBehaviour
{
    public AudioSource fuenteAudio;
    public float intervalo = 5f;

    private bool activado = false;

    private void Awake()
    {
        fuenteAudio.playOnAwake = false;
        fuenteAudio.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg") && !activado)
        {
            activado = true;
            StartCoroutine(ReproducirSonido());
        }
    }

    private IEnumerator ReproducirSonido()
    {
        while (activado)
        {
            yield return new WaitForSeconds(intervalo);

            fuenteAudio.Play();
        }
    }
}