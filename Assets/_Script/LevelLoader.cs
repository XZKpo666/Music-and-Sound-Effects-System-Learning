using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour, IGameService
{
    public Animator _transitionAnimator; 
    public float _transitionTime = 1f; 
    
    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<LevelLoader>(false);
    }

    public void LoadLevel(int scencemode)
    {
        StartCoroutine(LoadLevelWithDelay(scencemode, _transitionTime));
    }

    IEnumerator LoadLevelWithDelay(int levelIndex, float delay)
    {
        if (_transitionAnimator != null)
        {
            _transitionAnimator.SetTrigger("Start"); 
        }
        else
        {
            Debug.LogWarning("Transition Animator is not assigned. Skipping animation.");
        } 

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(levelIndex);
    }

}
