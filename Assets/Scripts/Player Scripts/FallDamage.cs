using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FallDamage : MonoBehaviour
{
    [Header("Barra de vida")]
    public Image fondoBarra;
    public Image barraVida;

    [Header("Imagenes de estado")]
    public Image vidaCompleta;
    public Image vidaUnaCaida;
    public Image vidaDosCaidas;

    [Header("Configuracion de caida")]
    public int caidasRestantes = 3;
    public float alturaCaidaMortal = 20f;
    public float caidaMinima = 2.5f;

    private CharacterController controller;

    private float ultimaAlturaEnSuelo;
    private float alturaInicioCaida;

    private bool cayendo = false;
    private bool sistemaActivado = false;
    private bool finalizado = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        ultimaAlturaEnSuelo = transform.position.y;

        // Ocultar toda la interfaz al comenzar
        fondoBarra.enabled = false;
        barraVida.enabled = false;

        vidaCompleta.enabled = false;
        vidaUnaCaida.enabled = false;
        vidaDosCaidas.enabled = false;

        barraVida.fillAmount = 1f;
    }

    void Update()
    {
        if (!sistemaActivado || finalizado)
        {
            return;
        }

        if (controller.isGrounded)
        {
            if (cayendo)
            {
                float distanciaCaida = alturaInicioCaida - transform.position.y;

                ProcesarCaida(distanciaCaida);

                cayendo = false;
            }

            ultimaAlturaEnSuelo = transform.position.y;
        }
        else
        {
            if (!cayendo)
            {
                alturaInicioCaida = ultimaAlturaEnSuelo;
                cayendo = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg") && !sistemaActivado)
        {
            sistemaActivado = true;

            fondoBarra.enabled = true;
            barraVida.enabled = true;

            ActualizarInterfaz();
        }
    }

    void ProcesarCaida(float distancia)
    {
        // Caida mortal
        if (distancia > alturaCaidaMortal)
        {
            Perder();
            return;
        }

        // Caida normal
        if (distancia > caidaMinima)
        {
            caidasRestantes--;

            barraVida.fillAmount = caidasRestantes / 3f;

            ActualizarInterfaz();

            if (caidasRestantes <= 0)
            {
                Perder();
            }
        }
    }

    void ActualizarInterfaz()
    {
        vidaCompleta.enabled = false;
        vidaUnaCaida.enabled = false;
        vidaDosCaidas.enabled = false;

        if (caidasRestantes == 3)
        {
            vidaCompleta.enabled = true;
        }
        else if (caidasRestantes == 2)
        {
            vidaUnaCaida.enabled = true;
        }
        else if (caidasRestantes == 1)
        {
            vidaDosCaidas.enabled = true;
        }
    }

    void Perder()
    {
        finalizado = true;

        SceneManager.LoadScene("Lose");
    }
}