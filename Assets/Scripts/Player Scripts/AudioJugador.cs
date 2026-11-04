using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AudioJugador : MonoBehaviour
{
    [Header("Sonidos de salto")]
    [SerializeField] private AudioSource fuenteSalto;
    [SerializeField] private AudioClip[] sonidosSalto;

    [Header("Sonidos de aterrizaje")]
    [SerializeField] private AudioSource fuenteAterrizaje;
    [SerializeField] private AudioClip[] sonidosAterrizaje;

    private CharacterController controller;
    private bool estabaEnSuelo;
    private int ultimoSalto = -1;
    private int ultimoAterrizaje = -1;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        estabaEnSuelo = controller.isGrounded;
    }

    private void Update()
    {
        bool estaEnSuelo = controller.isGrounded;

        // Detectar aterrizaje
        if (estaEnSuelo && !estabaEnSuelo)
        {
            ReproducirAterrizaje();
        }

        estabaEnSuelo = estaEnSuelo;
    }

    public void ReproducirSalto()
    {
        ReproducirVariacion(
            fuenteSalto,
            sonidosSalto,
            ref ultimoSalto
        );
    }

    private void ReproducirAterrizaje()
    {
        ReproducirVariacion(
            fuenteAterrizaje,
            sonidosAterrizaje,
            ref ultimoAterrizaje
        );
    }

    private void ReproducirVariacion(
        AudioSource fuente,
        AudioClip[] clips,
        ref int ultimo
    )
    {
        if (fuente == null || clips == null || clips.Length == 0)
            return;

        int indice = Random.Range(0, clips.Length);

        if (clips.Length > 1)
        {
            while (indice == ultimo)
                indice = Random.Range(0, clips.Length);
        }

        ultimo = indice;

        if (clips[indice] != null)
            fuente.PlayOneShot(clips[indice]);
    }
}