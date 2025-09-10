using UnityEngine;

public class WinACoin : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            // Play the coin sound effect
            AudioManager audioManager = ServiceLocator.Instance.GetService<AudioManager>();
            if (audioManager == null)
            {
                Debug.Log("AudioManager not found in ServiceLocator.");
                return;
            }
            else audioManager.PlaySoundEffects("0"); // Assuming 0 is the ID for the coin sound
            Destroy(collision.gameObject);
        }
    }
}
