using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{

    public Animator _transitionAnimator; 
    public float _transitionTime = 1f; 
    private int _levelIndex;

    public void LoadLevel()
    {
        _levelIndex = SceneManager.GetActiveScene().buildIndex; 
        if (_levelIndex == 1)
        {
            _levelIndex = 0;
        }    
        _levelIndex++;
        StartCoroutine(LoadLevelWithDelay(_levelIndex, _transitionTime));
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
