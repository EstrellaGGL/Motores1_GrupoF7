using UnityEngine;
using System.Collections;

public class CameraIntroController : MonoBehaviour
{
    public GameObject sequencerCamera;
    public GameObject thirdPersonCamera;

    public float duracionSecuencia = 8f;

    void Start()
    {
        sequencerCamera.SetActive(true);
        thirdPersonCamera.SetActive(false);

        StartCoroutine(FinalizarSecuencia());
    }

    IEnumerator FinalizarSecuencia()
    {
        yield return new WaitForSeconds(duracionSecuencia);

        thirdPersonCamera.SetActive(true);
        sequencerCamera.SetActive(false);
    }
}