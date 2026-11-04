using UnityEngine;

public class InfoPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject[] panelesInfo;

    private void Start()
    {
        // Ocultamos todos los carteles al iniciar.
        foreach (GameObject panel in panelesInfo)
        {
            panel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos que quien entró sea el Player.
        if (other.CompareTag("Player"))
        {
            foreach (GameObject panel in panelesInfo)
            {
                panel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject panel in panelesInfo)
            {
                panel.SetActive(false);
            }
        }
    }
}
