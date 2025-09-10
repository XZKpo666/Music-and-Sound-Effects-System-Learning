using UnityEngine;

public class Backgroundcontrol : MonoBehaviour
{
    private AudioManager _audioManager;
    private void Start()
    {
        _audioManager = ServiceLocator.Instance.GetService<AudioManager>();
        _audioManager.PlayBackgroundMusic("0"); // Play the first background music at the start
    }   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _audioManager.PlayBackgroundMusic("0");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _audioManager.PlayBackgroundMusic("1");
        }
    }
}
