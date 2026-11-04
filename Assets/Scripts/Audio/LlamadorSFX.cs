using UnityEngine;

public class LlamadorSFX : MonoBehaviour
{
    [SerializeField] private ReproductorSFX reproductor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            reproductor.Reproducir();
        }
    }
}