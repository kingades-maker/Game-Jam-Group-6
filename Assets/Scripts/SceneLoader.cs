
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName = "SampleScene";
    public TextMeshProUGUI narratorText;
    public GameObject narratorBox;
    public float typewriterSpeed = 0.05f;

    private bool isTyping = false;
    private bool waitingForSpace = false;
    private int sentenceIndex = 0;

    // Split your narrator dialogue into sentences (ending with . or ?)
    private string[] sentences = new string[]
    {
        "Welcome to the story.",
        "This is the beginning of your journey.",
        "Are you ready to begin?",
        "Let's start the adventure!"
    };

    void Start()
    {
        if (narratorBox != null) narratorBox.SetActive(true);
        StartCoroutine(ShowNextSentence());
    }

    void Update()
    {
        // Skip typing animation if Space pressed while typing
        if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        {
            StopAllCoroutines();
            narratorText.text = sentences[sentenceIndex];
            isTyping = false;
            waitingForSpace = true;
        }
        // Show next sentence if Space pressed while waiting
        else if (Input.GetKeyDown(KeyCode.Space) && waitingForSpace)
        {
            sentenceIndex++;

            // Check if there are more sentences
            if (sentenceIndex < sentences.Length)
            {
                waitingForSpace = false;
                StartCoroutine(ShowNextSentence());
            }
            else
            {
                // All dialogue done - load next scene
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    IEnumerator ShowNextSentence()
    {
        isTyping = true;
        narratorText.text = "";

        string currentSentence = sentences[sentenceIndex];

        // Type out letter by letter
        foreach (char letter in currentSentence)
        {
            narratorText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        // Finished typing this sentence
        isTyping = false;
        waitingForSpace = true;
    }
}