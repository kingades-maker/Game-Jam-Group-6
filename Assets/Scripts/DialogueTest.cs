using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueTest : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public GameObject DialogueButton1;
    public GameObject DialogueButton2;

    [Header("Game Objects")]
    public GameObject Player;
    public GameObject NPC;
    public GameObject Comic;
    public GameObject Console; // NEW

    [Header("Settings")]
    public float npcDistance = 5f;
    public float comicDistance = 5f;
    public float consoleDistance = 5f; // NEW
    public float typewriterSpeed = 0.05f;

    [Header("Interaction Checks")]
    public InteractionChecks interactionChecks;

    private bool inDialogue = false;
    private int dialogueStep = 0;
    private bool nearComic = false;
    private bool inComicDialogue = false;
    private int comicDialogueStep = 0;
    private bool nearConsole = false; // NEW
    private bool inConsoleDialogue = false; // NEW
    private int consoleDialogueStep = 0; // NEW
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentPromptText = "";

    void Start()
    {
        if (dialogueBox == null) Debug.LogError("DialogueBox is not assigned!");
        if (Player == null) Debug.LogError("Player is not assigned!");
        if (NPC == null) Debug.LogError("NPC is not assigned!");
        if (Comic == null) Debug.LogError("Comic is not assigned!");
        if (Console == null) Debug.LogError("Console is not assigned!"); // NEW

        dialogueBox.SetActive(false);
        DialogueButton1.SetActive(false);
        DialogueButton2.SetActive(false);
    }

    void Update()
    {
        float distanceToNPC = Vector3.Distance(Player.transform.position, NPC.transform.position);
        float distanceToComic = Vector3.Distance(Player.transform.position, Comic.transform.position);
        float distanceToConsole = Vector3.Distance(Player.transform.position, Console.transform.position); // NEW

        bool nearNPC = distanceToNPC <= npcDistance;
        nearComic = distanceToComic <= comicDistance;
        nearConsole = distanceToConsole <= consoleDistance; // NEW

        // NPC prompt
        if (nearNPC && !inDialogue)
        {
            dialogueBox.SetActive(true);
            if (currentPromptText != "Press Space to talk")
            {
                currentPromptText = "Press Space to talk";
                RestartTypewriter(currentPromptText);
            }
        }
        else if (!nearNPC && !inDialogue && !nearComic && !inComicDialogue && !nearConsole && !inConsoleDialogue)
        {
            HideDialogueUI();
        }

        // Start NPC dialogue
        if (Input.GetKeyDown(KeyCode.Space) && nearNPC && !inDialogue && !inComicDialogue && !inConsoleDialogue)
        {
            dialogueBox.SetActive(true);
            inDialogue = true;
            dialogueStep = 0;
            currentPromptText = "";
            ShowDialogue();
        }

        // Skip typing animation
        if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        {
            SkipTypewriter();
        }

        if (inDialogue && !isTyping)
        {
            DialogueButton1.SetActive(true);
            DialogueButton2.SetActive(true);
        }

        if (!nearNPC && inDialogue)
        {
            EndDialogue();
        }

        // Comic prompt
        if (nearComic && !inComicDialogue && !inDialogue && !inConsoleDialogue)
        {
            dialogueBox.SetActive(true);
            if (currentPromptText != "Press Space to examine comic")
            {
                currentPromptText = "Press Space to examine comic";
                RestartTypewriter(currentPromptText);
            }
        }
        else if (!nearComic && !inComicDialogue && !inDialogue && !nearNPC && !nearConsole && !inConsoleDialogue)
        {
            HideDialogueUI();
        }

        // Start comic dialogue
        if (Input.GetKeyDown(KeyCode.Space) && nearComic && !inComicDialogue && !inDialogue && !inConsoleDialogue)
        {
            dialogueBox.SetActive(true);
            inComicDialogue = true;
            comicDialogueStep = 0;
            currentPromptText = "";
            ShowComicDialogue();
        }

        // Skip typewriter for comic
        if (Input.GetKeyDown(KeyCode.Space) && isTyping && inComicDialogue)
        {
            SkipComicTypewriter();
        }

        if (inComicDialogue && !isTyping)
        {
            DialogueButton1.SetActive(true);
            DialogueButton2.SetActive(false);
        }

        if (!nearComic && inComicDialogue)
        {
            EndComicDialogue();
        }

        // Console prompt (NEW)
        if (nearConsole && !inConsoleDialogue && !inDialogue && !inComicDialogue)
        {
            dialogueBox.SetActive(true);
            if (currentPromptText != "Press Space to examine console")
            {
                currentPromptText = "Press Space to examine console";
                RestartTypewriter(currentPromptText);
            }
        }
        else if (!nearConsole && !inConsoleDialogue && !inDialogue && !nearNPC && !nearComic && !inComicDialogue)
        {
            HideDialogueUI();
        }

        // Start console dialogue (NEW)
        if (Input.GetKeyDown(KeyCode.Space) && nearConsole && !inConsoleDialogue && !inDialogue && !inComicDialogue)
        {
            dialogueBox.SetActive(true);
            inConsoleDialogue = true;
            consoleDialogueStep = 0;
            currentPromptText = "";
            ShowConsoleDialogue();
        }

        // Skip typewriter for console (NEW)
        if (Input.GetKeyDown(KeyCode.Space) && isTyping && inConsoleDialogue)
        {
            SkipConsoleTypewriter();
        }

        if (inConsoleDialogue && !isTyping)
        {
            DialogueButton1.SetActive(true);
            DialogueButton2.SetActive(false);
        }

        if (!nearConsole && inConsoleDialogue)
        {
            EndConsoleDialogue();
        }
    }

    #region Dialogue Logic

    void ShowDialogue()
    {
        string text = dialogueStep switch
        {
            0 => "Julie: Hi!, I'm Julie. What's your name?\n\n1) Melissa\n\n2) ...",
            1 => "Julie: Melissa! Beautiful name. I'm sure we'll get along well!",
            2 => "Julie: Oh... okay then.",
            3 => "Narrator: You haven't found anything to talk about, go find something interesting around the room.",
            _ => ""
        };

        if (dialogueStep is 1 or 2 or 3)
            StartCoroutine(DialogueEndDelay());

        RestartTypewriter(text);
    }

    IEnumerator DialogueEndDelay()
    {
        yield return new WaitForSeconds(8f);
        EndDialogue();
    }

    void MakeChoice(int choice)
    {
        if (dialogueStep == 0)
        {
            dialogueStep = choice == 1 ? 1 : 2;
            ShowDialogue();
        }
    }

    void EndDialogue()
    {
        StopTypewriter();
        inDialogue = false;
        dialogueStep = 0;
        isTyping = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        currentPromptText = "";
    }

    #endregion

    #region Comic Logic

    void ShowComicDialogue()
    {
        string text = comicDialogueStep switch
        {
            0 => "You see a comic book titled 'Space Adventures'.\n\n1) Read it",
            1 => "You flip through the comic. The art is vibrant and the story is exciting!",
            _ => ""
        };

        if (comicDialogueStep == 1)
            StartCoroutine(ComicDialogueEndDelay());

        RestartTypewriter(text);
    }

    IEnumerator ComicDialogueEndDelay()
    {
        yield return new WaitForSeconds(6f);
        EndComicDialogue();
    }

    void MakeComicChoice(int choice)
    {
        if (comicDialogueStep == 0 && choice == 1)
        {
            comicDialogueStep = 1;
            ShowComicDialogue();
            if (interactionChecks != null)
                interactionChecks.ComicBook = true;
        }
    }

    void EndComicDialogue()
    {
        StopTypewriter();
        inComicDialogue = false;
        comicDialogueStep = 0;
        isTyping = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        currentPromptText = "";
    }

    #endregion

    #region Console Logic

    void ShowConsoleDialogue()
    {
        string text = consoleDialogueStep switch
        {
            0 => "You see a game console. It looks like it's running a cool project.\n\n1) Take a closer look",
            1 => "Melissa: Wow, this looks really interesting! Maybe someone here would love to see this.",
            _ => ""
        };

        if (consoleDialogueStep == 1)
            StartCoroutine(ConsoleDialogueEndDelay());

        RestartTypewriter(text);
    }

    IEnumerator ConsoleDialogueEndDelay()
    {
        yield return new WaitForSeconds(6f);
        EndConsoleDialogue();
    }

    void MakeConsoleChoice(int choice)
    {
        if (consoleDialogueStep == 0 && choice == 1)
        {
            consoleDialogueStep = 1;
            ShowConsoleDialogue();
            if (interactionChecks != null)
                interactionChecks.Console = true; // You may need to add this variable to InteractionChecks
        }
    }

    void EndConsoleDialogue()
    {
        StopTypewriter();
        inConsoleDialogue = false;
        consoleDialogueStep = 0;
        isTyping = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        currentPromptText = "";
    }

    #endregion

    #region Typewriter Logic

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

    void RestartTypewriter(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    void StopTypewriter()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = null;
        isTyping = false;
    }

    void SkipTypewriter()
    {
        StopTypewriter();
        if (!inDialogue && !inComicDialogue && !inConsoleDialogue)
        {
            dialogueText.text = currentPromptText;
        }
        else if (inDialogue)
        {
            ShowDialogue();
        }
        else if (inComicDialogue)
        {
            ShowComicDialogue();
        }
        else if (inConsoleDialogue)
        {
            ShowConsoleDialogue();
        }
    }

    void SkipComicTypewriter()
    {
        StopTypewriter();
        ShowComicDialogue();
    }

    void SkipConsoleTypewriter()
    {
        StopTypewriter();
        ShowConsoleDialogue();
    }

    #endregion

    #region UI Helpers

    void HideDialogueUI()
    {
        dialogueBox.SetActive(false);
        currentPromptText = "";
        StopTypewriter();
        DialogueButton1.SetActive(false);
        DialogueButton2.SetActive(false);
    }

    #endregion

    #region Button Functions

    public void dialogue1()
    {
        if (inDialogue && !inComicDialogue && !inConsoleDialogue)
        {
            MakeChoice(1);
            Debug.Log("Dialogue 1 triggered");
        }
    }

    public void dialogue2()
    {
        if (inDialogue && !inComicDialogue && !inConsoleDialogue)
        {
            MakeChoice(2);
            Debug.Log("Dialogue 2 triggered");
        }
    }

    public void ComicDialogue1()
    {
        if (inComicDialogue && !inDialogue && !inConsoleDialogue)
        {
            MakeComicChoice(1);
            Debug.Log("Comic Dialogue 1 triggered");
        }
    }

    public void ConsoleDialogue1()
    {
        if (inConsoleDialogue && !inDialogue && !inComicDialogue)
        {
            MakeConsoleChoice(1);
            Debug.Log("Console Dialogue 1 triggered");
        }
    }

    #endregion
}