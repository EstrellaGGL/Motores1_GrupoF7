using UnityEngine;
using UnityEngine.Audio;

public class ControlViento : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Transform jugador;

    [Header("Altura")]
    [SerializeField] private float alturaMinima = 0f;
    [SerializeField] private float alturaMaxima = 30f;

    [Header("Viento del río")]
    [SerializeField] private float rioVolumenInicial = -10f;
    [SerializeField] private float rioVolumenFinal = -40f;

    [Header("Viento de altura")]
    [SerializeField] private float alturaVolumenInicial = -40f;
    [SerializeField] private float alturaVolumenFinal = -10f;

    [Header("Parámetros del Mixer")]
    [SerializeField] private string parametroRio = "VolumenVientoRio";
    [SerializeField] private string parametroAltura = "VolumenVientoAltura";

    [Header("Suavizado")]
    [SerializeField] private float velocidadSuavizado = 2f;

    private float volumenRioActual;
    private float volumenAlturaActual;

    void Start()
    {
        volumenRioActual = rioVolumenInicial;
        volumenAlturaActual = alturaVolumenInicial;
    }

    void Update()
    {
        float progreso = Mathf.InverseLerp(
            alturaMinima,
            alturaMaxima,
            jugador.position.y
        );

        float rioObjetivo = Mathf.Lerp(
            rioVolumenInicial,
            rioVolumenFinal,
            progreso
        );

        float alturaObjetivo = Mathf.Lerp(
            alturaVolumenInicial,
            alturaVolumenFinal,
            progreso
        );

        volumenRioActual = Mathf.Lerp(
            volumenRioActual,
            rioObjetivo,
            velocidadSuavizado * Time.deltaTime
        );

        volumenAlturaActual = Mathf.Lerp(
            volumenAlturaActual,
            alturaObjetivo,
            velocidadSuavizado * Time.deltaTime
        );

        mixer.SetFloat(parametroRio, volumenRioActual);
        mixer.SetFloat(parametroAltura, volumenAlturaActual);
    }
}