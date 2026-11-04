using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;

    private bool estaPausado = false;

    private void Start()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (estaPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    private void Pausar()
    {
        estaPausado = true;
        Time.timeScale = 0f;
        panelPausa.SetActive(true);
    }

    private void Reanudar()
    {
        estaPausado = false;
        Time.timeScale = 1f;
        panelPausa.SetActive(false);
    }
}
