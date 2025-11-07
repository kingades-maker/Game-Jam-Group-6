using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MinigameEntrance : MonoBehaviour
{
    // CRITICAL: This is now a GameObject to control the entire Panel + Text unit.
    // Drag your MinigamePromptPanel object into this slot in the Inspector.
    public GameObject minigamePromptPanel;

    // Ensure this matches the name of your minigame scene.
    public string minigameSceneName = "MayaMiniGame";

    private bool playerIsNear = false;

    void Update()
    {
        // Check if the player is near AND presses the 'Space' key
        if (playerIsNear && Input.GetKeyDown(KeyCode.Space))
        {
            EnterMinigame();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player (assuming Player tag is set)
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            if (minigamePromptPanel != null)
            {
                // Activate the whole panel when the player enters
                minigamePromptPanel.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player is leaving the trigger
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            if (minigamePromptPanel != null)
            {
                // Deactivate the whole panel when the player leaves
                minigamePromptPanel.SetActive(false);
            }
        }
    }

    void EnterMinigame()
    {
        // Load the minigame scene
        SceneManager.LoadScene(minigameSceneName);
    }
}