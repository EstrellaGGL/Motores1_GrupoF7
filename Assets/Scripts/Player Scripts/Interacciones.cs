using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Interacciones : MonoBehaviour
{
    public TextMeshProUGUI textoInteraccion;
    public RawImage imagenInteraccion;

    private bool puedeInteractuar = false;
    private InteraccionObject objetoActual;

    private int indicePanel = 0;

    private void Start()
    {
        CerrarPaneles();
    }

    public void OnInteract()
    {
        if (puedeInteractuar && objetoActual != null)
        {
            if (objetoActual.paneles == null || objetoActual.paneles.Length == 0)
            {
                Debug.LogWarning("El objeto interactuable no tiene paneles asignados.");
                return;
            }

            if (indicePanel < 0 || indicePanel >= objetoActual.paneles.Length)
            {
                Debug.LogWarning("El índice del panel está fuera de rango.");
                return;
            }

            PanelInformacion informacion = objetoActual.paneles[indicePanel];

            if (informacion == null)
            {
                Debug.LogWarning("La información del panel no está asignada.");
                return;
            }

            if (informacion.panel == null)
            {
                Debug.LogWarning("Falta asignar el GameObject del panel.");
                return;
            }

            CerrarPaneles();

            informacion.panel.SetActive(true);

            if (textoInteraccion != null)
            {
                textoInteraccion.text = informacion.texto;
            }

            if (imagenInteraccion != null)
            {
                imagenInteraccion.texture = informacion.imagen;
            }

            // Reproducir el audio del objeto.
            objetoActual.ReproducirSonido();
        }
    }

    private void CerrarPaneles()
    {
        if (objetoActual != null && objetoActual.paneles != null)
        {
            foreach (PanelInformacion informacion in objetoActual.paneles)
            {
                if (informacion != null && informacion.panel != null)
                {
                    informacion.panel.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            objetoActual = other.GetComponent<InteraccionObject>();

            if (objetoActual != null)
            {
                puedeInteractuar = true;
                indicePanel = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            CerrarPaneles();

            puedeInteractuar = false;
            objetoActual = null;
        }
    }
}