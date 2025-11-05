using UnityEngine;
using UnityEngine.SceneManagement; // 1. Necessary for loading scenes
using System.Collections; // 2. Necessary for using Coroutines (the timer)

public class SceneLoader : MonoBehaviour
{
    // Make sure this name exactly matches your main game scene file!
    public string nextSceneName = "SampleScene";

    // Set how long the intro image should be visible (in seconds)
    public float delayTime = 3f;

    void Start()
    {
        // Start the timer when the scene begins
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    // This function will pause execution for the specified time
    IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait for the duration set in 'delayTime'
        yield return new WaitForSeconds(delayTime);

        // Load the next scene by the name specified above
        SceneManager.LoadScene(nextSceneName);
    }
}