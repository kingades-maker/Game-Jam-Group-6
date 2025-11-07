using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueTest : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public GameObject Player;
    public GameObject NPC;
    public float npcDistance = 5f;
    public float typewriterSpeed = 0.05f; // Time between each character

    private bool inDialogue = false;
    private int dialogueStep = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentPromptText = ""; // Track the current prompt

    void Start()
    {
        Debug.Log("Script started!");
        if (dialogueBox == null) Debug.LogError("DialogueBox is not assigned!");
        if (Player == null) Debug.LogError("Player is not assigned!");
        if (NPC == null) Debug.LogError("NPC is not assigned!");

        dialogueBox.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(Player.transform.position, NPC.transform.position);
        bool nearNPC = distance <= npcDistance;

        Debug.Log("Distance to NPC: " + distance + " | Near NPC: " + nearNPC);

        if (nearNPC && !inDialogue)
        {
            Debug.Log("Should show prompt!");
            dialogueBox.SetActive(true);

            // Only start typing if we're not already showing this prompt
            if (currentPromptText != "Press Space to talk")
            {
                currentPromptText = "Press Space to talk";
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeText(currentPromptText));
            }
        }
        else if (!nearNPC && !inDialogue)
        {
            dialogueBox.SetActive(false);
            currentPromptText = ""; // Reset when leaving
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && nearNPC && !inDialogue)
        {
            Debug.Log("Space pressed! Starting dialogue");
            dialogueBox.SetActive(true);
            inDialogue = true;
            dialogueStep = 0;
            currentPromptText = ""; // Reset prompt tracking
            ShowDialogue();
        }

        // Skip typing animation if Space is pressed while typing
        if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        {
            SkipTypewriter();
        }

        if (inDialogue && !isTyping) // Only allow choices when not typing
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MakeChoice(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MakeChoice(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                MakeChoice(3);
            }
        }

        if (!nearNPC && inDialogue)
        {
            EndDialogue();
        }
    }

    void ShowDialogue()
    {
        string text = "";

        switch (dialogueStep)
        {
            case 0:
                text = "Julie: Hi!, I'm Julie. What's your name?\n\n1) Melissa\n\n2) ...";
                break;
            case 1:
                text = "Julie: Melissa! Beautiful name. I'm sure we'll get along well!";
                break;
            case 2:
                text = "Julie: Oh... okay then.";
                break;
        }

        // Start typewriter effect
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in fullText)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
    }

    void SkipTypewriter()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        isTyping = false;

        // Show full text based on current state
        if (!inDialogue)
        {
            dialogueText.text = currentPromptText;
        }
        else
        {
            // Show full text based on current dialogue step
            switch (dialogueStep)
            {
                case 0:
                    dialogueText.text = "Julie: Hi!, I'm Julie. What's your name?\n\n1) Melissa\n\n2) ...";
                    break;
                case 1:
                    dialogueText.text = "Julie: Melissa! Beautiful name. I'm sure we'll get along well!";
                    break;
                case 2:
                    dialogueText.text = "Julie: Oh... okay then.";
                    break;
            }
        }
    }

    void MakeChoice(int choice)
    {
        if (dialogueStep == 0)
        {
            if (choice == 1) dialogueStep = 1;
            else if (choice == 2) dialogueStep = 2;
            else if (choice == 3) dialogueStep = 3;
            ShowDialogue();
        }
        else if (dialogueStep == 1 || dialogueStep == 2)
        {
            if (choice == 1) dialogueStep = 4;
            else if (choice == 2) dialogueStep = 5;
            else if (choice == 3) EndDialogue();
            ShowDialogue();
        }
        else if (dialogueStep == 3)
        {
            EndDialogue();
        }
        else if (dialogueStep == 4)
        {
            if (choice == 1) dialogueStep = 6;
            else if (choice == 2) dialogueStep = 7;
            else if (choice == 3) EndDialogue();
            ShowDialogue();
        }
        else if (dialogueStep == 5 || dialogueStep == 6 || dialogueStep == 7)
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        inDialogue = false;
        dialogueStep = 0;
        isTyping = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        currentPromptText = "";
    }
}