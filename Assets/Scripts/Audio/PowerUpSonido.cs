using UnityEngine;

public class PowerUpSonido : MonoBehaviour
{
    public AudioClip sonidoPowerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(
                sonidoPowerUp,
                other.transform.position
            );

            Destroy(gameObject);
        }
    }
}
