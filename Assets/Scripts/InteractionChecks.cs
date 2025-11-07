using UnityEngine;

public class InteractionChecks : MonoBehaviour
{
    public bool ComicBook;
    public bool Console;
    
    void Start()
    {
        ComicBook = false;
        Console = false;
    }
    public void SetComicTrue()
    {
        ComicBook = true;
    }
    public void SetConsoleTrue()
    {
        Console = true;
    }
}
