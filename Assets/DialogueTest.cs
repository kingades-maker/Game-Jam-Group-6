using UnityEngine;
using System.Collections; // Required for Coroutines
using TMPro; // Required for TextMeshPro
using UnityEngine.UI; // <-- NEW: Required for Button components!

// =================================================================
// 1. NEW DATA STRUCTURES (Outside the main MonoBehaviour class)
// =================================================================
// These classes allow you to structure your dialogue and choices in the Inspector.

[System.Serializable]
public class DialogueEntry
{
    [TextArea(3, 5)]
    public string message;
    public Choice[] choices;

    // Helper to easily check if choices should be shown
    public bool HasChoices => choices != null && choices.Length > 0;
}

[System.Serializable]
public class Choice
{
    public string buttonText;       // Text shown on the button (e.g., "Take the Quest")
    public int nextMessageIndex;    // The index in the 'dialogue' array to jump to
}


// =================================================================
// 2. MAIN MONOBEHAVIOUR CLASS
// =================================================================

public class DialogueTest : MonoBehaviour
{
    // --- Existing UI & Environment References ---
    public TMP_Text myText;
    public GameObject textBox;
    public GameObject Player;
    public GameObject NPC;
    public float npcDistance;

    // --- Typewriter Settings ---
    [Header("Typewriter Settings")]
    public float typingSpeed = 0.05f; // Time in seconds between each character
    private Coroutine typingCoroutine;

    // --- NEW: Choice UI References (ASSIGN IN INSPECTOR) ---
    [Header("Choice UI References")]
    public GameObject choicePanel;      // The Panel that holds all the buttons
    public Button[] choiceButtons;      // All the individual Button components

    // --- NEW: Dialogue Data (ASSIGN IN INSPECTOR) ---
    // This replaces the old 'string[] messages'
    [Header("Dialogue Data")]
    public DialogueEntry[] dialogue;

    private int messageIndex = 0; // Current index in the 'dialogue' array

    // Called once before the first execution of Update
    void Start()
    {
        textBox.SetActive(false);
        // Ensure the choice panel is also hidden at the start
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isInRange = Vector3.Distance(Player.transform.position, NPC.transform.position) < npcDistance;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // --- Case 1: Initial Dialogue Trigger ---
            if (!textBox.activeSelf && isInRange)
            {
                messageIndex = 0; // Start at the beginning
                textBox.SetActive(true);
                typingCoroutine = StartCoroutine(TypeText(dialogue[messageIndex].message));
            }

            // --- Case 2: Advancing Dialogue / Skipping ---
            else if (textBox.activeSelf)
            {
                // CRITICAL CHECK: Block 'E' if choices are visible!
                if (choicePanel.activeSelf)
                {
                    return; // Player must click a button
                }

                // Sub-Case 2a: Typing is still running (Skip it)
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    myText.maxVisibleCharacters = myText.text.Length;
                    typingCoroutine = null;
                    return; // Return so the script doesn't try to advance the dialogue immediately
                }
                // Sub-Case 2b: Typing is finished (Advance to next message)
                else
                {
                    // Check if there is a next message in the linear path (since no choices were presented)
                    if (messageIndex + 1 < dialogue.Length)
                    {
                        messageIndex++;
                        typingCoroutine = StartCoroutine(TypeText(dialogue[messageIndex].message));
                    }
                    else
                    {
                        // End Dialogue 
                        messageIndex = 0;
                        textBox.SetActive(false);
                    }
                }
            }
        }
    }

    // =================================================================
    // TYPEWRITER COROUTINE (Updated to check for choices)
    // =================================================================

    private IEnumerator TypeText(string textToDisplay)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Hide choices before starting a new message
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }

        myText.text = textToDisplay;
        myText.maxVisibleCharacters = 0;

        int totalCharacters = textToDisplay.Length;

        while (myText.maxVisibleCharacters < totalCharacters)
        {
            myText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null; // Typing finished

        // NEW: Check if this finished message requires choices
        DialogueEntry currentEntry = dialogue[messageIndex];
        if (currentEntry.HasChoices)
        {
            DisplayChoices(currentEntry.choices);
        }
    }

    // =================================================================
    // CHOICE HANDLERS
    // =================================================================

    private void DisplayChoices(Choice[] choices)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button button = choiceButtons[i];

            if (i < choices.Length)
            {
                // Set up and show the button
                button.gameObject.SetActive(true);

                // Find the TextMeshPro component on the button's child
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
                buttonText.text = choices[i].buttonText;

                // Remove old listeners and add the new one
                button.onClick.RemoveAllListeners();

                int nextIndex = choices[i].nextMessageIndex;
                // Lambda function calls OnChoiceSelected with the index from the data
                button.onClick.AddListener(() => OnChoiceSelected(nextIndex));
            }
            else
            {
                // Hide any unused buttons
                button.gameObject.SetActive(false);
            }
        }
    }

    public void OnChoiceSelected(int nextIndex)
    {
        // 1. Hide the choices panel
        choicePanel.SetActive(false);

        // 2. Set the messageIndex to the result of the choice
        messageIndex = nextIndex;

        // 3. Start the new branch sequence
        if (messageIndex < dialogue.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(dialogue[messageIndex].message));
        }
        else
        {
            // End Dialogue (if the choice led to an end state)
            messageIndex = 0;
            textBox.SetActive(false);
        }
    }
}